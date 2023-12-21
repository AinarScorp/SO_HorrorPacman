using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Manages the picksup & actually picking up the items.
 * Every pickup has a unique tag that identifies them, wihch is then sent over to the player.
 * The player then activates its ability.
 * Alternatively if it's a key, it's sent to the game manager.
 */

public class PickupScript : MonoBehaviour
{
    [SerializeField] GameManagerScript gameManagerScript;
    [SerializeField] AudioClip pickupSound;

    Light pickupLight;
    AudioSource pickupSource;
    Player player;

    private void Awake()
    {
        player =  FindObjectOfType<Player>();

        gameManagerScript = FindObjectOfType<GameManagerScript>();
    }
    void Start()
    {
        GameObject lightGameObject = new GameObject("Pickup Light");
        pickupLight = lightGameObject.AddComponent<Light>();
        pickupSource = GetComponent<AudioSource>();

        pickupLight.color = Color.yellow;
        pickupLight.intensity = 50;
        pickupLight.range = 0.5f;

        lightGameObject.transform.position = transform.position;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 100);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            string itemName = gameObject.tag;
            if (itemName == "Key")
            {
                gameManagerScript.PickedUpKey();
            }
            else
            {
                player.PickedUpItem(itemName);
            }
            StartCoroutine(KillTheItem());
        }
    }

    IEnumerator KillTheItem()
    {
        pickupSource.PlayOneShot(pickupSound);// TODO here you can make that it destroys immidiately but the sound plays
        yield return new WaitForSeconds(0.4f);
        Destroy(pickupLight);
        gameObject.SetActive(false);
    }
}
