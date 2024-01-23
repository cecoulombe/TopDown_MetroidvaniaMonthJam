using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    //public SceneLoader sceneLoader;

    public DeathMenu_TopDown deathScreen;

    public PlayerController_TopDown player;

    public SceneLoader sceneLoader;

    private float maxHealth;

    private void Start()
    {
        sceneLoader = GameObject.FindObjectOfType(typeof(SceneLoader)) as SceneLoader;
    }

    public void Death()
    {
        player.gameObject.SetActive(false);
        GameStatus.GetInstance().AddDeath();
        deathScreen.gameObject.SetActive(true);

        //StartCoroutine("RestartGameCo");

    }

    public void ReloadLevel()
    {
        deathScreen.gameObject.SetActive(false);
        sceneLoader.ReloadRoom();
        //SceneManager.ReloadRoom();
        maxHealth = GameStatus.GetInstance().GetMaxHealth();
        GameStatus.GetInstance().AddHealth(maxHealth * 10);
    }
}
