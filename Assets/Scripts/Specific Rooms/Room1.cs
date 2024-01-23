using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room1 : MonoBehaviour
{
    #region Variables
    public EnemyAttack_TopDown[] enemyList;

    private float enemyDeathCounter;

    private string currentRoom;

    [Header("Gates/doors/chests tracking")]
    [SerializeField]
    private GameObject Room1_EnemyGate;
    [SerializeField]
    private GameObject Room1_RoomAfterGate;
    [SerializeField]
    private bool room1_enemyGateOpen;
    #endregion

    private void Update()
    {

        currentRoom = SceneManager.GetActiveScene().name;
        if (currentRoom == "Room1")     // this is how I will track whatever rooom we are in without looking at every single room in the game (hopefully)
        {
            //use this to check any/all gates and everything else in the room
            room1_enemyGateOpen = GameStatus.GetInstance().GetGateState(currentRoom);
            if (!room1_enemyGateOpen)  // the gate is open, so get rid of it and show the room after
            {
                Debug.Log("closed gate");
                Room1_EnemyGate.SetActive(true);
                Room1_RoomAfterGate.SetActive(false);
            }
            else    // the gate is not open, so keep it closed and hide the room after
            {
                Debug.Log("open gate");
                Room1_EnemyGate.SetActive(false);
                Room1_RoomAfterGate.SetActive(true);
            }
        }

        Enemies();

        
    }

    private void Enemies()
    {
        enemyDeathCounter = 0;
        for (int i = 0; i < enemyList.Length; i++)
        {
            if (enemyList[i].isDead)
            {
                Debug.Log("enemy dead");
                enemyDeathCounter++;
                if (enemyDeathCounter == enemyList.Length)
                {
                    Debug.Log("all enemies dead, opening the gate");
                    GameStatus.GetInstance().SetGateState(currentRoom);
                }
            }
        }
    }
}