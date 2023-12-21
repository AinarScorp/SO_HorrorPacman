using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * this script scans measures the distance between itself and the player and it turns off this script when that distance is less than huntRange
 * In addition to that, this script stores informtaion about certain position on the map and randomly goes there when the player loses a life
 * 
 */

public class EnemyGlowerHunt : EnemyBehaviour
{
    [SerializeField] float huntRange = 3;
    [SerializeField] [Range(0.1f, 20f)] float speed = 2f;
    [SerializeField] Vector2Int[] hidingLocationCoordinates;

    List<Node> path = new List<Node>();
    List<Node> hidingNodes = new List<Node>();

    BoxCollider playerCollider;

    private void Start()
    {
        SetUpHidingSpots();
        playerCollider = enemy.Player.gameObject.GetComponent<BoxCollider>();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        if (PlayerWithinRange())
        {
            enemy.StartChasing();
        }
    }
    private void SetUpHidingSpots()
    {
        hidingNodes.Add(SetRespawnPoint());
        foreach (Vector2Int destination in hidingLocationCoordinates)
        {
            if (gridManager.Grid[destination] != null)
            {
                hidingNodes.Add(gridManager.Grid[destination]);
            }
        }
    }
    // in order to avoid errors of not having any locations to return after player's death it automatically stores the position where it was in the beginning of the game
    private Node SetRespawnPoint()
    {
        Vector2Int coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        return gridManager.Grid[coordinates];
    }
    public bool PlayerWithinRange()
    {
        if (playerCollider.enabled && enemy.DistanceFromPlayer <= huntRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TravelToHiding()
    {
        PrepareThePath();
        StartCoroutine(FollowPath());
    }

    private void PrepareThePath()
    {
        Vector2Int coordinates = new Vector2Int();

        coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates, ChooseRandomHidingSpot());
    }
    private Vector2Int ChooseRandomHidingSpot()
    {
        int rnd = Random.Range(0, hidingNodes.Count);
        return hidingNodes[rnd].coordiantes;
    }

    IEnumerator FollowPath()
    {
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = gridManager.GetPositionFromCoordinates(path[i].coordiantes);
            endPos.y = transform.position.y;

            float travelPercent = 0f;

            transform.LookAt(endPos);

            while (travelPercent < 1f)
            {

                travelPercent += Time.deltaTime * speed;

                transform.position = Vector3.Lerp(startPos, endPos, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
