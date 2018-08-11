using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public CellGrid cellGrid;

    public Cell currentCell;

    public SpriteRenderer spriteRenderer;

    float timeSinceLastAttack;
    float attackCooldown = 0.10f;
    bool canAttack;

    int score = 0;

    private void Start()
    {
        currentCell = cellGrid.cells[Random.Range(0, cellGrid.cells.Length)];
        transform.position = currentCell.transform.position + new Vector3(0, 0.2f, 0);
    }

    // Update is called once per frame
    void Update () {
        HandleInput();
    }

    private void FixedUpdate()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack > attackCooldown && !canAttack)
        {
            //Action
            //timeSinceLastAttack = 0;
            canAttack = true;
        }
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
            if (spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;
            }
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
            if (!spriteRenderer.flipX)
            {
                spriteRenderer.flipX = true;
            }
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
            transform.position = currentCell.transform.localPosition + new Vector3(0, 0.2f, 0);
        }
        else if (targetCell && targetCell.HasBlock())
        {
            if (canAttack)
            {
                StartCoroutine(AttackBlock(direction));
                targetCell.block.TakeHit();
                if (targetCell.block == null)
                {
                    IncreaseScore();
                }
            }
        }
    }

    void IncreaseScore()
    {
        score++;
    }

    public int GetScore()
    {
        return score;
    }
    
    IEnumerator AttackBlock(Direction direction)
    {
        Vector3 startPos = transform.position;
        Vector3 delta;
        switch (direction)
        {
            case Direction.NORTH:
                delta = new Vector3(0, 0, 10);
                break;

            case Direction.WEST:
                delta = new Vector3(-10, 0, 0);
                break;

            case Direction.SOUTH:
                delta = new Vector3(0, 0, -10);
                break;

            case Direction.EAST:
                delta = new Vector3(10, 0, 0);
                break;

            default:
                delta = new Vector3(0, 0, 0);
                break;
        }
        float t = Time.deltaTime;
        for (; t < 0.10f; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos, startPos + delta, t);
            yield return null;
        }

        for (; t < 0.15f; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos + delta, startPos, t);
            yield return null;
        }

        transform.position = currentCell.transform.position + new Vector3(0, 0.2f, 0);
        canAttack = false;
        timeSinceLastAttack = 0;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
