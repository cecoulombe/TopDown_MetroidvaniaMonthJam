using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room1 : MonoBehaviour
{
    #region Variables
    public EnemyAttack_TopDown[] enemyList;

    public GameObject roomAfterGate;

    private float deathCounter;

    [SerializeField]
    private GameObject Room1_EnemyGate;
    #endregion

    private void Update()
    {
        Enemies();
        if(GameStatus.GetInstance().GetGateState(Room1_EnemyGate) == false)
        {
            roomAfterGate.SetActive(true);
        }
    }

    private void Enemies()
    {
        deathCounter = 0;
        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i].isDead)
            {
                deathCounter++;
                if (deathCounter == enemyList.Length)
                {
                    //enemyGate.SetActive(false);
                    GameStatus.GetInstance().SetGateState(Room1_EnemyGate, false);
                }
            }
        }
    }
}