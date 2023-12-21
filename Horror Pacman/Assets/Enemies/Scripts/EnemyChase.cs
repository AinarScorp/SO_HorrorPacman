using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script is responsible to follow the last registered player position
 * this script calculates the best possible way to get there
 * if the enemy is in the middle of the chase and sees the player it will recalculate the path again
 * If you walk out of range from them, they will go to the tile you were last seen on and then go back to wandering.
 */

public class EnemyChase : EnemyBehaviour
{
    [SerializeField] [Range(0.1f, 20f)] float speed = 2f;
    [SerializeField] Animator animator;
    List<Node> path = new List<Node>();

    private void OnEnable()
    {
        RecalculatePath();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void RecalculatePath()
    {
        if (this.enabled)
        {
            Vector2Int coordinates = new Vector2Int();
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            StopAllCoroutines();
            path.Clear();
            path = pathFinder.GetNewPath(coordinates, pathFinder.PlayerLocationNode.coordiantes);
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
    {
        if (animator != null) {animator.SetBool("IsChasing", true); }

        Node playerLocationNode = pathFinder.PlayerLocationNode;

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

            if (playerLocationNode != pathFinder.PlayerLocationNode && enemy.CanSeePlayer()) // in theory this have to register a player's new postion if it an enemy logically can see the player
            {
                RecalculatePath();
            }
        }
        enemy.StopChasing();
        if (animator != null) {animator.SetBool("IsChasing", false);}
    }
}
