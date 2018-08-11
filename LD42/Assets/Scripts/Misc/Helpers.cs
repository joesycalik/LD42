using System.IO;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Helpers contains any universal methods
/// </summary>
public static class Helpers
{

    //Return the Cell under an input position
    public static Cell GetCellUnderCursor(CellGrid cellGrid)
    {
        return cellGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
    } //End GetCellUnderCursor()

    //Return the Cell under an input position
    public static Cell GetCellUnderPosition(CellGrid cellGrid, Vector3 position)
    {
        return cellGrid.GetCell(position);
    } //End GetCellUnderCursor()

    public static void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
} //End Helpers