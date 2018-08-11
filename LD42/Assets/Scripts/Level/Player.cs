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

    Vector3 yVector = new Vector3(0, 0.45f, 0);

    bool moving;

    private void Start()
    {
        currentCell = cellGrid.cells[Random.Range(0, cellGrid.cells.Length)];
        transform.position = currentCell.transform.position + yVector;
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
        if (Input.GetKey(KeyCode.W) && !moving)
        {
            Move(Direction.NORTH);
        }
        //Move left
        else if (Input.GetKey(KeyCode.A) && !moving)
        {
            if (spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;
            }

            Move(Direction.WEST);
        }
        //Move down
        else if (Input.GetKey(KeyCode.S) && !moving)
        {
            Move(Direction.SOUTH);
        }
        //Move right
        else if (Input.GetKey(KeyCode.D) && !moving)
        {
            if (!spriteRenderer.flipX)
            {
                spriteRenderer.flipX = true;
            }
            Move(Direction.EAST);
        }
    }

    void Move(Direction direction)
    {
        Cell targetCell = currentCell.GetNeighbor(direction);
        if (targetCell && !targetCell.HasBlock())
        {
            moving = true;
            currentCell.player = null;
            Vector3 startPos = transform.position;
            currentCell = targetCell;
            currentCell.player = this;
            Vector3 targetPosition = currentCell.transform.localPosition + yVector;
            StartCoroutine(AnimateMove(startPos, targetPosition));
        }
        else if (targetCell && targetCell.HasBlock())
        {
            if (canAttack)
            {
                moving = true;
                StartCoroutine(AttackBlock(direction));
                targetCell.block.TakeHit();
                if (targetCell.block == null)
                {
                    IncreaseScore();
                }
                moving = false;
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
        Vector3 startPos = currentCell.transform.position + yVector;
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
        for (; t < 0.01f; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos, startPos + delta, t);
            yield return null;
        }

        for (; t < 0.025f; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos + delta, startPos, t);
            yield return null;
        }

        transform.position = currentCell.transform.position + yVector;
        canAttack = false;
        timeSinceLastAttack = 0;
    }

    IEnumerator AnimateMove(Vector3 startPos, Vector3 targetPos)
    {
        float t = Time.deltaTime * 6.75f;
        for (; t < 1f; t += Time.deltaTime * 6.75f)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;

        moving = false;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
