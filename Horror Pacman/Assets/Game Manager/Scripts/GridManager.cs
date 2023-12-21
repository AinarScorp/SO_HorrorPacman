using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * creates scripts called nodes (they don't have monobehavoir) to store inforamtion.
 * Creates the grid for the map, since we use a coordinate system for pretty much everything in the game.
 * Also manages the locating and opening of the door.
 */

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;

    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } }

    private void Awake()
    {
        CreateGrid();
    }
    //return a reqeusted Node
    public Node GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }
        return null;
    }
    //fills up the dictionary (grid) with newly created Nodes(script files)
    void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
            }
        }
    }
    //changes the boolean for a Node.
    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = false;
        }
    }

    public void ResetNodes()
    {
        foreach (KeyValuePair<Vector2Int, Node> item in grid)
        {
            item.Value.ConnectedTo = null;
            item.Value.isExplored = false;
            item.Value.isPath = false;
        }
    }
    //two following methods convert the vector3 coordinates into vector2int and vise versa, it is used because Grid is created in 2D rather than 3D
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x );
        coordinates.y = Mathf.RoundToInt(position.z );

        return coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();

        position.x = coordinates.x;
        position.z = coordinates.y;
        return position;
    }
    public void FindDoor(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isDoor = true;
        }
    }
    public void OpenDoor(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = true;
        }
    }
}
