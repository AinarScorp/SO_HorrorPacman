using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  Manages the hearts of the player.
 *  Also manages damage of the player.
 */

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int wounds = 0;
    PlayerMovement playerMovement;

    //hearts UI
    [SerializeField] Transform heartsOnScreen;
    [SerializeField] List<Image> heartImages;
    [SerializeField] Sprite brokenHeart;


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Start()
    {
        ReferenceHearts();
    }

    private void ReferenceHearts()
    {
        wounds = 0;
        //don't forget to drag HeartsOnScreenComponent into this script on the player
        foreach (Transform heartImage in heartsOnScreen)
        {
            heartImages.Add(heartImage.gameObject.GetComponent<Image>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy>() != null)
        {
            GetDamaged();
        }
    }

    void GetDamaged()
    {
        playerMovement.GotCaught();
        HandleDamage();
        ResetEnemies();
    }

    void HandleDamage()
    {
        wounds += 1;
        if (wounds < heartImages.Count)
        {
            heartImages[wounds - 1].sprite = brokenHeart;
        }
        else
        {
            GameOverPlayerDied();
        }
    }
    //when the player loses a life it finds a certain enemy and forces it to find a new location
    void ResetEnemies()
    {
        EnemyGlowerHunt[] glowers = FindObjectsOfType<EnemyGlowerHunt>();
        foreach (EnemyGlowerHunt glower in glowers)
        {
            glower.TravelToHiding();
        }
    }

    private void GameOverPlayerDied()
    {
        GameState.currentPlayState = GameState.PlayState.LOST;
        SceneControl scenecontrol = FindObjectOfType<SceneControl>();
        scenecontrol.GoToMainMenu();
    }
}
