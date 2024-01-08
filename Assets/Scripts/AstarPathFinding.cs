using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

[BurstCompile]
public static class AstarPathFinding
{
    public class Node
    {
        public int x;
        public int y;
        public Node fromNode;
        public int h;
        public int f => h + g;
        public int g;

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    [BurstCompile]
    public static Queue<Vector3> GetPath(Vector3 fromPosition, Vector3 toPosition)
    {
        var resultList = new NativeList<Vector3>(Allocator.TempJob);

        var job = new PathfindingJob
        {
            FromPosition = fromPosition,
            ToPosition = toPosition,
            ResultList = resultList
        };

        job.Schedule().Complete();

        var positionQueue = new Queue<Vector3>(resultList.ToArray());

        resultList.Dispose();

        return positionQueue;
    }

    private static bool Contains(int x,int y,List<Node> nodes)
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

    private static bool TryGetNode(int x , int y, List<Node> nodes, out Node node)
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

    private static bool NodeMatch(Node a, Node b)
        => a.x == b.x && a.y == b.y;

    private static int CalculateHeuristicValue(Node from, Node to)
    {
        return (from.x - to.x) * (from.x - to.x) + (from.y - to.y) * (from.y - to.y);
    }

    private static Node FindLeastFCostNode(List<Node> nodes)
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

    private static List<Node> GetAllNeighbors(Node node, Cell[,] cells)
    {
        List<Node> nodes = new List<Node>();
        int x = node.x;
        int y = node.y;
        if (x > 0 && !cells[x - 1, y].isBlocked)
            nodes.Add(new Node(x - 1, y));
        if (x < cells.GetLength(0) - 1 && !cells[x + 1, y].isBlocked)
            nodes.Add(new Node(x + 1, y));
        if (y > 0 && !cells[x, y - 1].isBlocked)
            nodes.Add(new Node(x, y - 1));
        if (y < cells.GetLength(1) - 1 && !cells[x, y + 1].isBlocked)
            nodes.Add(new Node(x, y + 1));
        return nodes;
    }

    public static Vector3 GetCellPosition(int x,int y)
    {
        return new Vector3(x,0, y);
    }

    public static void GetCellCoordinate(Vector3 pos, out int x, out int y)
    {
        x = (int)(pos.x);
        y = (int)(pos.z);
    }    

    public struct PathfindingJob : IJob
    {
        public Vector3 FromPosition;
        public Vector3 ToPosition;
        public NativeList<Vector3> ResultList;

        public void Execute()
        {

            GetCellCoordinate(FromPosition, out int startX, out int startY);
            GetCellCoordinate(ToPosition, out int endX, out int endY);

            Node start = new Node(startX, startY);
            Node end = new Node(endX, endY);
            Node current;

            List<Node> openList = new List<Node>();
            List<Node> closeList = new List<Node>();

            openList.Add(start);

            Node traceNode = null;

            int tried = 100_000;

            while (openList.Count > 0)
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

                List<Node> neighbors = GetAllNeighbors(current, GameField.Instance.cells/* cells */);

                foreach (var neighbor in neighbors)
                {
                    if (Contains(neighbor.x, neighbor.y, closeList))
                        continue;

                    neighbor.fromNode = current;
                    neighbor.g = current.g + 1;
                    neighbor.h = CalculateHeuristicValue(neighbor, end);

                    if (TryGetNode(neighbor.x, neighbor.y, openList, out Node n))
                    {
                        if (neighbor.g >= n.g)
                            continue;
                    }
                    openList.Add(neighbor);
                }
            }

            if (tried < 0)
            {
                Debug.LogError("Error in finding!");
                return;
            }

            List<Node> rebuild = new List<Node>();

            while (traceNode != start)
            {
                rebuild.Add(traceNode);
                traceNode = traceNode.fromNode;
            }

            for (int i = rebuild.Count - 1; i >= 0; i--)
            {
                Node nodes = rebuild[i];
                ResultList.Add(GetCellPosition(nodes.x, nodes.y));
            }
        
            // Debug.Log("Job Completed");
        }

    }
}

