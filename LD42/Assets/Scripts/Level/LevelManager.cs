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

    //Check for user input
    private void Update()
    {

    } //End Update()

    public void RestartGame()
    {

    }

    public void ReturnToMenu()
    {
        Helpers.ReturnToMenu();
    }

    //Reset UI and Level Stats
    void ResetUI()
    {
        //loadedLevelText.text = GameManager.instance.loadedLevelText;
        //totalMoves = 0;
        //totalGoals = cellGrid.goals.Count;
        //cellGrid.occupiedGoals.Clear();
        //occupiedGoals = 0;

        ////Scale minimumGoalsForWin based on totalGoals
        //if (totalGoals > 2)
        //{
        //    minimumGoalsForWin = Mathf.FloorToInt(totalGoals / 2.75f +  totalGoals / 3.3f);
        //}
        ////If totalGoals < 2, all goals must be filled to win
        //else
        //{
        //    minimumGoalsForWin = totalGoals;
        //}
    } //End LoadNewLevel()

    //Update the Level Stats and their UI elements
    void UpdateUI()
    {
        //occupiedGoals = cellGrid.occupiedGoals.Count;

        //goalsText.text = "Goals: " + occupiedGoals + "/" + totalGoals;
        //movesText.text = "Moves: " + totalMoves;

        ////If player has met the win condition
        //    //Set requiredGoalsText to green
        //if (occupiedGoals == totalGoals)
        //{
        //    goalsText.color = Color.green;
        //}
        ////If player has yet to meet the win condition
        //    //Set requiredGoalsText to red
        //else
        //{
        //    goalsText.color = Color.red;
        //} 
    } //End UpdateLevelStats()
} //End LevelManager class
