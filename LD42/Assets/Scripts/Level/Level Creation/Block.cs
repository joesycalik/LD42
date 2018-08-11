using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Block : MonoBehaviour
{
    public CellGrid cellGrid;

    Vector3 startPos;
    bool placed = false;
    public SpriteRenderer spriteRenderer;
    public Sprite movingSprite;
    public Sprite placedSprite;

    public float timeSinceLastMove;

    int hp = 3;

    Cell currentCell;

    private void Start()
    {
        spriteRenderer.sprite = movingSprite;
    }

    private void Update()
    {
        if (!placed && startPos != null)
        {
            timeSinceLastMove += Time.deltaTime;
            if (timeSinceLastMove > cellGrid.blockMoveCooldown)
            {
                //Action
                timeSinceLastMove = 0;
                Move();
            }
        }
    }

    public void SetStartPosition()
    {
        startPos = transform.position;
    }

    void Move()
    {
        //Spawned on the bottom of the screen
        if (startPos.z < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 20);
        }
        //Spawned on the left side of the screen
        else if (startPos.x < 0)
        {
            transform.position = new Vector3(transform.position.x + 20, transform.position.y, transform.position.z);
        }
        //Spawned on the top of the screen
        if (startPos.z > (cellGrid.cellCountZ * 18))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 20);
        }
        //Spawned on the right side of the screen
        else if (startPos.x > (cellGrid.cellCountX * 18))
        {
            transform.position = new Vector3(transform.position.x - 20, transform.position.y, transform.position.z);
        }

        if (transform.position.x < 0 || transform.position.x > (cellGrid.cellCountX * 18)
            || transform.position.z < 0 || transform.position.z > (cellGrid.cellCountX * 18))
        {
            Destroy(gameObject);
            return;
        }

        if (!BlockUnderneath())
        {
            bool placeBlock = UnityEngine.Random.Range(0, 10) > 8 ? true : false;
            if (placeBlock)
            {
                PlaceBlock();
            }
        }
    }

    void PlaceBlock()
    {
        currentCell = Helpers.GetCellUnderPosition(cellGrid, transform.position);
        currentCell.block = this;
        spriteRenderer.sprite = placedSprite;
        placed = true;
        if (currentCell.player != null)
        {
            currentCell.player.Die();
        }
    }

    bool BlockUnderneath()
    {
        if (Helpers.GetCellUnderPosition(cellGrid, transform.position).HasBlock())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeHit()
    {
        hp -= 1;
        if (hp <= 0)
        {
            currentCell.block = null;
            Destroy(gameObject);
        }
    }
}
