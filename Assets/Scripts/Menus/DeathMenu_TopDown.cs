using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu_TopDown : MonoBehaviour
{
    public string mainMenu_TopDown;
    public DeathManager deathManager;

    public void ReloadLevel()
    {
        //FindObjectOfType<GameManager_TopDown>().ReloadLevel();
        deathManager.ReloadLevel();
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(mainMenu_TopDown);
    }
}