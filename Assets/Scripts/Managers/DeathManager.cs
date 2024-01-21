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

    public void Death()
    {
        player.gameObject.SetActive(false);

        deathScreen.gameObject.SetActive(true);

        //StartCoroutine("RestartGameCo");
    }

    public void ReloadLevel()
    {
        deathScreen.gameObject.SetActive(false);
        sceneLoader.ReloadRoom();
        maxHealth = GameStatus.GetInstance().GetMaxHealth();
        GameStatus.GetInstance().AddHealth(maxHealth * 10);
    }
}
