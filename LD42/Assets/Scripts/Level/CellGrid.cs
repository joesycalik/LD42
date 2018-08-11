﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Assets.Scripts.Level
{
    public class CellGrid : MonoBehaviour
    {

        //CellGrid dimensions
        public int cellCountX = 5;
        public int cellCountZ = 5;

        //Prefabs for the Cell and Level objects
        public Cell cellPrefab;
        public Text cellLabelPrefab;

        //Arrays and Lists for Cells and Level objects
        public Cell[] cells;
        public Color[] colors;

        //Canvas for Cell Labels
        Canvas gridCanvas;

        //Mesh for Cells
        CellMesh cellMesh;

        //Get components, instantiate lists, and create the Cell grid
        private void Awake()
        {
            gridCanvas = GetComponentInChildren<Canvas>();
            cellMesh = GetComponentInChildren<CellMesh>();

            CellMetrics.colors = colors;

            CreateLevel(cellCountX, cellCountZ);
        } //End Awake()

        //Re-Triangulate the Cell grid to refresh properties
        private void LateUpdate()
        {
            cellMesh.Triangulate(cells);
            enabled = false;
        } //End LateUpdate()

        //Replace the current Cell grid with a new one
        public bool CreateLevel(int x, int z)
        {
            //Check Cell count values
            if (
                x <= 0 || z <= 0
            )
            {
                Debug.LogError("Unsupported map size.");
                return false;
            }

            //Destroy all existing Cells
            if (cells != null)
            {
                for (int i = 0; i < cells.Length; i++)
                {
                    Destroy(cells[i].uiRect.gameObject);
                    Destroy(cells[i].gameObject);
                }
            }

            //Set new grid dimensions
            cellCountX = x;
            cellCountZ = z;

            //Create the new Cell grid
            CreateCells();
            Refresh();

            return true;
        } //End CreateLevel()

        //Create a grid of new Cells
        void CreateCells()
        {
            //Set the cells array 
            cells = new Cell[cellCountZ * cellCountX];

            for (int z = 0, i = 0; z < cellCountZ; z++)
            {
                for (int x = 0; x < cellCountX; x++)
                {
                    CreateCell(x, z, i++);
                }
            }
        } //End CreateCells()

        //Create a new Cell
        void CreateCell(int x, int z, int i)
        {
            //Set the Cell's position
            Vector3 position;
            position.x = x * (CellMetrics.outerSide * 2f);
            position.y = 0f;
            position.z = z * (CellMetrics.outerSide * 2f);

            //Create the Cell and attach it to the grid
            Cell cell = cells[i] = Instantiate<Cell>(cellPrefab);
            cell.transform.SetParent(transform, false);
            cell.transform.localPosition = position;
            cell.coordinates = new CellCoordinates(x, z);
            cell.cellGrid = this;

            //Set Cell neighbor connections
            if (x > 0)
            {
                cell.SetNeighbor(Direction.WEST, cells[i - 1]);
            }
            if (z > 0)
            {
                cell.SetNeighbor(Direction.SOUTH, cells[i - cellCountX]);
            }

            //Set up the Cell Label
            //Text label = Instantiate<Text>(cellLabelPrefab);
            //label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
            //cell.uiRect = label.rectTransform;
            //cell.uiRect.SetParent(gridCanvas.transform, false);
            //label.text = cell.coordinates.ToString();
        } //End CreateCell()

        //Enable the Cell grid for LateUpdate()
        public void Refresh()
        {
            enabled = true;
        } //End Refresh()

        //Get the Cell at user input location
        public Cell GetCell(Ray ray)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return GetCell(hit.point);
            }
            return null;
        } //End GetCell(ray)

        //Get the Cell translated from a Vector3 hit point
        public Cell GetCell(Vector3 position)
        {
            position = transform.InverseTransformPoint(position);
            CellCoordinates coordinates = CellCoordinates.ConvertToCoords(position);

            int index = coordinates.X + (coordinates.Z * cellCountX);
            return cells[index];
        } //End GetCell(position)
    } //End CellGrid class
}