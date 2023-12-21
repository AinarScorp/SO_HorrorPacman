using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * Displays coordinates on the tiles.
 * This is mainly used for developing.
 * The coordinates are turned off in the base game.
 */

[RequireComponent(typeof(TextMeshPro))]
[ExecuteAlways]
public class Coordinates : MonoBehaviour
{
    [SerializeField] Color defaultColour = Color.white;
    [SerializeField] Color blockedColour = Color.red;
    [SerializeField] Color exploredColour = Color.yellow;
    [SerializeField] Color pathColour = Color.blue;

    TextMeshPro label;
    GridManager gridManager;
    Vector2Int coordinates = new Vector2Int();

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        DisplayCoordinates();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            label.enabled = !label.enabled;
        }
        DisplayCoordinates();
        SetLabelColour();
    }

    void SetLabelColour()
    {
        if (gridManager == null)
        {
            return;
        }

        Node node = gridManager.GetNode(coordinates);

        if (node == null)
        {
            return;
        }

        if (!node.isWalkable)
        {
            label.color = blockedColour;
        }
        else if (node.isPath)
        {
            label.color = pathColour;
        }
        else if (node.isExplored)
        {
            label.color = exploredColour;
        }
        else
        {
            label.color = defaultColour;
        }
    }

    void DisplayCoordinates()
    {
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x );
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z );

        label.text = coordinates.x + "," + coordinates.y;
    }
}
