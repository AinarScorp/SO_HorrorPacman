using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script is responsible for scanning the map and finding the best possible way to get to different point
 * It checks every adjacent tile and makes paths.
 */

public class PathFinder : MonoBehaviour
{
    Vector2Int startCoordinates;
    public Vector2Int StartCoordinates { get { return startCoordinates; } }

    Vector2Int destinationCoordinates;
    public Vector2Int DestinationCoordinates { get { return destinationCoordinates; } }

    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

    //node settings
    Node currentSearchNode;
    Node destinationNode;
    Node playerLocationNode;
    public Node PlayerLocationNode { get { return playerLocationNode; } }


    Queue<Node> queuedTiles = new Queue<Node>();

    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    GridManager gridManager;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null){grid = gridManager.Grid; }
    }


    public List<Node> GetNewPath(Vector2Int coordinates, Vector2Int destination)
    {
        startCoordinates = coordinates;
        destinationCoordinates = destination;

        this.destinationNode = grid[destination];
        gridManager.ResetNodes();
        BreathFirstSearch(coordinates);
        return BuildPath();
    }

   
    //the way this algoritm works it needs to find a certain location(coordinates) on the grid and it starts from it's current positiion, 
    //then it looks at all avaialable neigbours on the grid and adds them to the queue,
    ////then for every tile it looks for adjecent tiles that were not looked up previously until it finds the desired location
    void BreathFirstSearch(Vector2Int coordinates)
    {
        queuedTiles.Clear();
        reached.Clear();

        bool isRunning = true;
        queuedTiles.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);

        while (queuedTiles.Count > 0 && isRunning)
        {
            currentSearchNode = queuedTiles.Dequeue();

            currentSearchNode.isExplored = true;
            ExploreNeighbours();
            if (currentSearchNode.coordiantes == destinationNode.coordiantes)
            {
                isRunning = false;
            }
        }
    }
    void ExploreNeighbours()
    {
        List<Node> neighbours = new List<Node>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbourCoords = currentSearchNode.coordiantes + direction;
            if (grid.ContainsKey(neighbourCoords))
            {
                neighbours.Add(grid[neighbourCoords]);
            }

        }
        foreach (Node neighbour in neighbours)
        {
            if (!reached.ContainsKey(neighbour.coordiantes) && neighbour.isWalkable)
            {
                neighbour.ConnectedTo = currentSearchNode;
                reached.Add(neighbour.coordiantes, neighbour);
                queuedTiles.Enqueue(neighbour);
            }
        }
    }
    List<Node> BuildPath()
    {
        List<Node> path = new List<Node>();

        Node currentNode = destinationNode;

        path.Add(currentNode);
        currentNode.isPath = true;

        while (currentNode.ConnectedTo != null)
        {
            currentNode = currentNode.ConnectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;

        }

        path.Reverse();
        return path;
    }

    public void UpdatePlayerLocation(Node playerLocation)
    {
        this.playerLocationNode = playerLocation;
    }
}
