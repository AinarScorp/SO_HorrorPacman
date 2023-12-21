using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Manages the player's movement.
 * Has input for a speed modifier, which is from the energy drink powerup.
 * Also has inputs for moving around on the mpa.
 * in addition to that it resets the player position to it's starting position(stored at the Start) after losing a life
 */

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerSpeed = 2f;
    [SerializeField] float animaitonTime = 1.4f;
    [SerializeField]Animator animator;

    float speedModifier = 1f;
    int directionNumber;  // 1- up 2- right 3- down - 4 - left
    bool finishedMovement = true;
    bool animationRunning = false;
    bool isDying;

    Node startingTile;
    GridManager gridManager;
    PathFinder pathFinder;
    BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    private void Start()
    {
        SetRespawnPoint();
    }

    void Update()
    {
        Movement();
    }

    public void ChangeSpeedModifier(float modifier)
    {
        speedModifier = modifier;
    }

    void Movement()
    {
        if (!isDying)
        {

            PressKeysToMove();
        }
    }

    private void PressKeysToMove()
    {
        if (Input.GetKey(KeyCode.W) && ReadyToTakeInput(1))
        {
            StartWalkingIfPossible(ChooseDirection(0, 1));
        }
        else if (Input.GetKey(KeyCode.S) && ReadyToTakeInput(3))
        {
            StartWalkingIfPossible(ChooseDirection(0, -1));
        }
        else if (Input.GetKey(KeyCode.D) && ReadyToTakeInput(2))
        {
            StartWalkingIfPossible(ChooseDirection(1, 0));
        }
        else if (Input.GetKey(KeyCode.A) && ReadyToTakeInput(4))
        {
            StartWalkingIfPossible(ChooseDirection(-1, 0));
        }
    }

    bool ReadyToTakeInput(int number)
    {
        if (directionNumber == number || !finishedMovement)
        {
            return false;
        }
        else
        {
            directionNumber = number;
            return true;
        }
    }

    Vector2Int ChooseDirection(int x, int y)
    {
        Vector2Int coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        coordinates.x += x;
        coordinates.y += y;
        return coordinates;
    }
    void StartWalkingIfPossible(Vector2Int coordinates)
    {
        if (PossibleToWalk(coordinates))
        {
            StopAllCoroutines();
            pathFinder.UpdatePlayerLocation(gridManager.Grid[coordinates]);
            StartCoroutine(FollowDirection(coordinates));
        }
        else
        {
            return;
        }
    }

    bool PossibleToWalk(Vector2Int coordinates)
    {
        if (gridManager.Grid[coordinates].isWalkable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator FollowDirection(Vector2Int coordinates)
    {
        ToggleMoveAnimation();
        finishedMovement = false;
        Vector3 startPos = transform.position;
        Vector3 endPos = gridManager.GetPositionFromCoordinates(coordinates);
        endPos.y = transform.position.y;
        float travelPercent = 0f;
        transform.LookAt(endPos);
        while (travelPercent < 1f)
        {
            travelPercent += Time.deltaTime * playerSpeed * speedModifier;
            transform.position = Vector3.Lerp(startPos, endPos, travelPercent);
            yield return new WaitForEndOfFrame();
        }
        finishedMovement = true;
        directionNumber = 0;
        ToggleMoveAnimation();
    }
    private void ToggleMoveAnimation()
    {
        if (!animationRunning)
        {
            animator.SetBool("IsRunning", true);
            animationRunning = true;
        }
        else if (animationRunning)
        {
            animator.SetBool("IsRunning", false);
            animationRunning = false;
        }
    }

    void ToggleDeathAnimation()
    {
        animator.SetTrigger("hasDied");

    }

    public void GotCaught()
    {
        isDying = true;
        if (boxCollider.enabled)
        {
            ToggleCollider();


        }
        if (!finishedMovement)
        {
            StopAllCoroutines();
            ToggleMoveAnimation();


        }

        ToggleDeathAnimation();

        StartCoroutine(RespawnPlayerAfterAnimation(animaitonTime));
    }

    private void SetRespawnPoint()
    {
        Vector2Int coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        pathFinder.UpdatePlayerLocation(gridManager.Grid[coordinates]);
        startingTile = pathFinder.PlayerLocationNode;
    }

    IEnumerator RespawnPlayerAfterAnimation(float animaitonTime)
    {
        yield return new WaitForSeconds(animaitonTime);

        transform.position = new Vector3(startingTile.coordiantes.x, transform.position.y, startingTile.coordiantes.y);
        pathFinder.UpdatePlayerLocation(startingTile);
        directionNumber = 0;
        finishedMovement = true;
        ToggleCollider();
        isDying = false;

    }

    void ToggleCollider()
    {
        boxCollider.enabled = !boxCollider.enabled;
    }
}
