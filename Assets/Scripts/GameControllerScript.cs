using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class GameControllerScript : MonoBehaviour
{
    public UnityEvent GameStarted;
    public GameState gameState = GameState.startIdle;

    public GameObject startView;
    public GameObject gameplayView;
    public GameObject gameOverView;
    public GameObject victoryView;

    private GameObject currentView;

    private void Awake() {
        currentView = startView;
        gameplayView.SetActive(false);
        gameOverView.SetActive(false);
        victoryView.SetActive(false);
    }

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
            currentView.SetActive(false);
            currentView = gameOverView;
            currentView.SetActive(true);
        }
    }

    public void Victory()
    {
        if(gameState == GameState.play)
        {
            gameState = GameState.victory;
            currentView.SetActive(false);
            currentView = victoryView;
            currentView.SetActive(true);
        }
    }

    private void InitializeGame()
    {
        gameState = GameState.play;
        currentView.SetActive(false);
        currentView = gameplayView;
        currentView.SetActive(true);
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
