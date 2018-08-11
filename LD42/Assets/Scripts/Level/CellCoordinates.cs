using UnityEngine;

namespace Assets.Scripts.Level
{
    /// <summary>
    /// The CellCoordinates struct stores a Cell's square grid coordinates in (x, z) format
    /// </summary>
    [System.Serializable]
    public struct CellCoordinates
    {

        //Coordinate values
        [SerializeField]
        private int x, z;

        //Access the X value
        public int X
        {
            //X getter
            get
            {
                return x;
            } //End X getter
        }

        //Access the Z value
        public int Z
        {
            //Z getter
            get
            {
                return z;
            } //End Z getter
        } //End Z

        //CellCoordinates Constructor
        //Set the CellCoordinates x and z values
        public CellCoordinates(int x, int z)
        {
            this.x = x;
            this.z = z;
        } //End CellCoordinates Constructor

        //Take a Vector3 input position and return a new CellCoordinates object
        //Vector3 -> (x,z) coordinates
        public static CellCoordinates ConvertToCoords(Vector3 position)
        {
            float x = position.x / (CellMetrics.outerSide * 2);
            float z = position.z / (CellMetrics.outerSide * 2);

            int iX = Mathf.RoundToInt(x);
            int iZ = Mathf.RoundToInt(z);

            return new CellCoordinates(iX, iZ);
        } //End ConvertToCoords()

        //ToString()
        //return (x, z) 
        public override string ToString()
        {
            return "(" + X.ToString() + ", " + Z.ToString() + ")";
        } //End ToString()
    } //End CellCoordinates struct
}