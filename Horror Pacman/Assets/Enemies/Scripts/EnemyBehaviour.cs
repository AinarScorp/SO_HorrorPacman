using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Every behavior script inherits from this script
 */

public class EnemyBehaviour : MonoBehaviour
{
    public Enemy enemy { get; private set; }

    public GridManager gridManager { get; private set; }
    public PathFinder pathFinder { get; private set; }
    public GameManagerScript gameManager { get; private set; }


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManagerScript>();
        enemy = GetComponent<Enemy>();
        pathFinder = FindObjectOfType<PathFinder>();
        gridManager = FindObjectOfType<GridManager>();

    }

}
