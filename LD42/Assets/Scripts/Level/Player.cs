using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public CellGrid cellGrid;

    public Cell currentCell;

    public SpriteRenderer spriteRenderer;
    public SpriteRenderer shadowRenderer;

    public List<Sprite> normalSprites;
    public List<Sprite> shadowSprites;

    bool canAttack = true;

    int score = 0;

    Vector3 yVector = new Vector3(0, 0.45f, 0);

    bool moving;

    public float moveSpeed = 6.65f;

    bool shadowed = false;

    private void Start()
    {
        currentCell = cellGrid.cells[Random.Range(0, cellGrid.cells.Length)];
        transform.position = currentCell.transform.position + yVector;
        spriteRenderer.flipX =  Random.Range(0, 2) == 0 ? true : false;
        shadowRenderer.flipX = spriteRenderer.flipX;

        spriteRenderer.sprite = normalSprites[0];
        shadowRenderer.sprite = shadowSprites[0];

    }

    // Update is called once per frame
    void Update () {
        HandleInput();
        CheckRenderShadow();
        if (moving)
        {
            SwapSprites(1);
        }
        else
        {
            SwapSprites(0);
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
                shadowRenderer.flipX = false;
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
                shadowRenderer.flipX = true;
            }
            Move(Direction.EAST);
        }
    }

    void CheckRenderShadow()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Block");

        bool renderShadow = false;
        for (var i = 0; i < gameObjects.Length; i++)
        {
            if (currentCell == gameObjects[i].GetComponent<Block>().GetCellBelow())
            {
                renderShadow = true;
                break;
            }
            else
            {
                renderShadow = false;
            }
        }
        if (renderShadow)
        {
            shadowRenderer.gameObject.SetActive(true);
            shadowed = true;
        }
        else
        {
            shadowRenderer.gameObject.SetActive(false);
            shadowed = false;
        }
    }

    void Move(Direction direction)
    {
        Cell targetCell = currentCell.GetNeighbor(direction);
        if (targetCell && !targetCell.HasBlock() && !targetCell.shattered)
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
                canAttack = false;
                moving = true;
                StartCoroutine(AttackBlock(direction));
                StartCoroutine(targetCell.block.TakeHit());
                moving = false;
            }
        }
    }

    public void IncreaseScore()
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
        int deltaMultiplier = 30;
        switch (direction)
        {
            case Direction.NORTH:
                delta = new Vector3(0, 0, 1 * deltaMultiplier);
                break;

            case Direction.WEST:
                delta = new Vector3(-1 * deltaMultiplier, 0, 0);
                break;

            case Direction.SOUTH:
                delta = new Vector3(0, 0, -1 * deltaMultiplier);
                break;

            case Direction.EAST:
                delta = new Vector3(1 * deltaMultiplier, 0, 0);
                break;

            default:
                delta = new Vector3(0, 0, 0);
                break;
        }
        float t = Time.deltaTime;
        SwapSprites(1);
        for (; t < 0.15f; t += Time.deltaTime)
        {
            if (t > 0.5f)
            {
                SwapSprites(1);
            }
            transform.position = Vector3.Lerp(startPos, startPos + delta, t);
            yield return null;
        }

        SwapSprites(0);
        for (; t < 0.075f; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(startPos + delta, startPos, t);
            yield return null;
        }

        transform.position = currentCell.transform.position + yVector;
        canAttack = true;
    }

    void SwapSprites(int spriteNum)
    {
        if (!shadowed)
        {
            spriteRenderer.sprite = normalSprites[spriteNum];
        }
        else
        {
            spriteRenderer.sprite = shadowSprites[spriteNum];
        }
    }

    IEnumerator AnimateMove(Vector3 startPos, Vector3 targetPos)
    {
        float t = Time.deltaTime * moveSpeed;
        for (; t < 1f; t += Time.deltaTime * moveSpeed)
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
