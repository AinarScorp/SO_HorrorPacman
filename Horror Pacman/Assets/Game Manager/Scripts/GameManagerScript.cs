using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * Manages the keys, and whether the player can exit the level yet.
 * Also displays notifications for the player.
 */

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] float durationOfMessages = 2f;
    [SerializeField] TextMeshProUGUI displayKeys;

    int requiredKeys;
    int currentNumberOfKeys = 0;
    public int CurrentNumberOfKeys { get { return currentNumberOfKeys; } }
    TextMeshProUGUI messageForPlayer;

    void Start()
    {
        CountKeysOnTheLevel();
        messageForPlayer = GameObject.Find("MessageForPlayer").GetComponent<TextMeshProUGUI>();
        messageForPlayer.enabled = false;
        if (messageForPlayer == null) 
        {
            Debug.Log("messageForPlayer is null check Gamemanager script");
        }
    }

    void CountKeysOnTheLevel()
    {
        requiredKeys = GameObject.FindGameObjectsWithTag("Key").Length;
        UpdateKeys();
    }

    public void PickedUpKey()
    {
        currentNumberOfKeys += 1;
        EnoughKeysToOpenDoor();
        UpdateKeys();
    }

    private void EnoughKeysToOpenDoor()
    {
        if (currentNumberOfKeys >= requiredKeys)
        {
            Door door = FindObjectOfType<Door>();

            BoxCollider boxCollider;
            boxCollider = door.gameObject.GetComponent<BoxCollider>();
            door.gameObject.layer = 0;
            if (boxCollider == null)
            {
                Debug.Log("have not found box collider on the door");
            }
            boxCollider.isTrigger = true;
            Tile[] tiles = FindObjectsOfType<Tile>();
            foreach (Tile tile in tiles)
            {
                if (tile.IsDoorTile)
                {
                    GridManager gridManager = FindObjectOfType<GridManager>();
                    gridManager.OpenDoor(tile.Coordinates);
                }
            }
            string message = "The Door is Open, time to escape";
            door.DoorOpen();
            DisplayMessage(message);
        }
    }

    void UpdateKeys()
    {
        displayKeys.text = string.Format("{0:0}/{1:0}", currentNumberOfKeys, requiredKeys);
    }

    public void DisplayMessage(string message)
    {
        StartCoroutine(StartDisplayingMessage(message));
    }

    IEnumerator StartDisplayingMessage(string message)
    {
        messageForPlayer.text = message;
        messageForPlayer.enabled = true;
        yield return new WaitForSeconds(durationOfMessages);
        messageForPlayer.enabled = false;
    }
}
