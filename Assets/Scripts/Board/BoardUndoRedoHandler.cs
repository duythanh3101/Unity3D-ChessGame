using System;
using System.Collections.Generic;
using Extension.ExtraTypes;
using UnityEngine;

public class BoardUndoRedoHandler : MonoBehaviour
{
    [SerializeField]
    private BoardManager boardManager;

    [SerializeField]
    private MainCamera mainCamera;

    private Stack<ChessMove> allPreviousMoves;
    private Stack<ChessMove> undoMoves;

    protected virtual void Awake ()
    {
        allPreviousMoves = new Stack<ChessMove>();
        undoMoves = new Stack<ChessMove>();

        boardManager.OnAChessPieceMoved += OnAChessPieceMovedHandle;
    }

    private void OnAChessPieceMovedHandle(ChessMove chessMove)
    {
        allPreviousMoves.Push(chessMove);
    }

    public void Undo ()
    {
        if (allPreviousMoves == null || undoMoves == null)
            return;

        if (allPreviousMoves.Count < 1)
            return;

        ChessMove previousMove = allPreviousMoves.Pop();
        undoMoves.Push(previousMove);

        MoveUndo(previousMove);
    }

    private void MoveUndo(ChessMove chessMove)
    {
        ChessPiece selectedChessman = chessMove.MovedPiece;
        Vector3 newPosition = boardManager.GetTileCenter(chessMove.BaseCoordinate.x, chessMove.BaseCoordinate.y);

        if (chessMove.OldPawnPiece != null)
        {
            chessMove.MovedPiece.gameObject.SetActive(false);
            chessMove.OldPawnPiece.gameObject.SetActive(true);

            boardManager.ActiveChessPieces.Remove(chessMove.MovedPiece.gameObject);
            boardManager.ActiveChessPieces.Add(chessMove.OldPawnPiece.gameObject);

            selectedChessman = chessMove.OldPawnPiece;
            newPosition = boardManager.GetTileCenter(selectedChessman.CurrentX, selectedChessman.CurrentY);
        }

        if (chessMove.EatenPiece != null)
        {
            chessMove.EatenPiece.gameObject.SetActive(true);
            boardManager.ActiveChessPieces.Add(chessMove.EatenPiece.gameObject);

            boardManager.ChessPieces[chessMove.DesCoordinate.x, chessMove.DesCoordinate.y] = chessMove.EatenPiece;
        }
        else
        {
            boardManager.ChessPieces[chessMove.DesCoordinate.x, chessMove.DesCoordinate.y] = null;
        }

        selectedChessman.gameObject.transform.position = newPosition;

        boardManager.IsWhiteTurn = selectedChessman.isWhite;

        boardManager.ChessPieces[chessMove.BaseCoordinate.x, chessMove.BaseCoordinate.y] = selectedChessman;

        selectedChessman.SetPosition(chessMove.BaseCoordinate.x, chessMove.BaseCoordinate.y);

        mainCamera.RotateMainCamera(boardManager.IsWhiteTurn);
    }

    public void Redo ()
    {
        if (allPreviousMoves == null || undoMoves == null)
            return;

        if (undoMoves.Count < 1)
            return;

        ChessMove lastUndoMove = undoMoves.Pop();
        allPreviousMoves.Push(lastUndoMove);

        MoveRedo(lastUndoMove);
    }

    private void MoveRedo(ChessMove lastUndoMove)
    {
        ChessPiece selectedChessman = lastUndoMove.MovedPiece;
        Vector3 newPosition = boardManager.GetTileCenter(lastUndoMove.BaseCoordinate.x, lastUndoMove.BaseCoordinate.y);

        if (lastUndoMove.OldPawnPiece != null)
        {
            lastUndoMove.MovedPiece.gameObject.SetActive(true);
            lastUndoMove.OldPawnPiece.gameObject.SetActive(false);

            boardManager.ActiveChessPieces.Add(lastUndoMove.MovedPiece.gameObject);
            boardManager.ActiveChessPieces.Remove(lastUndoMove.OldPawnPiece.gameObject);
        }

        if (lastUndoMove.EatenPiece != null)
        {
            lastUndoMove.EatenPiece.gameObject.SetActive(false);
            boardManager.ActiveChessPieces.Remove(lastUndoMove.EatenPiece.gameObject);
        }

        boardManager.ChessPieces[lastUndoMove.BaseCoordinate.x, lastUndoMove.BaseCoordinate.y] = null;

        selectedChessman.gameObject.transform.position = boardManager.GetTileCenter(lastUndoMove.DesCoordinate.x, lastUndoMove.DesCoordinate.y);

        boardManager.IsWhiteTurn = !selectedChessman.isWhite;

        boardManager.ChessPieces[lastUndoMove.DesCoordinate.x, lastUndoMove.DesCoordinate.y] = selectedChessman;

        selectedChessman.SetPosition(lastUndoMove.DesCoordinate.x, lastUndoMove.DesCoordinate.y);

        mainCamera.RotateMainCamera(boardManager.IsWhiteTurn);
    }
}