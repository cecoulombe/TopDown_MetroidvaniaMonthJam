using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        GameStatus.GetInstance().SetPreviousRoom(GameStatus.GetInstance().GetCurrentRoom());
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadRoom()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
