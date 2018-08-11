using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public CellGrid cellGrid;

    public Cell currentCell;

    private void Start()
    {
        currentCell = cellGrid.cells[0];
    }

    // Update is called once per frame
    void Update () {
        HandleInput();
	}

    void HandleInput()
    {
        //Move up
        if (Input.GetKeyUp(KeyCode.W))
        {
            Move(Direction.NORTH);
        }
        //Move left
        else if (Input.GetKeyUp(KeyCode.A))
        {
            Move(Direction.WEST);
        }
        //Move down
        else if (Input.GetKeyUp(KeyCode.S))
        {
            Move(Direction.SOUTH);
        }
        //Move right
        else if (Input.GetKeyUp(KeyCode.D))
        {
            Move(Direction.EAST);
        }
    }

    void Move(Direction direction)
    {
        Cell targetCell = currentCell.GetNeighbor(direction);
        if (targetCell && !targetCell.HasBlock())
        {
            currentCell.player = null;
            currentCell = targetCell;
            currentCell.player = this;
            transform.position = currentCell.transform.position + new Vector3(0, 0.2f, 0);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
