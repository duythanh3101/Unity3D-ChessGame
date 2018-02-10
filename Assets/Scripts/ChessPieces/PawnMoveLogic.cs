using System.Collections;
using UnityEngine;
using System;

public class PawnMoveLogic : ChessPiece
{
    public override bool[,] AllPossibleMoves()
    {
        bool[,] allPossibleMoves = new bool[8, 8];

        ChessPiece c, c2;

        int[] e = BoardManager.Instance.EnPassantMove;

        if (isWhite)
        {
            ////// White team move //////

            // Diagonal left
            if (CurrentX != 0 && CurrentY != 7)
            {
                if(e[0] == CurrentX -1 && e[1] == CurrentY + 1)
                    allPossibleMoves[CurrentX - 1, CurrentY + 1] = true;

                c = BoardManager.Instance.ChessPieces[CurrentX - 1, CurrentY + 1];
                if (c != null && !c.isWhite)
                    allPossibleMoves[CurrentX - 1, CurrentY + 1] = true;
            }

            // Diagonal right
            if (CurrentX != 7 && CurrentY != 7)
            {
                if (e[0] == CurrentX + 1 && e[1] == CurrentY + 1)
                    allPossibleMoves[CurrentX + 1, CurrentY + 1] = true;

                c = BoardManager.Instance.ChessPieces[CurrentX + 1, CurrentY + 1];
                if (c != null && !c.isWhite)
                    allPossibleMoves[CurrentX + 1, CurrentY + 1] = true;
            }

            // Middle
            if (CurrentY != 7)
            {
                c = BoardManager.Instance.ChessPieces[CurrentX, CurrentY + 1];
                if (c == null)
                    allPossibleMoves[CurrentX, CurrentY + 1] = true;
            }

            // Middle on first move
            if (CurrentY == 1)
            {
                c = BoardManager.Instance.ChessPieces[CurrentX, CurrentY + 1];
                c2 = BoardManager.Instance.ChessPieces[CurrentX, CurrentY + 2];
                if (c == null && c2 == null)
                    allPossibleMoves[CurrentX, CurrentY + 2] = true;
            }
        }
        else
        {
            ////// Black team move //////

            // Diagonal left
            if (CurrentX != 0 && CurrentY != 0)
            {
                if (e[0] == CurrentX - 1 && e[1] == CurrentY - 1)
                    allPossibleMoves[CurrentX - 1, CurrentY - 1] = true;

                c = BoardManager.Instance.ChessPieces[CurrentX - 1, CurrentY - 1];
                if (c != null && c.isWhite)
                    allPossibleMoves[CurrentX - 1, CurrentY - 1] = true;
            }

            // Diagonal right
            if (CurrentX != 7 && CurrentY != 0)
            {
                if (e[0] == CurrentX + 1 && e[1] == CurrentY - 1)
                    allPossibleMoves[CurrentX + 1, CurrentY - 1] = true;

                c = BoardManager.Instance.ChessPieces[CurrentX + 1, CurrentY - 1];
                if (c != null && c.isWhite)
                    allPossibleMoves[CurrentX + 1, CurrentY - 1] = true;
            }

            // Middle
            if (CurrentY != 0)
            {
                c = BoardManager.Instance.ChessPieces[CurrentX, CurrentY - 1];
                if (c == null)
                    allPossibleMoves[CurrentX, CurrentY - 1] = true;
            }

            // Middle on first move
            if (CurrentY == 6)
            {
                c = BoardManager.Instance.ChessPieces[CurrentX, CurrentY - 1];
                c2 = BoardManager.Instance.ChessPieces[CurrentX, CurrentY - 2];
                if (c == null && c2 == null)
                    allPossibleMoves[CurrentX, CurrentY - 2] = true;
            }
        }

        return allPossibleMoves;
    }
}
