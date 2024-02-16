using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartNewGame(string FirstRoom)
    {
        // have a popup to confirm that they want to erase the previous save file and start a new game
        GameStatus.GetInstance().ResetPlayerPrefs();
        GameStatus.GetInstance().LoadSettings();
        SceneManager.LoadScene(FirstRoom);         // replace this with whatever you end up making the first level of the game

    }

    public void ContinueGame()
    {
        string saveRoom = GameStatus.GetInstance().GetSaveRoom();
        SceneManager.LoadScene(saveRoom);
    }

    public void Options()
    {
        Debug.Log("Oops, you have pressed a button that doesn't work yet!");
    }

    public void QuitGame()
    {
        Debug.Log("quitting the game");
        Application.Quit();
    }
}