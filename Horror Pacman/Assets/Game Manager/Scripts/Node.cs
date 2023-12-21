using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script that stores information only.
 */

[System.Serializable]
public class Node
{
    public Vector2Int coordiantes;
    public bool isWalkable;
    public bool isExplored;
    public bool isPath;
    public Node ConnectedTo;
    public bool isDoor;
    public Node(Vector2Int coordiantes, bool isWalkable)
    {
        this.coordiantes = coordiantes;
        this.isWalkable = isWalkable;
    }
}
