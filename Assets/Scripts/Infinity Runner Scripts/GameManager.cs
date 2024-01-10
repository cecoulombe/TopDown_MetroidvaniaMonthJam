using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    public Transform platformGenerator;
    private Vector3 platformStartPoint;

    public PlayerController player;
    private Vector3 playerStartPoint;

    private PlatformDestroyer[] platformList;

    private ScoreManager scoreManager;

    public DeathMenu deathScreen;
    #endregion

    void Start()
    {
        platformStartPoint = platformGenerator.position;
        playerStartPoint = player.transform.position;

        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void RestartGame()
    {
        scoreManager.scoreIncreasing = false;
        player.gameObject.SetActive(false);

        deathScreen.gameObject.SetActive(true);

        //StartCoroutine("RestartGameCo");
    }

    public void Reset()
    {
        deathScreen.gameObject.SetActive(false);

        platformList = FindObjectsOfType<PlatformDestroyer>();
        for (int i = 0; i < platformList.Length; i++)
        {
            platformList[i].gameObject.SetActive(false);
        }
        player.transform.position = playerStartPoint;
        platformGenerator.position = platformStartPoint;
        player.gameObject.SetActive(true);

        scoreManager.scoreCount = 0;
        scoreManager.scoreIncreasing = true;
    }

    //public IEnumerator RestartGameCo()
    //{

    //    yield return new WaitForSeconds(0.5f);
    //    platformList = FindObjectsOfType<PlatformDestroyer>();
    //    for(int i = 0; i < platformList.Length; i++)
    //    {
    //        platformList[i].gameObject.SetActive(false);
    //    }
    //    player.transform.position = playerStartPoint;
    //    platformGenerator.position = platformStartPoint;
    //    player.gameObject.SetActive(true);

    //    scoreManager.scoreCount = 0;
    //    scoreManager.scoreIncreasing = true;
    //}
}
