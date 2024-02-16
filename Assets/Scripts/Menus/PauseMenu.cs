using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string MainMenu;

    public GameObject pauseMenu;

    private bool gamePaused;

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