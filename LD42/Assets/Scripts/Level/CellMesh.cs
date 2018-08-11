using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level
{
    /// <summary>
    /// CellGrid Mesh component: Responsible for the rendering of cells
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class CellMesh : MonoBehaviour
    {

        //The mesh object
        Mesh cellMesh;

        //These lists are populated with the vertex, triangle, and Cell color data for rendering the mesh
        List<Vector3> vertices;
        List<int> triangles;
        List<Color> colors;

        public Color innerColor;

        //CellMesh Collider
        MeshCollider meshCollider;

        //On startup, set up the MeshFilter and MeshCollider
        //Name the mesh and initialize the Lists
        private void Awake()
        {
            GetComponent<MeshFilter>().mesh = cellMesh = new Mesh();
            meshCollider = gameObject.AddComponent<MeshCollider>();

            cellMesh.name = "Cell Mesh";

            vertices = new List<Vector3>();
            triangles = new List<int>();
            colors = new List<Color>();
        } //End Awake()

        //Controls the Mesh triangulation and calls the individual Cell Triangulation method
        //When called, clear the Mesh and Lists
        //Triangulate each individual Cell and populate the Lists with the Mesh data
        //Recalculate the normals and set the meshCollider
        public void Triangulate(Cell[] cells)
        {
            cellMesh.Clear();
            vertices.Clear();
            triangles.Clear();
            colors.Clear();

            for (int i = 0; i < cells.Length; i++)
            {
                Triangulate(cells[i]);
            }
            cellMesh.vertices = vertices.ToArray();
            cellMesh.triangles = triangles.ToArray();
            cellMesh.colors = colors.ToArray();

            cellMesh.RecalculateNormals();
            meshCollider.sharedMesh = cellMesh;
        } //End Triangulate(Cell[])

        //Triangulate a single cell
        //Get the Cell's center position
        //Give the Cell a quad using the CellMetrics.corners
        //Give the quad a color
        void Triangulate(Cell cell)
        {
            Vector3 center = cell.transform.localPosition;
            AddQuad(
                center + CellMetrics.outerCorners[0],
                center + CellMetrics.outerCorners[1],
                center + CellMetrics.outerCorners[2],
                center + CellMetrics.outerCorners[3]);

            AddQuadColor(cell.Color);

            AddQuad(
                center + CellMetrics.innerCorners[0],
                center + CellMetrics.innerCorners[1],
                center + CellMetrics.innerCorners[2],
                center + CellMetrics.innerCorners[3]);

            AddQuadColor(innerColor);
            //AddQuadColor(new Color(70, 40, 40));
        } //End Triangulate(Cell)

        //Create a quad
        //Add 4 vertices
        //Use the 4 vertices to assign 2 triangles to the quad
        void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            vertices.Add(v4);
            triangles.Add(vertexIndex + 2);
            triangles.Add(vertexIndex + 3);
            triangles.Add(vertexIndex + 1);
            triangles.Add(vertexIndex + 3);
            triangles.Add(vertexIndex);
            triangles.Add(vertexIndex + 1);
        } //End AddQuad()

        //Add color to a quad
        void AddQuadColor(Color color)
        {
            colors.Add(color);
            colors.Add(color);
            colors.Add(color);
            colors.Add(color);
        } //End AddQuadColor()
    } //End CellMesh class
}