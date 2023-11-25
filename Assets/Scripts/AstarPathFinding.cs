using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarPathFinding : MonoBehaviour
{
    public class Node
    {
        public int x;
        public int y;
        public Node fromNode;
        public int h;
        public int f => h + g;
        public int g;

        public Node(Cell cell)
        {
            this.x = cell.x;
            this.y = cell.y;
        }
    }

    public Queue<Vector3> GetPath(Vector3 fromPosition, Vector3 toPosition)
    {
        GameField gameField = FindObjectOfType<GameField>();
        var cells = gameField.cells;

        gameField.GetCellCoordinate(fromPosition, out int startX, out int startY);
        gameField.GetCellCoordinate(toPosition, out int endX, out int endY);

        Node start = new Node(cells[startX,startY]);
        Node end = new Node(cells[endX, endY]);
        Node current;

        List<Node> openList = new List<Node>();
        List<Node> closeList = new List<Node>();

        openList.Add(start);

        Node traceNode = null;

        int tried = 100_000;

        while(openList.Count > 0)
        {
            tried--;
            if (tried < 0)
                break;
            current = FindLeastFCostNode(openList);
            openList.Remove(current);
            closeList.Add(current);

            if (NodeMatch(current, end))
            {
                traceNode = current;
                break;
            }                

            List<Node> neighbors = GetAllNeighbors(current, cells);

            foreach(var neighbor in neighbors)
            {
                if (Contains(neighbor.x, neighbor.y, closeList))
                    continue;

                neighbor.fromNode = current;
                neighbor.g = current.g + 1;
                neighbor.h = CalculateHeuristicValue(neighbor, end);

                if(TryGetNode(neighbor.x,neighbor.y,openList,out Node n))
                {
                    if (neighbor.g >= n.g)
                        continue;
                }
                openList.Add(neighbor);
            }
        }

        if(tried < 0)
        {
            Debug.LogError("Error in finding!");
            return new Queue<Vector3>();
        }

        List<Node> rebuild = new List<Node>();

        while (traceNode != start)
        {
            rebuild.Add(traceNode);
            traceNode = traceNode.fromNode;
        }

        Queue<Vector3> positionQueue = new Queue<Vector3>();

        for (int i = rebuild.Count - 1; i >= 0; i--)
        {
            Node nodes = rebuild[i];
            positionQueue.Enqueue(gameField.GetCellPosition(nodes.x,nodes.y));
        }

        return positionQueue;
    }

    private bool Contains(int x,int y,List<Node> nodes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            var n = nodes[i];
            if (n.x == x && n.y == y)
            {
                return true;
            }
        }
        return false;
    }

    private bool TryGetNode(int x , int y, List<Node> nodes, out Node node)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            var n = nodes[i];
            if(n.x == x && n.y == y)
            {
                node = n;
                return true;
            }
        }
        node = null;
        return false;
    }

    private bool NodeMatch(Node a, Node b)
        => a.x == b.x && a.y == b.y;

    private int CalculateHeuristicValue(Node from, Node to)
    {
        return (from.x - to.x) * (from.x - to.x) + (from.y - to.y) * (from.y - to.y);
    }

    private Node FindLeastFCostNode(List<Node> nodes)
    {
        int lowest = int.MaxValue;
        Node lowestNode = null;
        foreach (var n in nodes)
        {
            if(n.f < lowest)
            {
                lowestNode = n;
                lowest = n.f;
            }
        }
        return lowestNode;
    }

    private List<Node> GetAllNeighbors(Node node, Cell[,] cells)
    {
        List<Node> nodes = new List<Node>();
        int x = node.x;
        int y = node.y;
        if (x > 0 && !cells[x - 1, y].isBlocked)
            nodes.Add(new Node(cells[x - 1, y]));
        if (x < cells.GetLength(0) - 1 && !cells[x + 1, y].isBlocked)
            nodes.Add(new Node(cells[x + 1, y]));
        if (y > 0 && !cells[x, y - 1].isBlocked)
            nodes.Add(new Node(cells[x, y - 1]));
        if (y < cells.GetLength(1) - 1 && !cells[x, y + 1].isBlocked)
            nodes.Add(new Node(cells[x, y + 1]));
        return nodes;
    }    
}

