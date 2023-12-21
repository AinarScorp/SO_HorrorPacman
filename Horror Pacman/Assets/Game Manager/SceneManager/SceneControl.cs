using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Manages which scene is currently active & also switches to the next scene whenever the player beats a level.
 * Can also load specific scenes.
 */

public class SceneControl : MonoBehaviour
{
    private void Update()
    {
        RegisterInput();
    }

    private void RegisterInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMainMenu();
        }
    }

    public void RestartLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
    public void LoadNextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentScene + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            GameState.currentPlayState = GameState.PlayState.WON;
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    public void LoadRequestedScene(LevelButton levelButton)
    {
        int index = levelButton.LvlIndex;
        int nextSceneIndex = index + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void CloseTheGame()
    {
        Application.Quit();
        Debug.Log("Closing the game...");
    }

}
