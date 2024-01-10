using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string playGameLevel;
    public string letterLevel;

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(playGameLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenLetter()
    {
        SceneManager.LoadScene(letterLevel);
    }
}
