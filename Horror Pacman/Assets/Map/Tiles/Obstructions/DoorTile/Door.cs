using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Manages the door.
 * Makes the player travel to the next scene when entered.
 * Can only be entered when a cetain number of keys is collected.
 */

public class Door : MonoBehaviour
{
    AudioSource doorOpenSource;
    SceneControl sceneControl;
    [SerializeField] AudioClip doorOpenSound;

    private void Start()
    {
        sceneControl = FindObjectOfType<SceneControl>();
        doorOpenSource = GetComponent<AudioSource>();
        BoxCollider objectCollider = GetComponent<BoxCollider>();
        objectCollider.isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        sceneControl.LoadNextScene();
    }

    public void DoorOpen()
    {
        StartCoroutine(StartDoorSound());
    }

    IEnumerator StartDoorSound()
    {
        yield return new WaitForSeconds(0.5f);
        doorOpenSource.PlayOneShot(doorOpenSound);
    }
}

