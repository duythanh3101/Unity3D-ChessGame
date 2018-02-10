using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovedMovesDisplayer : MonoBehaviour
{
    [SerializeField]
    private BoardManager boardManager;

    [SerializeField]
    private Text textDisplayPrefab;

    [SerializeField]
    private Transform layout;

    protected virtual void Awake ()
    {
        boardManager.OnAChessPieceMoved += OnAChessPieceMovedHandle;
    }

    private void OnAChessPieceMovedHandle(ChessMove chessMove)
    {
        Text newMoveDisplayText = Instantiate(textDisplayPrefab, layout);
        newMoveDisplayText.text = chessMove.ToString();
    }
}
