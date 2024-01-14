using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu_TopDown : MonoBehaviour
{
    public string mainMenu_TopDown;

    public void ReloadLevel()
    {
        FindObjectOfType<GameManager_TopDown>().ReloadLevel();
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene(mainMenu_TopDown);
    }
}