﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.InputSystem;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    NodeGrid grid;
    private void Awake() {
        grid = GetComponent<NodeGrid>();
    }

    private void Update() {
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            FindPath(seeker.position, target.position);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0){
            Node currentNode = openSet.RemoveFirst();
            //Node currentNode = openSet[0];
            /*
            for (int i =1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            */
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                sw.Stop();
                print("Path found: " + sw.ElapsedTicks + " ticks");
                RetracePath(startNode,targetNode);
                return;
            }

            foreach (Node neighbors in grid.GetNeighbours(currentNode))
            {
                if (!neighbors.walkable || closedSet.Contains(neighbors))
                {
                    continue;
                }
                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode,neighbors);
                if (newMovementCostToNeighbour < neighbors.gCost || !openSet.Contains(neighbors)){
                    neighbors.gCost = newMovementCostToNeighbour;
                    neighbors.hCost = GetDistance(neighbors,targetNode);
                    neighbors.parent = currentNode;

                    if (!openSet.Contains(neighbors))
                        openSet.Add(neighbors);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX < distY)
            return 14*distY + 10 * (distX-distY);
        return 13*distX + 10 * (distY-distX);
    }
}
