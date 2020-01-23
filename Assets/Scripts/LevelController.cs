using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    const int GRID_WIDTH = 20;
    const int GRID_HEIGHT = 10;
    const float START_X = -7.125f;
    const float START_Y = -4.0f;
    const float LENGTH = 0.75f;
    public GameObject blockPrefab;
    List<GameObject> allBlock;
    public BlockController[,] allGrid;
    private int score = 0;
    private int hightScore = 0;
    public Text scoreText;
    public Text hightScoreText;
    public GameObject gameOverObject;

    void Start()
    {
        allGrid = new BlockController[GRID_WIDTH, GRID_HEIGHT];
        InitBlock();
    }

    public void Reset()
    {
        RemoveAllBlock();
        InitBlock();
        score = 0;
        UpdateScoreText();
        gameOverObject.SetActive(false);
    }

    public void IncreaseScore(int addScore)
    {
        score += addScore;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score : " + score.ToString("0000");
    }

    void InitBlock()
    {
        allBlock = new List<GameObject>();
        for (int y = 0; y < GRID_HEIGHT; y++)
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                GameObject theBlock = Instantiate(blockPrefab, new Vector3(START_X + x * LENGTH, START_Y + y * LENGTH, -2), Quaternion.identity);
                allBlock.Add(theBlock);
                allGrid[x, y] = theBlock.GetComponent<BlockController>();
                allGrid[x, y].SetType((BlockController.BlockType)Random.Range(0, 5));
                allGrid[x, y].x = x;
                allGrid[x, y].y = y;
            }
    }

    void RemoveAllBlock()
    {
        for (int i = 0; i < allBlock.Count; i++)
        {
            Destroy(allBlock[i]);
        }
    }

    public void DestroyBlock(BlockController theBlock)
    {
        List<BlockController> sameBlock = new List<BlockController>();
        BlockController currentBlock;
        BlockController currentNode;
        List<BlockController> openList = new List<BlockController>();
        List<BlockController> closeList = new List<BlockController>();


        sameBlock.Add(theBlock);
        openList.Add(theBlock);


        while (openList.Count > 0)
        {
            currentNode = openList[0];
            if (closeList.Contains(currentNode))
            {
                openList.Remove(currentNode);
                continue;
            }

            if (currentNode.x + 1 < GRID_WIDTH && allGrid[currentNode.x + 1, currentNode.y] != null)
            {
                currentBlock = allGrid[currentNode.x + 1, currentNode.y];
                if (currentBlock.blockType == theBlock.blockType)
                {
                    if (!sameBlock.Contains(currentBlock))
                        sameBlock.Add(currentBlock);
                    openList.Add(currentBlock);
                }
            }

            if (currentNode.x - 1 >= 0 && allGrid[currentNode.x - 1, currentNode.y] != null)
            {
                currentBlock = allGrid[currentNode.x - 1, currentNode.y];
                if (currentBlock.blockType == theBlock.blockType)
                {
                    if (!sameBlock.Contains(currentBlock))
                        sameBlock.Add(currentBlock);
                    openList.Add(currentBlock);
                }
            }

            if (currentNode.y + 1 < GRID_HEIGHT && allGrid[currentNode.x, currentNode.y + 1] != null)
            {
                currentBlock = allGrid[currentNode.x, currentNode.y + 1];
                if (currentBlock.blockType == theBlock.blockType)
                {
                    if (!sameBlock.Contains(currentBlock))
                        sameBlock.Add(currentBlock);
                    openList.Add(currentBlock);
                }
            }

            if (currentNode.y - 1 >= 0 && allGrid[currentNode.x, currentNode.y - 1] != null)
            {
                currentBlock = allGrid[currentNode.x, currentNode.y - 1];
                if (currentBlock.blockType == theBlock.blockType)
                {
                    if (!sameBlock.Contains(currentBlock))
                        sameBlock.Add(currentBlock);
                    openList.Add(currentBlock);
                }
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);
        }

        if (sameBlock.Count > 1)
        {
            CalculateScore(sameBlock.Count);
            for (int i = 0; i < sameBlock.Count; i++)
            {
                allGrid[sameBlock[i].x, sameBlock[i].y] = null;
                allBlock.Remove(sameBlock[i].gameObject);
                Destroy(sameBlock[i].gameObject);
            }
        }

        MoveBlockDown();
        ShiftBlockLeft();
        CheckRemainingBlock();
    }

    void CalculateScore(int numberBlock)
    {
        Debug.Log(numberBlock);
        if (numberBlock <= 3)
            IncreaseScore(1);
        else
            IncreaseScore((numberBlock - 3) * (numberBlock - 3));
    }

    void MoveBlockDown()
    {
        BlockController currentBlock;
        for (int y = 1; y < GRID_HEIGHT; y++)
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                currentBlock = allGrid[x, y];
                if (currentBlock != null)
                    while (currentBlock.y - 1 >= 0 && allGrid[x, currentBlock.y - 1] == null)
                    {
                        allGrid[currentBlock.x, currentBlock.y - 1] = currentBlock;
                        allGrid[currentBlock.x, currentBlock.y] = null;
                        currentBlock.y -= 1;
                        currentBlock.transform.position = new Vector3(START_X + x * LENGTH, START_Y + currentBlock.y * LENGTH, -2);
                    }
            }
    }

    void ShiftBlockLeft()
    {
        int leftCount = 0;
        for (int x = 1; x < GRID_WIDTH; x++)
        {
            for (int y = 0; y < GRID_HEIGHT; y++)
                if (allGrid[x, y] != null && allGrid[x - 1, y] != null)
                    leftCount++;

            if (leftCount <= 0)
                for (int i = x; i < GRID_WIDTH; i++)
                    for (int y = 0; y < GRID_HEIGHT; y++)
                    {
                        if (allGrid[i, y] != null)
                        {
                            allGrid[i, y].transform.position = new Vector3(START_X + (i - 1) * LENGTH, START_Y + y * LENGTH, -2);
                            allGrid[i, y].x--;
                            allGrid[i - 1, y] = allGrid[i, y];
                            allGrid[i, y] = null;
                        }
                    }


            leftCount = 0;
        }
    }

    void CheckRemainingBlock()
    {
        int count = 0;
        for (int y = 0; y < GRID_HEIGHT; y++)
            for (int x = 0; x < GRID_WIDTH; x++)
            {
                if (allGrid[x, y] != null)
                {
                    if (x - 1 >= 0 && allGrid[x - 1, y] != null)
                        if (allGrid[x, y].blockType == allGrid[x - 1, y].blockType)
                            count++;
                    if (x + 1 < GRID_WIDTH && allGrid[x + 1, y] != null)
                        if (allGrid[x, y].blockType == allGrid[x + 1, y].blockType)
                            count++;
                    if (y - 1 >= 0 && allGrid[x, y - 1] != null)
                        if (allGrid[x, y].blockType == allGrid[x, y - 1].blockType)
                            count++;
                    if (y + 1 < GRID_HEIGHT && allGrid[x, y + 1] != null)
                        if (allGrid[x, y].blockType == allGrid[x, y + 1].blockType)
                            count++;
                }
            }

        if (count <= 0)
            GameOver();
    }

    void GameOver()
    {
        gameOverObject.SetActive(true);
        if (score > hightScore)
        {
            hightScore = score;
            UpdateHightScore();
        }
    }

    void UpdateHightScore()
    {
        hightScoreText.text = "HScore : " + hightScore.ToString("0000");
    }
}
