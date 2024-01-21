//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameManager_TopDown : MonoBehaviour
//{
//    #region Variables

//    public PlayerController_TopDown player;
//    private Vector3 playerStartPoint;

//    public DeathMenu_TopDown deathScreen;
//    public string currentLevel;

//    public EnemyAttack_TopDown[] enemyList;

//    public GameObject enemyGate;
//    public GameObject roomAfterGate;

//    private float deathCounter;
//    #endregion

//    void Start()
//    {
//        playerStartPoint = player.transform.position;
//    }

//    private void Update()
//    {
//        Enemies();
//    }

//    //public void Death()
//    //{
//    //    player.gameObject.SetActive(false);

//    //    deathScreen.gameObject.SetActive(true);

//    //    //StartCoroutine("RestartGameCo");
//    //}

//    //public void ReloadLevel()
//    //{
//    //    deathScreen.gameObject.SetActive(false);
//    //    sceneLoader.ReloadRoom();
//    //    health = maxHealth;
//    //}

//    private void Enemies()
//    {
//        deathCounter = 0;
//        for (int i = 0; i < enemyList.Length; i++)
//        {
//            if (enemyList[i].isDead)
//            {
//                deathCounter++;
//                if (deathCounter == enemyList.Length)
//                {
//                    enemyGate.SetActive(false);
//                    roomAfterGate.SetActive(true);
//                }
//            }
//        }
//    }
//}