using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChessData
{
    public bool[,] OnChessBoard;
    public Queue<ChessPiece.ChessType> PieceType;
    public Queue<bool> OnWhiteTurn;
    public bool IsWhiteTurn;

    public ChessData(ChessPiece[,] chessPieces)
    {
        OnChessBoard = new bool[8, 8];
        PieceType = new Queue<ChessPiece.ChessType>();
        OnWhiteTurn = new Queue<bool>();
        IsWhiteTurn = BoardManager.Instance.IsWhiteTurn;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (chessPieces[x, y] != null)
                {
                    OnChessBoard[x, y] = true;
                    PieceType.Enqueue(chessPieces[x, y].chessType);
                    OnWhiteTurn.Enqueue(chessPieces[x, y].isWhite);
                }
            }
        }
    }
}