using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartNewGame : MonoBehaviour
{
    public GameObject _ImSureButton;
    public GameObject maybeNotButton;

    public GameObject mainMenuButtons;

    private void Start()
    {
        // clear the event system before setting the first button
        EventSystem.current.SetSelectedGameObject(null);
        // set a new selected object using the first button

        EventSystem.current.SetSelectedGameObject(_ImSureButton);
    }


    public void StartNew(string FirstRoom)
    {
        // have a popup to confirm that they want to erase the previous save file and start a new game
        GameStatus.GetInstance().ResetPlayerPrefs();
        GameStatus.GetInstance().LoadSettings();
        SceneManager.LoadScene(FirstRoom);         // replace this with whatever you end up making the first level of the game
    }

    public void Nevermind()
    {
        gameObject.SetActive(false);
        mainMenuButtons.SetActive(true);
    }
}