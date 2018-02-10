using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverDisplayer : MonoBehaviour
{
    [SerializeField]
    private BoardManager boardManager;

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private Text displayText;

    protected virtual void Awake ()
    {
        gameOverPanel.SetActive(false);

        boardManager.OnGameEnd += OnGameEndHandle;
    }

    private void OnGameEndHandle(bool isWhiteWin)
    {
        gameOverPanel.SetActive(true);
        displayText.text = isWhiteWin ? "White won!!" : "Black won!!";
    }
}
