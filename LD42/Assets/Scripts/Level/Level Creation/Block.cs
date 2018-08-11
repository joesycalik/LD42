using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{
    public CellGrid cellGrid;

    Vector3 startPos;
    bool placed = false;
    public SpriteRenderer spriteRenderer;
    public Sprite movingSprite;
    public Sprite placedSprite;

    float movingBlockY = 0.5f;
    float placedBlockY = 0.40f;

    public float timeSinceLastMove;

    int hp = 3;

    Cell currentCell;

    bool moving;

    Animator anim;

    private void Start()
    {
        spriteRenderer.sprite = movingSprite;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!placed && startPos != null)
        {
            timeSinceLastMove += Time.deltaTime;
            if (timeSinceLastMove > cellGrid.GetBlockMoveCooldown())
            {
                //Action
                timeSinceLastMove = 0;
                if (!moving)
                {
                    Move();
                }
            }
        }
    }

    public void SetStartPosition()
    {
        startPos = transform.position;
    }

    private void Move()
    {
        moving = true;
        Vector3 targetPosition = new Vector3(0, 0, 0);
        Vector3 startPosition = transform.position;
        //Spawned on the bottom of the screen
        if (startPos.z < 0)
        {
            targetPosition = new Vector3(transform.localPosition.x, movingBlockY, transform.position.z + 20);
        }
        //Spawned on the left side of the screen
        else if (startPos.x < 0)
        {
            targetPosition = new Vector3(transform.localPosition.x + 20, movingBlockY, transform.position.z);
        }
        //Spawned on the top of the screen
        if (startPos.z > (cellGrid.cellCountZ * 18))
        {
            targetPosition = new Vector3(transform.localPosition.x, movingBlockY, transform.position.z - 20);
        }
        //Spawned on the right side of the screen
        else if (startPos.x > (cellGrid.cellCountX * 18))
        {
            targetPosition = new Vector3(transform.localPosition.x - 20, movingBlockY, transform.position.z);
        }

        StartCoroutine(AnimateMove(startPosition, targetPosition)); 
    }

    IEnumerator AnimateMove(Vector3 startPos, Vector3 targetPos)
    {
        float t = Time.deltaTime * 5f;
        for (; t < 1f; t += Time.deltaTime * 5f)
        {
            transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;

        if (transform.position.x < 0 || transform.position.x > (cellGrid.cellCountX * 18)
            || transform.position.z < 0 || transform.position.z > (cellGrid.cellCountX * 18))
        {
            Destroy(gameObject);
        }

        if (!BlockUnderneath() && GetCellBelow() != null)
        {
            bool placeBlock = UnityEngine.Random.Range(0, 10) > 8 ? true : false;
            if (placeBlock)
            {
                PlaceBlock();
            }
        }

        moving = false;
    }

    void PlaceBlock()
    {
        transform.position = new Vector3(transform.position.x, placedBlockY, transform.position.z);
        currentCell = GetCellBelow();
        currentCell.block = this;
        spriteRenderer.sprite = placedSprite;
        spriteRenderer.sortingOrder = 1;
        placed = true;
        if (currentCell.player != null)
        {
            currentCell.player.Die();
        }
    }

    bool BlockUnderneath()
    {
        if (GetCellBelow() && GetCellBelow().HasBlock())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    Cell GetCellBelow()
    {
        return Helpers.GetCellUnderPosition(cellGrid, transform.position);
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
