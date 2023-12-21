using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * this script is able to store informtaion about cetrain coordinates on the map and when it is active the game object randomly picks those
 * and travels between them
 * this script is also responsible for calculating shortest ways to reach those destinations;
 */

public class EnemyWander : EnemyBehaviour
{
    List<Node> path = new List<Node>();
    [SerializeField] [Range(0.1f, 20f)] float speed = 2f;

    [SerializeField] Vector2Int[] destinationCoordinates;

    List<Node> destinationNodes = new List<Node>();
    private void OnEnable()
    {
        Vector2Int startingPosCoord = gridManager.GetCoordinatesFromPosition(transform.position);
        destinationNodes.Add(gridManager.Grid[startingPosCoord]);
        PrepareDestinationsForTravel();
        RecalculateWanderPath();
    }
    private void OnDisable()
    {
        StopAllCoroutines();

    }
    
    void PrepareDestinationsForTravel()
    {
        foreach (Vector2Int destination in destinationCoordinates)
        {
            if (gridManager.Grid[destination] != null)
            {
                destinationNodes.Add(gridManager.Grid[destination]);
            }
        }
    }

    private Vector2Int ChooseRandomDestination()
    {
        int rnd = Random.Range(0, destinationNodes.Count);
        return destinationNodes[rnd].coordiantes;
    }
    void RecalculateWanderPath()
    {
        if (this.enabled)
        {
            Vector2Int coordinates = new Vector2Int();

            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            StopAllCoroutines();
            path.Clear();
            path = pathFinder.GetNewPath(coordinates, ChooseRandomDestination());
            StartCoroutine(FollowPath());

        }


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
        RecalculateWanderPath();
    }


}