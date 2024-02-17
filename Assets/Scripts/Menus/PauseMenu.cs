using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public string MainMenu;

    public GameObject pauseMenu;

    private bool gamePaused;

    public GameObject pauseFirstButton;

    private void Update()
    {
        gamePaused = GameStatus.GetInstance().GetGamePaused();

        if(gamePaused)
        {
            Debug.Log("opening the pause menu");
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);

        // clear the event system before setting the first button
        EventSystem.current.SetSelectedGameObject(null);
        // set a new selected object using the first button
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        GameStatus.GetInstance().SetGamePaused(false);
        pauseMenu.SetActive(false);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        GameStatus.GetInstance().SetSaveRoom();
        GameStatus.GetInstance().SetGamePaused(false);
        pauseMenu.SetActive(false);
        SceneManager.LoadScene(MainMenu);
    }
}