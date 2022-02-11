using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public GameState gameState = GameState.startIdle;
    public Player player;
    public GameObject enemyContainer; // контейнер просто для чтобы не засорять иерархию
    public GameObject enemyPrefab;
    public List<Enemy> enemies = new List<Enemy>();
    private Grid grid;

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
        player.CanBeControlled = true;
    }

    public enum GameState
    {
        startIdle,
        play,
        gameOver,
        victory
    }
}
