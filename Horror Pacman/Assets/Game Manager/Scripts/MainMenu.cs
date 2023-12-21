using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Depending on the state of the game this script loads different parts of the main menu
 * Manages the main menu. We have an Enum that manages the current state of the game.
 */

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject StartMenu;
    [SerializeField] GameObject LevelSelector;
    [SerializeField] GameObject LoseScreen;
    [SerializeField] GameObject WinScreen;


    private void Start()
    {
        if (GameState.currentPlayState == GameState.PlayState.WON)
        {
            WinScreen.gameObject.SetActive(true);
        }
        else if (GameState.currentPlayState == GameState.PlayState.LOST)
        {
            LoseScreen.gameObject.SetActive(true);
        }
        else
        {
            StartMenu.gameObject.SetActive(true);
        }
    }

    public void SwitchToLevelSelect()
    {
        StartMenu.gameObject.SetActive(false);
        LevelSelector.gameObject.SetActive(true);
    }
}
