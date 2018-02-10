using System.Collections;
using UnityEngine;
using System;

public class RookMoveLogic : ChessPiece
{
    public override bool[,] AllPossibleMoves()
    {
        bool[,] allPossibleMoves = new bool[8, 8];

        int CurrentCoordinate;

        // Right
        CurrentCoordinate = CurrentX;
        while (true)
        {
            CurrentCoordinate++;
            if (CurrentCoordinate >= 8)
                break;

            if (Move(CurrentCoordinate, CurrentY, ref allPossibleMoves))
                break;
        }

        // Left
        CurrentCoordinate = CurrentX;
        while (true)
        {
            CurrentCoordinate--;
            if (CurrentCoordinate < 0)
                break;

            if (Move(CurrentCoordinate, CurrentY, ref allPossibleMoves))
                break;
        }

        // Up
        CurrentCoordinate = CurrentY;
        while (true)
        {
            CurrentCoordinate++;
            if (CurrentCoordinate >= 8)
                break;

            if (Move(CurrentX, CurrentCoordinate, ref allPossibleMoves))
                break;
        }

        // Down
        CurrentCoordinate = CurrentY;
        while (true)
        {
            CurrentCoordinate--;
            if (CurrentCoordinate < 0)
                break;

            if (Move(CurrentX, CurrentCoordinate, ref allPossibleMoves))
                break;

        }

        return allPossibleMoves;
    }
}
