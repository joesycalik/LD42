using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Level
{
    /// <summary>
    /// CellCamera will zoom in and out based on map size and rotate to the views of Robots
    /// 
    /// 5x5
    /// Y position is 85
    /// North - X:40 Z:-11 | South - X:45 Z:100
    /// 7x7
    /// Y position is 120
    /// North - X:55 Z:-20 | South - X:65 Z:140
    /// 9x9
    /// Y position is 140
    /// North - X:75 Z:-20 | South - X:85 Z:180
    /// 
    /// CALCULATIONS:
    /// Zoom
    /// If CellCountX == CellCountZ
    ///     Y position = 100 + ((newX - oldX) * 10) (Should scale up or down)
    ///     NorthX = 40 + ((newX - oldX) * 10)
    ///     NorthZ = -30
    ///     SouthX = 45 + ((newX - oldX) * 10)
    ///     SouthZ = 100 + ((newX - oldX) * 20)
    /// 
    /// Rotate
    /// Start at North - X:NorthX Z:NorthZ (Y rotation = 0)
    /// South - X:SouthX Z:SouthZ (Y rotation = 180)
    /// East - X:NorthZ Z:SouthX (Y rotation = 90)
    /// West - X:SouthZ Z:NorthX (Y rotation) = -90
    /// </summary>
    public class CellCamera : MonoBehaviour
    {
        public Vector3 defaultPos;

        //private variables
        int NorthX;
        int NorthZ = -11;
        int SouthX;
        int SouthZ;
        int yPosition;
        int currentCellCountX = 5;

        public void ResetCamera(int cellCountX, int cellCountZ)
        {
            AdjustScale(cellCountX, cellCountZ);
        }

        public void AdjustScale(int cellCountX, int cellCountZ)
        {
            if (cellCountX == cellCountZ)
            {
                yPosition = ((cellCountX - currentCellCountX) * 20);
                NorthX = 40 + ((cellCountX - currentCellCountX) * 10);
                //SouthX = 40 + ((cellCountX - currentCellCountX) * 20);
                //SouthZ = 91 + ((cellCountX - currentCellCountX) * 20);

                transform.position = new Vector3(NorthX, yPosition, NorthX);
            }
        }
    }
}