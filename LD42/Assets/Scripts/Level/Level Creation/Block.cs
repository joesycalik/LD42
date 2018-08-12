using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public CellGrid cellGrid;

    Vector3 startPos;
    bool placed = false;
    public SpriteRenderer spriteRenderer;
    public Sprite movingSprite;
    public Sprite warningSprite;
    public Sprite flashSprite;
    public Sprite placedSprite;
    public List<Sprite> damagedSprites;

    float movingBlockY = 0.5f;
    float placedBlockY = 0.40f;

    public float timeSinceLastMove;

    int maxHP = 3;
    int hp;

    Cell currentCell;

    bool moving;

    Animator anim;

    float moveSpeed = 5f;

    int damagedSpriteID = -1;

    bool placing;

    private void Start()
    {
        spriteRenderer.sprite = movingSprite;
        anim = GetComponent<Animator>();
        hp = maxHP;
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
        moveSpeed = cellGrid.blockMoveSpeed;
    }

    public void SetStartPosition()
    {
        startPos = transform.position;
    }

    private void Move()
    {
        if (!placing)
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

        if (transform.position.x < 0 || transform.position.x > (cellGrid.cellCountX * 18)
            || transform.position.z < 0 || transform.position.z > (cellGrid.cellCountX * 18))
        {
            Destroy(gameObject);
        }

        if (!BlockUnderneath() && GetCellBelow())
        {
            bool placeBlock = UnityEngine.Random.Range(0, 10) > 8 ? true : false;
            if (placeBlock && !GetCellBelow().shattered)
            {
                StartCoroutine(PlaceBlock());
            }
        }

        moving = false;
    }

    float redFlashTime = 0.25f;
    float blueFlashTime = 0.15f;

    IEnumerator PlaceBlock()
    {
        placing = true;
        transform.position = new Vector3(transform.position.x, placedBlockY, transform.position.z);

        //GameSoundManager.instance.PlayWarningSound();

        for (int i = 0; i < 3; i++)
        {
            GameSoundManager.instance.PlayRedBlockSound();
            spriteRenderer.sprite = warningSprite;
            spriteRenderer.sortingOrder = 4;
            yield return new WaitForSeconds(redFlashTime);
            //GameSoundManager.instance.PlayBlueBlockSound();
            spriteRenderer.sprite = movingSprite;
            spriteRenderer.sortingOrder = 3;
            yield return new WaitForSeconds(redFlashTime);
        }

        GameSoundManager.instance.PlayBlockPlaceSound();

        currentCell = GetCellBelow();
        currentCell.block = this;        

        spriteRenderer.sprite = placedSprite;
        spriteRenderer.sortingOrder = 1;
        placed = true;
        if (currentCell.player != null)
        {
            currentCell.player.Die();
        }
        moving = false;
        placing = false;
    }

    int lastDifficulty;
    public void AdjustPlaceFlashTimers(int difficulty)
    {
        if (difficulty != lastDifficulty)
        {
            redFlashTime -= (difficulty * 0.01f);
            if (redFlashTime < 0.10f)
            {
                redFlashTime = 0.10f;
            }
            blueFlashTime -= (difficulty * 0.01f);
            if (blueFlashTime < 0.025f)
            {
                blueFlashTime = 0.025f;
            }
            lastDifficulty = difficulty;
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

    public Cell GetCellBelow()
    {
        return Helpers.GetCellUnderPosition(cellGrid, transform.position);
    }

    public IEnumerator TakeHit()
    {
        hp -= 1;
        damagedSpriteID++;
        if (damagedSpriteID > 1)
        {
            damagedSpriteID = 1;
        }

        spriteRenderer.sprite = flashSprite;
        yield return new WaitForSeconds(0.0005f);
        spriteRenderer.sprite = damagedSprites[damagedSpriteID];
        
        if (hp <= 0)
        {
            GameSoundManager.instance.PlayBlockBreakSound();
            Vector3 position = transform.position;
            Destroy(gameObject);
            currentCell.block = null;
            currentCell.shattered = true;
            cellGrid.GetPlayer().IncreaseScore();

            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Block");
            for (var i = 0; i < gameObjects.Length; i++)
            {
                if (gameObjects[i].transform.position == position)
                {
                    Destroy(gameObjects[i]);
                }
            }
        }
    }
}
