using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject startNewFirstButton;
    public GameObject continueFirstButton;

    public GameObject StartNewGameScreen;

    public GameObject mainMenuButtons;

    private void Start()
    {
        // clear the event system before setting the first button
        EventSystem.current.SetSelectedGameObject(null);
        // set a new selected object using the first button

        if(GameStatus.GetInstance().GetPreviousRoom() == "spawn")
        {
            Button continueFile = continueFirstButton.GetComponent<Button>();
            continueFile.interactable = false;
            EventSystem.current.SetSelectedGameObject(startNewFirstButton);
        }
        else
        {
            Button continueFile = continueFirstButton.GetComponent<Button>();
            continueFile.interactable = true;
            EventSystem.current.SetSelectedGameObject(continueFirstButton);
        }

    }


    public void StartNewGame(string FirstRoom)
    {
        if (GameStatus.GetInstance().GetPreviousRoom() == "spawn")
        {
            GameStatus.GetInstance().ResetPlayerPrefs();
            GameStatus.GetInstance().SetSaveRoom();
            GameStatus.GetInstance().LoadSettings();
            GameStatus.GetInstance().AddHealth(GameStatus.GetInstance().GetMaxHealth());
            SceneManager.LoadScene(FirstRoom);
        }
        else
        {
            // confirm that they actually want to restart
            StartNewGameScreen.SetActive(true);
            mainMenuButtons.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        string saveRoom = GameStatus.GetInstance().GetSaveRoom();
        Debug.Log("continuing the game from room: " + saveRoom);
        GameStatus.GetInstance().AddHealth(GameStatus.GetInstance().GetMaxHealth());
        SceneManager.LoadScene(saveRoom);
    }

    public void Options()
    {
        Debug.Log("Oops, you have pressed a button that doesn't work yet!");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game");
        Application.Quit();
    }
}