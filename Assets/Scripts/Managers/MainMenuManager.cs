using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    public GameObject firstButton;

    private void Start()
    {
        // clear the event system before setting the first button
        EventSystem.current.SetSelectedGameObject(null);
        // set a new selected object using the first button
        EventSystem.current.SetSelectedGameObject(firstButton);
    }


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
        Debug.Log("continuing the game from room: " + saveRoom);
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