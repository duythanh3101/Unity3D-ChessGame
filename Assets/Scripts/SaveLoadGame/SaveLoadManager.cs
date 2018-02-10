using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager: MonoBehaviour
{
    [SerializeField]
    private BoardManager boardManager;

    [SerializeField]
    private MainCamera mainCamera;

    public void SaveButton()
    {
        DataSaver.SaveChessBoard(boardManager.ChessPieces);
        Debug.Log("Save succeed!");
    }

    public void LoadData()
    {
        ChessData data = DataSaver.LoadChessBoard();
        ChessPiece[,] ChessPieces = new ChessPiece[8, 8];
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (data.OnChessBoard[x, y])
                {
                    ChessPiece.ChessType ChessType = data.PieceType.Dequeue();
                    bool isWhite = data.OnWhiteTurn.Dequeue();
                    int index = FindIndexPositiionOfChess(ChessType, isWhite);

                    Vector3 position = boardManager.GetTileCenter(x, y);
                    GameObject go;
                    if (isWhite)
                    {
                        go = Instantiate(boardManager.chessPiecePrefabs[index], position, 
                            boardManager.whiteOrientation) as GameObject;
                    }
                    else
                    {
                        go = Instantiate(boardManager.chessPiecePrefabs[index], position, boardManager.
                            blackOrientation) as GameObject;
                    }

                    go.transform.SetParent(transform);
                    ChessPieces[x, y] = go.GetComponent<ChessPiece>();
                    ChessPieces[x, y].SetPosition(x, y);
                    boardManager.ActiveChessPieces.Add(go);
                }
                else
                {
                    ChessPieces[x, y] = null;
                }
            }
        }
        boardManager.IsWhiteTurn = data.IsWhiteTurn;
        boardManager.ChessPieces = ChessPieces.Clone() as ChessPiece[,];
        mainCamera.RotateMainCamera(boardManager.IsWhiteTurn);
    }

    public void LoadButton()
    {
        boardManager.ClearAllChessInBoard();
        LoadData();
        Debug.Log("Load succeed!");
    }

    private int FindIndexPositiionOfChess(ChessPiece.ChessType chessType, bool isWhite)
    {
        if (chessType == ChessPiece.ChessType.King && isWhite)
            return 0;
        else if (chessType == ChessPiece.ChessType.Queen && isWhite)
            return 1;
        else if (chessType == ChessPiece.ChessType.Rook && isWhite)
            return 2;
        else if (chessType == ChessPiece.ChessType.Bishop && isWhite)
            return 3;
        else if (chessType == ChessPiece.ChessType.Knight && isWhite)
            return 4;
        else if (chessType == ChessPiece.ChessType.Pawn && isWhite)
            return 5;
        else if (chessType == ChessPiece.ChessType.King && !isWhite)
            return 6;
        else if (chessType == ChessPiece.ChessType.Queen && !isWhite)
            return 7;
        else if (chessType == ChessPiece.ChessType.Rook && !isWhite)
            return 8;
        else if (chessType == ChessPiece.ChessType.Bishop && !isWhite)
            return 9;
        else if (chessType == ChessPiece.ChessType.Knight && !isWhite)
            return 10;

        return 11;
    }

}