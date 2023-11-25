using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour
{
    public Cell[,] cells;
    public GameObject cellPrefab;
    public Material blockMaterial;
    public Material unblockMaterial;
    public GameObject aiPrefab;
    public GameObject rewardPrefab;

    public void InitGameField(int width, int height)
    {
        cells = new Cell[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject cell = Instantiate(cellPrefab);
                Vector3 cellPosition = GetCellPosition(i, j);
                cells[i, j] = new Cell()
                {
                    x = i,
                    y = j,
                    position = GetCellPosition(i, j),
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

    public Vector3 GetCellPosition(int x,int y)
    {
        return new Vector3(x,0, y);
    }

    public void GetCellCoordinate(Vector3 pos, out int x, out int y)
    {
        x = (int)(pos.x);
        y = (int)(pos.z);
    }

    public void InitAICharacter(int x,int y)
    {
        GameObject ai = Instantiate(aiPrefab);
        ai.transform.position = GetCellPosition(x, y);
    }

    public GameObject CreateReward(int x,int y)
    {
        GameObject reward = Instantiate(rewardPrefab);
        reward.transform.position = GetCellPosition(x, y);
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