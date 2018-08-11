using System.IO;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Level
{
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

        public static void ReturnToMenu()
        {
            SceneManager.LoadScene(0);
        }
    } //End Helpers
}