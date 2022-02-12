using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameControllerScript : MonoBehaviour
{
    public GameState gameState = GameState.startIdle;

    public UnityEvent GameStarted;

    public void StartGame()
    {
        if(gameState == GameState.startIdle)
        {
            InitializeGame();
        }
    }

    public void RestartGame()
    {
        if(gameState == GameState.gameOver || gameState == GameState.victory)
        {
            InitializeGame();
        }
    }

    public void GameOver()
    {
        if(gameState == GameState.play)
        {
            gameState = GameState.gameOver;
        }
    }

    public void Victory()
    {
        if(gameState == GameState.play)
        {
            gameState = GameState.victory;
        }
    }

    private void InitializeGame()
    {
        gameState = GameState.play;
        GameStarted.Invoke();
    }

    public enum GameState
    {
        startIdle,
        play,
        gameOver,
        victory
    }
}
