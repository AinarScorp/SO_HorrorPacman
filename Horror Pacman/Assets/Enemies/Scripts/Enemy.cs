using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Manages everything enemy related.
 * responsible for changing behaviours
 * For example, manages whether enemies are hidden or visible based on the distance from the player
 * 
 */

public class Enemy : MonoBehaviour
{

    [SerializeField] EnemyBehaviour startingBehaviour;
    [SerializeField] bool isHound;
    [SerializeField] bool isGlower;
    [SerializeField] Light glowerBlueLight;
    Player player;


    float distanceFromPlayer;
    public float DistanceFromPlayer { get { return distanceFromPlayer; } }

    public Player Player { get => player; }

    EnemyChase chase;
    EnemyWander wander;
    EnemyHoundAlarm houndAlarm;
    EnemyGlowerHunt glowerHunt;

    private void Awake()
    {
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        //enemyRenderer = GetComponent<Renderer>();

        //No need for now isHound or isGlower because if they don't have these components it will be just null

        GetBehaviours();

        Invoke("StartInitialBehavior", 1f);
    }

    private void GetBehaviours()
    {
        glowerHunt = GetComponent<EnemyGlowerHunt>();
        houndAlarm = GetComponent<EnemyHoundAlarm>();
        chase = GetComponent<EnemyChase>();
        wander = GetComponent<EnemyWander>();
    }

    void Update()
    {
        distanceFromPlayer = Vector3.Distance(Player.transform.position, transform.position);

        if (!isHound && !isGlower && chase.enabled == false && CanSeePlayer())
        {
            StartChasing();
        }
        else if (isHound)
        {
            StartHoundBehaviour();
        }

        ShowHideOnTheMap();
    }
    void StartHoundBehaviour()
    {
        if (!houndAlarm.enabled && CanSeePlayer())
        {
            HowlToAlarmEnemies();
        }
    }

    private void ShowHideOnTheMap()
    {
        //if (distanceFromPlayer <= Player.VisionRange)
        //{
        //    foreach (Renderer r in rs)
        //    {
        //        if (glowerBlueLight != null)
        //        {
        //            glowerBlueLight.enabled = true;
        //        }
        //        r.enabled = true;
        //    }
        //}
        //else
        //{
        //    foreach (Renderer r in rs)
        //    {
        //        if (glowerBlueLight != null)
        //        {
        //            glowerBlueLight.enabled = false;
        //        }
        //        r.enabled = false;
        //    }
        //}
    }

    void StartInitialBehavior() // it is invoked if you are not sure what it means ask Einar before working with it
    {
        if (startingBehaviour != null)
        {
            startingBehaviour.enabled = true;
        }
    }

    void HowlToAlarmEnemies()
    {
        wander.enabled = false;
        houndAlarm.enabled = true;
    }

    public IEnumerator StopHowling(float howlingLength)
    {
        yield return new WaitForSeconds(howlingLength);
        houndAlarm.enabled = false;
        wander.enabled = true;
    }

    public void StartChasing()
    {
        if (wander != null)
        {
            wander.enabled = false;

        }
        if (glowerHunt != null)
        {
            glowerHunt.enabled = false;
        }
        if (chase.enabled == false)
        {
            chase.enabled = true;
        }
    }

    public void StopChasing()
    {
        if (wander != null)
        {
            wander.enabled = true;

        }
        if (glowerHunt != null)
        {
            glowerHunt.enabled = true;
        }
        chase.enabled = false;
    }
    // shoots raycasts in 3 directions and returns true if it hits the player collider
    public bool CanSeePlayer()
    {
        if (isGlower)
        {
            return glowerHunt.PlayerWithinRange();
        }
        RaycastHit hit;
        Vector3 centerCollider = transform.TransformPoint(gameObject.GetComponent<BoxCollider>().center);
        Ray ray = new Ray(centerCollider, transform.forward);

        Debug.DrawRay(centerCollider, transform.forward, Color.red);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                return true;

            }
        }
        ray.direction = -transform.right;
        Debug.DrawRay(centerCollider, -transform.right, Color.blue);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                return true;

            }
        }
        ray.direction = transform.right;
        Debug.DrawRay(centerCollider, transform.right, Color.green);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                return true;

            }
        }
        return false;
    }
}
