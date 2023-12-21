using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/*
 * Manages everything player related.
 * Also manages pickups that the player has collected and sends signals to other corresponding scripts.
 */

public class Player : MonoBehaviour
{
    [SerializeField] float drinkSpeedModifier;
    [SerializeField] [Range(1, 20)] float visionRange;
    [SerializeField] KeyCode zoomKey;

    CinemachineVirtualCamera playerCam;
    CinemachineVirtualCamera mapCam;

    public float VisionRange { get { return visionRange; } }

    bool cameraZoomedIn;
    bool hasMap;
    LightScript lightScript;
    PlayerMovement playerMovement;

    private void Start()
    {
        hasMap = true;
        cameraZoomedIn = true;
        SetUpCamera();
        lightScript = FindObjectOfType<LightScript>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void SetUpCamera() //Manages the camera to follow & look at the player in the beginning of the game.
    {
        mapCam = GameObject.Find("MapCamera").GetComponent<CinemachineVirtualCamera>();
        playerCam = GameObject.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        playerCam.Follow = transform;
        mapCam.Follow = transform;
        playerCam.LookAt = playerCam.Follow;
        mapCam.LookAt = mapCam.Follow;
    }

    void Update()
    {
        if (Input.GetKeyDown(zoomKey) && hasMap == true)
        {
            SwitchCamera();
        }
    }

    void VisionRangeChange()
    {
        visionRange = 4;
    }
    //when the player picks up an item that item calls for this method and depending on it's name it affects the player in different ways.
    public void PickedUpItem(string itemName)
    {
        if (itemName == "Map")
        {
            GameManagerScript gameManager = FindObjectOfType<GameManagerScript>();
            string message = $"press " + zoomKey + " to change camera view";
            gameManager.DisplayMessage(message);
            hasMap = true;
        }
        else if(itemName == "Energydrink")
        {
            playerMovement.ChangeSpeedModifier(drinkSpeedModifier);
        }
        else if (itemName == "Flashlight")
        {
            lightScript.LightRanger();
            VisionRangeChange();
        }
    }

    private void SwitchCamera()
    {
        if (cameraZoomedIn)
        {
            mapCam.Priority = 1;
            playerCam.Priority = 0;
        }
        else
        {
            mapCam.Priority = 0;
            playerCam.Priority = 1;
        }
        cameraZoomedIn = !cameraZoomedIn;
    }
}
