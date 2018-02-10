using Extension.ExtraTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private BoardHighlights boardHighlights;

    public static BoardManager Instance { get; set; }
    private bool[,] AllowedMoves { get; set; }

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int mousePositionX = -1;
    private int mousePositionY = -1;

    public List<GameObject> chessPiecePrefabs;
    public List<GameObject> ActiveChessPieces { get; set; }

    public Quaternion whiteOrientation { get; set; }
    public Quaternion blackOrientation { get; set; }

    public ChessPiece[,] ChessPieces { get; set; }
    private ChessPiece selectedChessPiece;

    public bool IsWhiteTurn { get; set; }

    private Material previousMat;
    public Material selectedMat;

    public int[] EnPassantMove { set; get; }

    [SerializeField]
    private MainCamera mainCamera;

    public Action<ChessMove> OnAChessPieceMoved;
    public Action<bool> OnGameEnd;

    void Start()
    {
        OnAChessPieceMoved += _ => { };
        OnGameEnd += _ => { };

        IsWhiteTurn = true;
        Instance = this;
        SpawnAllChessPieces();
        EnPassantMove = new int[2] { -1, -1 };

        whiteOrientation = Quaternion.Euler(0, 270, 0);
        blackOrientation = Quaternion.Euler(0, 90, 0);
}

    void Update()
    {
        UpdateMousePosition();

        if (Input.GetMouseButtonDown(0))
        {
            if (mousePositionX >= 0 && mousePositionY >= 0)
            {
                if (selectedChessPiece == null)
                {
                    // Select the chessman
                    SelectChessPiece(mousePositionX, mousePositionY);
                }
                else
                {
                    // Move the chessman
                    MoveChessPiece(mousePositionX, mousePositionY);
                }
            }
        }
    }

    private void SelectChessPiece(int x, int y)
    {
        if (ChessPieces[x, y] == null)
            return;

        if (ChessPieces[x, y].isWhite != IsWhiteTurn)
            return;

        bool hasAtLeastOneMove = false;

        AllowedMoves = ChessPieces[x, y].AllPossibleMoves();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (AllowedMoves[i, j])
                {
                    hasAtLeastOneMove = true;
                    i = 8;
                    break;
                }
            }
        }

        if (!hasAtLeastOneMove)
            return;

        selectedChessPiece = ChessPieces[x, y];
        previousMat = selectedChessPiece.GetComponent<MeshRenderer>().material;
        selectedMat.mainTexture = previousMat.mainTexture;
        selectedChessPiece.GetComponent<MeshRenderer>().material = selectedMat;

        boardHighlights.HighLightAllowedMoves(AllowedMoves, offset);
    }

    public void MoveChessPiece(int x, int y)
    {
        if (AllowedMoves[x, y])
        {
            ChessPiece eatenChessman = ChessPieces[x, y];
            ChessPiece oldPawnPiece = null;

            if (eatenChessman != null && eatenChessman.isWhite != IsWhiteTurn)
            {
                // Capture a piece

                if (eatenChessman.chessType == ChessPiece.ChessType.King)
                {
                    // End the game
                    EndGame();
                    return;
                }

                ActiveChessPieces.Remove(eatenChessman.gameObject);
                eatenChessman.gameObject.SetActive(false);
            }
            if (x == EnPassantMove[0] && y == EnPassantMove[1])
            {
                if (IsWhiteTurn)
                    eatenChessman = ChessPieces[x, y - 1];
                else
                    eatenChessman = ChessPieces[x, y + 1];

                ActiveChessPieces.Remove(eatenChessman.gameObject);
                eatenChessman.gameObject.SetActive(false);
            }

            EnPassantMove[0] = -1;
            EnPassantMove[1] = -1;

            if (selectedChessPiece.chessType == ChessPiece.ChessType.Pawn)
            {
                if(y == 7) // White Promotion
                {
                    ActiveChessPieces.Remove(selectedChessPiece.gameObject);
                    selectedChessPiece.gameObject.SetActive(false);

                    oldPawnPiece = selectedChessPiece;

                    SpawnChessPieceAtCoordinate(1, x, y, true);
                    selectedChessPiece = ChessPieces[x, y];
                }

                else if (y == 0) // Black Promotion
                {
                    ActiveChessPieces.Remove(selectedChessPiece.gameObject);
                    selectedChessPiece.gameObject.SetActive(false);

                    oldPawnPiece = selectedChessPiece;

                    SpawnChessPieceAtCoordinate(7, x, y, false);
                    selectedChessPiece = ChessPieces[x, y];
                }

                EnPassantMove[0] = x;
                if (selectedChessPiece.CurrentY == 1 && y == 3)
                {
                    EnPassantMove[1] = y - 1;
                }
                else if (selectedChessPiece.CurrentY == 6 && y == 4)
                {
                    EnPassantMove[1] = y + 1;
                }
            }

            ChessMove newChessMove = new ChessMove(selectedChessPiece, 
                                                   new IntVector2(selectedChessPiece.CurrentX, selectedChessPiece.CurrentY),
                                                   new IntVector2(x, y), 
                                                   eatenChessman,
                                                   oldPawnPiece);

            OnAChessPieceMoved(newChessMove);

            ChessPieces[selectedChessPiece.CurrentX, selectedChessPiece.CurrentY] = null;
            ChessPieces[x, y] = selectedChessPiece;
            selectedChessPiece.transform.position = GetTileCenter(x, y);
            selectedChessPiece.SetPosition(x, y);
            IsWhiteTurn = !IsWhiteTurn;

            mainCamera.RotateMainCamera(IsWhiteTurn);
        }

        selectedChessPiece.GetComponent<MeshRenderer>().material = previousMat;

        boardHighlights.HideHighlights();
        selectedChessPiece = null;
    }

    private void UpdateMousePosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("ChessPlane")))
        {
            mousePositionX = (int)(hit.point.x + Math.Abs(offset.x));
            mousePositionY = (int)(hit.point.z + Math.Abs(offset.z));
            Debug.Log(mousePositionX + "," + mousePositionY);
        }
        else
        {
            mousePositionX = -1;
            mousePositionY = -1;
        }
    }

    public void SpawnChessPieceAtCoordinate(int index, int x, int y, bool isWhite)
    {
        Vector3 position = GetTileCenter(x, y);
        GameObject go;

        if (isWhite)
        {
            go = Instantiate(chessPiecePrefabs[index], position, whiteOrientation) as GameObject;
        }
        else
        {
            go = Instantiate(chessPiecePrefabs[index], position, blackOrientation) as GameObject;
        }

        go.transform.SetParent(transform);
        ChessPieces[x, y] = go.GetComponent<ChessPiece>();
        ChessPieces[x, y].SetPosition(x, y);
        ActiveChessPieces.Add(go);
    }

    public Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = offset;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;

        return origin;
    }

    private void SpawnAllChessPieces()
    {
        ActiveChessPieces = new List<GameObject>();
        ChessPieces = new ChessPiece[8, 8];

        /////// White ///////

        // King
        SpawnChessPieceAtCoordinate(0, 3, 0, true);

        // Queen
        SpawnChessPieceAtCoordinate(1, 4, 0, true);

        // Rooks
        SpawnChessPieceAtCoordinate(2, 0, 0, true);
        SpawnChessPieceAtCoordinate(2, 7, 0, true);

        // Bishops
        SpawnChessPieceAtCoordinate(3, 2, 0, true);
        SpawnChessPieceAtCoordinate(3, 5, 0, true);

        // Knights
        SpawnChessPieceAtCoordinate(4, 1, 0, true);
        SpawnChessPieceAtCoordinate(4, 6, 0, true);

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessPieceAtCoordinate(5, i, 1, true);
        }


        /////// Black ///////

        // King
        SpawnChessPieceAtCoordinate(6, 4, 7, false);

        // Queen
        SpawnChessPieceAtCoordinate(7, 3, 7, false);

        // Rooks
        SpawnChessPieceAtCoordinate(8, 0, 7, false);
        SpawnChessPieceAtCoordinate(8, 7, 7, false);

        // Bishops
        SpawnChessPieceAtCoordinate(9, 2, 7, false);
        SpawnChessPieceAtCoordinate(9, 5, 7, false);

        // Knights
        SpawnChessPieceAtCoordinate(10, 1, 7, false);
        SpawnChessPieceAtCoordinate(10, 6, 7, false);

        // Pawns
        for (int i = 0; i < 8; i++)
        {
            SpawnChessPieceAtCoordinate(11, i, 6, false);
        }
    }

    private void EndGame()
    {
        OnGameEnd(IsWhiteTurn);

        foreach (GameObject go in ActiveChessPieces)
        {
            Destroy(go);
        }

        IsWhiteTurn = true;
        boardHighlights.HideHighlights();
        SpawnAllChessPieces();
    }

    public void ClearAllChessInBoard()
    {
        foreach (GameObject go in ActiveChessPieces)
        {
            Destroy(go);
        }
    }
}