using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeathManager : MonoBehaviour
{
    //public SceneLoader sceneLoader;

    public DeathMenu_TopDown deathScreen;

    public PlayerController_TopDown player;

    public SceneLoader sceneLoader;

    private float maxHealth;

    public GameObject deathFirstButton;

    private void Start()
    {
        sceneLoader = GameObject.FindObjectOfType(typeof(SceneLoader)) as SceneLoader;
    }

    public void Death()
    {
        player.gameObject.SetActive(false);
        GameStatus.GetInstance().AddDeath();
        deathScreen.gameObject.SetActive(true);

        Time.timeScale = 0f;

        // clear the event system before setting the first button
        EventSystem.current.SetSelectedGameObject(null);
        // set a new selected object using the first button
        EventSystem.current.SetSelectedGameObject(deathFirstButton);
        //StartCoroutine("RestartGameCo");

    }

    public void ReloadLevel()
    {
        deathScreen.gameObject.SetActive(false);
        sceneLoader.ReloadRoom();
        Time.timeScale = 1f;

        //SceneManager.ReloadRoom();
        GameStatus.GetInstance().AddHealth(GameStatus.GetInstance().GetMaxHealth());
        GameStatus.GetInstance().AddAmmo(GameStatus.GetInstance().GetMaxAmmo());
    }
}
