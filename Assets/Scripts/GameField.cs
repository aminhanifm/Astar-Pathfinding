using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameField : MonoBehaviour
{
    public static GameField Instance {get; private set;}
    public Cell[,] cells;
    public GameObject cellPrefab;
    public Material blockMaterial;
    public Material unblockMaterial;
    public GameObject aiPrefab;
    public GameObject rewardPrefab;

    private void Awake() {
        Instance = this;
    }

    public void InitGameField(int width, int height)
    {
        cells = new Cell[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject cell = Instantiate(cellPrefab);
                Vector3 cellPosition = AstarPathFinding.GetCellPosition(i, j);
                cells[i, j] = new Cell()
                {
                    x = i,
                    y = j,
                    position = AstarPathFinding.GetCellPosition(i, j),
                    cellObject = cell
                };
                cell.transform.position = cellPosition;
                cell.transform.GetChild(0).GetComponent<Renderer>().material = unblockMaterial;
            }
        }
    }

    public void BlockCell(int x, int y)
    {
        cells[x, y].isBlocked = true;
        cells[x, y].cellObject.transform.GetChild(0).GetComponent<Renderer>().material = blockMaterial;
    }

    public void UnblockCell(int x, int y)
    {
        cells[x, y].isBlocked = false;
        cells[x, y].cellObject.transform.GetChild(0).GetComponent<Renderer>().material = unblockMaterial;
    }

    public bool IsCellBlocked(int x, int y)
    {
        return cells[x, y].isBlocked;
    }

    public void InitAICharacter(int x,int y)
    {
        GameObject ai = Instantiate(aiPrefab);
        ai.transform.position = AstarPathFinding.GetCellPosition(x, y);
    }

    public GameObject CreateReward(int x,int y)
    {
        GameObject reward = Instantiate(rewardPrefab);
        reward.transform.position = AstarPathFinding.GetCellPosition(x, y);
        return reward;
    }
}

public class Cell
{
    public int x;
    public int y;
    public Vector3 position;
    public GameObject cellObject;
    public bool isBlocked;
}