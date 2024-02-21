using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MeleeInstructions : MonoBehaviour
{
    public GameObject meleeInstructions;

    private bool gotMelee;

    public GameObject meleeFirstButton;

    private void Update()
    {
        gotMelee = GameStatus.GetInstance().OpenMeleeInstructions();

        if (gotMelee)
        {
            Debug.Log("opening the pause menu");
            GameStatus.GetInstance().SetOpenMeleeInstructions(false);
            OpenInstructions();
        }
    }

    public void OpenInstructions()
    {
        Time.timeScale = 0f;
        meleeInstructions.SetActive(true);

        // clear the event system before setting the first button
        EventSystem.current.SetSelectedGameObject(null);
        // set a new selected object using the first button
        EventSystem.current.SetSelectedGameObject(meleeFirstButton);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        GameStatus.GetInstance().SetGamePaused(false);
        meleeInstructions.SetActive(false);
    }
}