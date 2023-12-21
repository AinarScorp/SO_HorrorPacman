using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is where every tile checks whether they are obstructed or not.
 * 
 */

public class Tile : MonoBehaviour
{
    [SerializeField]bool blocked = false;
    [SerializeField] LayerMask obstacle;

    Vector2Int coordinates = new Vector2Int();
    public Vector2Int Coordinates { get { return coordinates; } }

    bool isDoorTile = false;
    public bool IsDoorTile { get { return isDoorTile; } }

    GridManager gridManager;

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    void Start()
    {
        coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        if (blocked == false && Physics.Raycast(transform.position, Vector3.up, out RaycastHit hit, 10f, obstacle))
        {
            if (hit.collider.gameObject.GetComponent<Door>() != null) 
            {
                gridManager.FindDoor(coordinates);
                isDoorTile = true;
            }
            gridManager.BlockNode(coordinates);

            blocked = true;
        }
    }
}
