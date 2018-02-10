using System.Collections;
using UnityEngine;
using System;

public class KingMoveLogic : ChessPiece
{
    public override bool[,] AllPossibleMoves()
    {
        bool[,] allPossibleMoves = new bool[8, 8];

        Move(CurrentX + 1, CurrentY, ref allPossibleMoves); // up
        Move(CurrentX - 1, CurrentY, ref allPossibleMoves); // down
        Move(CurrentX, CurrentY - 1, ref allPossibleMoves); // left
        Move(CurrentX, CurrentY + 1, ref allPossibleMoves); // right
        Move(CurrentX + 1, CurrentY - 1, ref allPossibleMoves); // up left
        Move(CurrentX - 1, CurrentY - 1, ref allPossibleMoves); // down left
        Move(CurrentX + 1, CurrentY + 1, ref allPossibleMoves); // up right
        Move(CurrentX - 1, CurrentY + 1, ref allPossibleMoves); // down right

        return allPossibleMoves;
    }
}
