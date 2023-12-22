using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Every behavior script inherits from this script
 */

public class EnemyBehaviour : MonoBehaviour
{
    protected Enemy enemy { get; private set; }

    protected GridManager gridManager { get; private set; }
    protected PathFinder pathFinder { get; private set; }
    protected GameManagerScript gameManager { get; private set; }


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManagerScript>();
        enemy = GetComponent<Enemy>();
        pathFinder = FindObjectOfType<PathFinder>();
        gridManager = FindObjectOfType<GridManager>();

    }

}
