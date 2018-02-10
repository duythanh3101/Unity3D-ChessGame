using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    public enum ChessType
    {
        King,
        Queen,
        Bishop,
        Knight,
        Pawn,
        Rook,
        None = 7
    }

    public int CurrentX { set; get; }
    public int CurrentY { set; get; }

    public ChessType chessType;
    public bool isWhite;

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }
    public ChessPiece()
    {

    }

    public ChessPiece(ChessType ChessType, bool IsWhite)
    {
        CurrentX = 0;
        CurrentY = 0;
        chessType = ChessType;
        isWhite = IsWhite;
    }

    public ChessPiece(int xCoor, int yCoor, ChessType ChessType, bool IsWhite)
    {
        CurrentX = xCoor;
        CurrentY = yCoor;
        chessType = ChessType;
        isWhite = IsWhite;
    }

    public virtual bool[,] AllPossibleMoves()
    {
        return new bool[8, 8];
    }

    public bool Move(int x, int y, ref bool[,] AllPossibleMoves)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            ChessPiece ChessPiece = BoardManager.Instance.ChessPieces[x, y];
            if (ChessPiece == null)
                AllPossibleMoves[x, y] = true;
            else
            {
                if (isWhite != ChessPiece.isWhite)
                    AllPossibleMoves[x, y] = true;
                return true;
            }
        }
        return false;
    }
}
