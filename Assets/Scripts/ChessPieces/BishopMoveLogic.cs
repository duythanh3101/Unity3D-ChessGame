using System.Collections;
using UnityEngine;
using System;

public class BishopMoveLogic : ChessPiece
{
    public override bool[,] AllPossibleMoves()
    {
        bool[,] allPossibleMoves = new bool[8, 8];

        int XCoordinate, YCoordinate;

        // Top left
        XCoordinate = CurrentX;
        YCoordinate = CurrentY;
        while (true)
        {
            XCoordinate--;
            YCoordinate++;
            if (XCoordinate < 0 || YCoordinate >= 8)
                break;

            if (Move(XCoordinate, YCoordinate, ref allPossibleMoves))
                break;
        }

        // Top right
        XCoordinate = CurrentX;
        YCoordinate = CurrentY;
        while (true)
        {
            XCoordinate++;
            YCoordinate++;
            if (XCoordinate >= 8 || YCoordinate >= 8)
                break;

            if (Move(XCoordinate, YCoordinate, ref allPossibleMoves))
                break;
        }

        // Down left
        XCoordinate = CurrentX;
        YCoordinate = CurrentY;
        while (true)
        {
            XCoordinate--;
            YCoordinate--;
            if (XCoordinate < 0 || YCoordinate < 0)
                break;

            if (Move(XCoordinate, YCoordinate, ref allPossibleMoves))
                break;
        }

        // Down right
        XCoordinate = CurrentX;
        YCoordinate = CurrentY;
        while (true)
        {
            XCoordinate++;
            YCoordinate--;
            if (XCoordinate >= 8 || YCoordinate < 0)
                break;

            if (Move(XCoordinate, YCoordinate, ref allPossibleMoves))
                break;
        }
        return allPossibleMoves;
    }
}