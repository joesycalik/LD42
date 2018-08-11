using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CellGrid : MonoBehaviour
{

    //CellGrid dimensions
    public int cellCountX = 5;
    public int cellCountZ = 5;

    //Prefabs for the Cell and Level objects
    public Cell cellPrefab;
    public Block blockPrefab;

    public Player playerPrefab;

    //Arrays and Lists for Cells and Level objects
    public Cell[] cells;
    public Color[] colors;
    //Mesh for Cells
    CellMesh cellMesh;

    //Cell Camera
    public CellCamera cellCamera;

    float timeSinceLastSpawn = 0;
    float spawnTimer = 5f;
    int spawnRate = 5;

    float baseBlockMoveCooldown = 1f;
    float blockMoveCooldown = 2f;

    Player player;

    public float GetBlockMoveCooldown()
    {
        return blockMoveCooldown;
    }

    public int difficulty;

    public int[] spawnLocations;

    bool triangulate = true;

    public struct SpawnLocation
    {
        public int x;
        public int z;
    }

    List<SpawnLocation> spawnsThisRound;

    //Get components, instantiate lists, and create the Cell grid
    private void Awake()
    {
        cellMesh = GetComponentInChildren<CellMesh>();
        NewGame();
    } //End Awake()

    public void NewGame()
    {
        spawnsThisRound = new List<SpawnLocation>();
        player = Instantiate(playerPrefab);
        player.cellGrid = this;

        CellMetrics.colors = colors;

        CreateLevel(cellCountX, cellCountZ);
    }

    private void Update()
    {

        spawnRate = (int) difficulty / 3;
        if (spawnRate > 10)
        {
            spawnRate = 10;
        }
        else if (spawnRate < 1)
        {
            spawnRate = 1;
        }

        spawnTimer = 2 - (difficulty * 0.1f);
        if (spawnTimer < 0.2f)
        {
            spawnTimer = 0.2f;
        }

        blockMoveCooldown = 2 - ((difficulty / 3) * 0.1f);
        if (blockMoveCooldown < 0.1f)
        {
            blockMoveCooldown = 0.1f;
        }

        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn > spawnTimer)
        {
            //Action
            timeSinceLastSpawn = 0;
            for (int i = 0; i < spawnRate; i++)
            {
                SpawnBlock();
            }
            spawnsThisRound.Clear();
        }
    }

    void SpawnBlock()
    {
        SpawnLocation spawn = new SpawnLocation();
        spawn.x = Random.Range(-1, cellCountX + 1);
        spawn.z = Random.Range(-1, cellCountX + 1);

        if (BadValueCheck(spawn))
        {
            SpawnBlock();
            return;
        }

        if (spawn.x != -1 && spawn.z != -1 && 
            spawn.x != cellCountX && spawn.z != cellCountX)
        {
            SpawnBlock();
            return;
        }

        foreach (SpawnLocation spawnLocation in spawnsThisRound)
        {
            if (spawnLocation.x == spawn.x && spawnLocation.z == spawn.z)
            {
                SpawnBlock();
                return;
            }
        }

        Block newBlock = Instantiate<Block>(blockPrefab);        

        newBlock.transform.position = newBlock.transform.position + new Vector3(spawn.x * 20, 0, spawn.z * 20);
        spawnsThisRound.Add(spawn);
        newBlock.SetStartPosition();
        newBlock.cellGrid = this;
    }

    bool BadValueCheck(SpawnLocation spawn)
    {
        if ((spawn.x == -1 && spawn.z == -1) ||
            (spawn.x == -1 && spawn.z == cellCountX) ||
            (spawn.x == -1 && spawn.z == cellCountX) ||
            (spawn.x == cellCountX && spawn.z == cellCountX))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Re-Triangulate the Cell grid to refresh properties
    private void LateUpdate()
    {
        if (triangulate)
        {
            cellMesh.Triangulate(cells);
            triangulate = false;
        }
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
                Destroy(cells[i].gameObject);
            }
        }

        //Set new grid dimensions
        cellCountX = x;
        cellCountZ = z;

        //Create the new Cell grid
        CreateCells();
        Refresh();

        //Refresh the camera
        cellCamera.ResetCamera(cellCountX, cellCountZ);

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
    } //End CreateCell()

    //Enable the Cell grid for LateUpdate()
    public void Refresh()
    {
        triangulate = true;
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

    public Player GetPlayer()
    {
        return player;
    }
} //End CellGrid class