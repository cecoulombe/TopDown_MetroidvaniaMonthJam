using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_TopDown : MonoBehaviour
{
    #region Variables

    public PlayerController player;
    private Vector3 playerStartPoint;

    public DeathMenu_TopDown deathScreen;
    #endregion

    void Start()
    {
        playerStartPoint = player.transform.position;
    }

    public void ReloadLevel()
    {
        player.gameObject.SetActive(false);

        deathScreen.gameObject.SetActive(true);

        StartCoroutine("RestartGameCo");
    }

    public void Reset()
    {
        deathScreen.gameObject.SetActive(false);
        player.transform.position = playerStartPoint;
        player.gameObject.SetActive(true);
    }

    public IEnumerator RestartGameCo()
    {
        yield return new WaitForSeconds(0.5f);

        player.transform.position = playerStartPoint;
        player.gameObject.SetActive(true);
    }
}