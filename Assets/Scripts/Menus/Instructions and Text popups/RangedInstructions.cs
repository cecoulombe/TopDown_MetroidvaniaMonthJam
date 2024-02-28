using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class RangedInstructions : MonoBehaviour
{
    public GameObject rangedInstructions;

    private bool gotRanged;

    public GameObject rangedFirstButton;

    private void Update()
    {
        gotRanged = GameStatus.GetInstance().OpenRangedInstructions();

        if (gotRanged)
        {
            Debug.Log("opening the ranged instructions");
            GameStatus.GetInstance().SetOpenRangedInstructions(false);
            OpenInstructions();
        }
    }

    public void OpenInstructions()
    {
        Time.timeScale = 0f;
        rangedInstructions.SetActive(true);

        // clear the event system before setting the first button
        EventSystem.current.SetSelectedGameObject(null);
        // set a new selected object using the first button
        EventSystem.current.SetSelectedGameObject(rangedFirstButton);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        GameStatus.GetInstance().SetGamePaused(false);
        rangedInstructions.SetActive(false);
    }
}