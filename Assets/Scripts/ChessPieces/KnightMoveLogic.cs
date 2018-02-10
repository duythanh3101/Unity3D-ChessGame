using System.Collections;
using UnityEngine;
using System;

public class KnightMoveLogic : ChessPiece
{
    public override bool[,] AllPossibleMoves()
    {
        bool[,] allPossibleMoves = new bool[8, 8];

        // Up left
        Move(CurrentX - 1, CurrentY + 2, ref allPossibleMoves);

        // Up right
        Move(CurrentX + 1, CurrentY + 2, ref allPossibleMoves);

        // Down left
        Move(CurrentX - 1, CurrentY - 2, ref allPossibleMoves);

        // Down right
        Move(CurrentX + 1, CurrentY - 2, ref allPossibleMoves);


        // Left Down
        Move(CurrentX - 2, CurrentY - 1, ref allPossibleMoves);

        // Right Down
        Move(CurrentX + 2, CurrentY - 1, ref allPossibleMoves);

        // Left Up
        Move(CurrentX - 2, CurrentY + 1, ref allPossibleMoves);

        // Right Up
        Move(CurrentX + 2, CurrentY + 1, ref allPossibleMoves);

        return allPossibleMoves;
    }

}
