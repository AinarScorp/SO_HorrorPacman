using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The Hound utilises a unique behavior in which it signals your location to every enemy on the map (excluding Glowers).
 * It in turn activates the Chase script, turns off wander scripts,(if those are not null) causing every enemy to chase the latest location you were seen.
 */

public class EnemyHoundAlarm : EnemyBehaviour
{
    Enemy[] enemies;
    AudioSource audioSource;
    [SerializeField] AudioClip howlingSound;
    [SerializeField] float howlingTimer = 2f;
    [SerializeField] Animator animator;

    bool howledOnce = false;


    private void OnEnable()
    {
        if (!howledOnce)
        {
            Invoke("SendHoundTutorialOnDisplay", 1f);
        }
        if (animator != null) { animator.SetTrigger("IsHowling"); }

        audioSource = GetComponent<AudioSource>();

        audioSource.PlayOneShot(howlingSound);
        enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].gameObject.GetComponent<EnemyChase>() != null)
            {
                if (enemies[i].gameObject.GetComponent<EnemyWander>() != null)
                {
                    enemies[i].gameObject.GetComponent<EnemyWander>().enabled = false;
                }
                if (enemies[i].gameObject.GetComponent<EnemyGlowerHunt>() == null)
                {

                    enemies[i].gameObject.GetComponent<EnemyChase>().enabled = true;
                }
            }
        }
        StartCoroutine(enemy.StopHowling(howlingTimer));
        
    }

    private void SendHoundTutorialOnDisplay()
    {
        string houndTutorial = "The hound has alerted other monsters! Beware, they know where you are.";
        gameManager.DisplayMessage(houndTutorial);
        howledOnce = true;
    }
}
