using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_TopDown : MonoBehaviour
{
    #region Variables

    public PlayerController_TopDown player;
    private Vector3 playerStartPoint;

    public DeathMenu_TopDown deathScreen;

    #endregion

    void Start()
    {
        playerStartPoint = player.transform.position;
    }

    public void Death()
    {
        player.gameObject.SetActive(false);

        deathScreen.gameObject.SetActive(true);

        //StartCoroutine("RestartGameCo");
    }

    public void ReloadLevel()
    {
        deathScreen.gameObject.SetActive(false);
        player.transform.position = playerStartPoint;
        player.gameObject.SetActive(true);
        player.Heal(player.maxHealth);
    }

    //public IEnumerator RestartGameCo()
    //{
    //    yield return new WaitForSeconds(0.5f);

    //    player.transform.position = playerStartPoint;
    //    player.gameObject.SetActive(true);
    //}
}