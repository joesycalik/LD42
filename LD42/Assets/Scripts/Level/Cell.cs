using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

namespace Assets.Scripts.Level
{
    public class Cell : MonoBehaviour
    {

        //The Cell's coordinates
        public CellCoordinates coordinates;

        //The CellGrid that the Cell is attached to
        public CellGrid cellGrid;

        //The Cell's 4 neighbors
        [SerializeField]
        Cell[] neighbors;

        //A reference to the Label used to display the Cell's coordinates
        public RectTransform uiRect;

        //The color index of the Cell references the CellMetrics Color array
        int colorIndex;
        public int ColorIndex
        {
            //ColorIndex getter
            get
            {
                return colorIndex;
            } //ColorIndex setter

            //ColorIndex getter
            set
            {
                if (colorIndex != value)
                {
                    colorIndex = value;
                    Refresh();
                }
            } //ColorIndex setter
        } //End ColorIndex

        //The Cell's color, based on its colorIndex
        public Color Color
        {
            //Color getter
            get
            {
                return CellMetrics.colors[colorIndex];
            } //End Color getter
        } //End Color

        /// <summary>
        /// Neighbor methods
        /// Getter and Setter
        /// Highlight - Set isHighlighted to true for all adjacent neighbors
        /// Unhighlight - Set isHighlighted to false for all adjacent neighbors
        /// </summary>

        //Get the Cell's neighbor in the specified direction
        public Cell GetNeighbor(Direction direction)
        {
            if (!neighbors[(int)direction])
            {
                return null;
            }
            return neighbors[(int)direction];
        } //End GetNeighbor()

        //Set the Cell's neighbor in the specified direction
        public void SetNeighbor(Direction direction, Cell cell)
        {
            neighbors[(int)direction] = cell;
            cell.neighbors[(int)direction.Opposite()] = this;
        } //End SetNeighbor()

        //Return the direction of a Cell relative to this Cell
        public int GetTargetCellDirection(Cell targetCell)
        {
            //Iterate through neighbor cells
            //If any cells match the target Cell, return the index matching the Direction
            for (int i = 0; i < 4; i++)
            {
                if (targetCell.Equals(GetNeighbor((Direction)i)))
                {
                    return i;
                }
            }
            return -1;
        } //End GetTargetCellDirection()

        //Refresh the CellGrid this Cell is attached to
        void Refresh()
        {
            if (cellGrid)
            {
                cellGrid.Refresh();
            }
        } //End Refresh()
    } //End Cell class
}