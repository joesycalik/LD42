using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The LevelManager is responsible for the core game logic
/// Control the flow of the game
/// Check for win conditions
/// </summary>
public class LevelManager : MonoBehaviour
{
    //Needs a reference to the CellGrid
    public CellGrid cellGrid;
    public Text timeElapsedText, levelText, scoreText;
    public GameOverMenu gameOverMenu;

    int level;
    float timeElapsed;
    float gameStartTime;

    private void Start()
    {
        gameStartTime = Time.time;
    }

    //Check for user input
    private void Update()
    {
        timeElapsed = Time.time - gameStartTime;
        cellGrid.difficulty = (int)(timeElapsed / 5) + 1;

        UpdateUI();

        CheckForGameEnd();
    } //End Update()

    public void CheckForGameEnd()
    {
        if (!cellGrid.GetPlayer())
        {
            gameOverMenu.Open();
        }
    }

    public void RestartGame()
    {
        cellGrid.NewGame();
        gameStartTime = Time.time;
    }

    public void ReturnToMenu()
    {
        Helpers.ReturnToMenu();
    }

    //Update the Level Stats and their UI elements
    void UpdateUI()
    {
        timeElapsedText.text = "Time Elapsed: " + timeElapsed.ToString("00");
        scoreText.text = "Score: " + cellGrid.GetPlayer().GetScore();
    } //End UpdateUI()

    public void DestroyAllBlocks()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Block");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }
} //End LevelManager class
