using Extension.ExtraTypes;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChessMove
{
    public ChessPiece MovedPiece { get; private set; }
    public IntVector2 BaseCoordinate { get; private set; }
    public IntVector2 DesCoordinate { get; private set; }

    public ChessPiece EatenPiece { get; private set; }

    public ChessPiece OldPawnPiece { get; private set; }

    public ChessMove(ChessPiece movedPiece, IntVector2 baseCoordinate, IntVector2 desCoordinate,
        ChessPiece eatenPiece = null, ChessPiece oldPawnPiece = null)
    {
        MovedPiece = movedPiece;
        BaseCoordinate = baseCoordinate;
        DesCoordinate = desCoordinate;

        EatenPiece = eatenPiece;

        OldPawnPiece = oldPawnPiece;
    }

    public override string ToString()
    {
        return String.Format("{0} -> {1} ]", BaseCoordinate, DesCoordinate);
    }
}
