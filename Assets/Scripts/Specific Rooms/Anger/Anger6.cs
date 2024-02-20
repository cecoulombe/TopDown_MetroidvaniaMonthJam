using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Anger6 : MonoBehaviour
{
    #region Variables
    public EnemyHealth[] enemyList;

    private float enemyDeathCounter;

    private string currentRoom;
    private string previousRoom;

    [Header("Loading zones for each door")]
    [SerializeField]
    private PlayerController_TopDown player;

    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject Anger5_LoadingZone;
    [SerializeField]
    private GameObject Anger2_LoadingZone;
    [SerializeField]
    private GameObject Anger7_LoadingZone;



    // for every set of variables in this script, make sure to add the corresponding tracker to the game status
    [Header("Gates/doors/chests/breakable walls tracking")]
    [Header("Gates")]
    [SerializeField]
    private GameObject Anger6_EnemyGate;
    [SerializeField]
    private GameObject Anger6_RoomAfterGate;
    [SerializeField]
    private bool Anger6_enemyGateOpen;

    [Header("Hidden Walls")]
    [SerializeField]
    private GameObject Anger2_and6_Hidden_Closed;
    [SerializeField]
    private GameObject Anger2_and6_Hidden_Opened;
    [SerializeField]
    private bool Anger2_and6_HiddenOpen;

    [Header("Permanent Upgrades")]
    [Header("Melee")]
    [SerializeField]
    private GameObject Anger2_MeleeUpgrade;
    [SerializeField]
    private bool Anger2_MeleeUpgradeTaken;
    #endregion

    private void Start()
    {
        // I want to know the room the player came from so that I can load them in at the right spot
        previousRoom = GameStatus.GetInstance().GetPreviousRoom();

        // determine where in the room to spawn players based on the previous room they were in
        if (previousRoom == "spawn")
        {
            player.transform.position = spawnPoint.transform.position;
        }
        else if (previousRoom == "MainMenu")  // if you are continuing a previous file, you will spawn in the last room you were in at the main menu location
        {
            player.transform.position = spawnPoint.transform.position;
        }
        else if (previousRoom == "Anger5")   // repeat this for each transition
        {
            player.transform.position = Anger5_LoadingZone.transform.position;
        }
        else if (previousRoom == "Anger2")   // repeat this for each transition
        {
            player.transform.position = Anger2_LoadingZone.transform.position;
        }
        else if (previousRoom == "Anger7")   // repeat this for each transition
        {
            player.transform.position = Anger7_LoadingZone.transform.position;
        }
    }

    private void Update()
    {

        currentRoom = SceneManager.GetActiveScene().name;
        if (currentRoom == "roomName")
        {
            #region Gates
            Anger6_enemyGateOpen = GameStatus.GetInstance().GetGateState(currentRoom);
            if (!Anger6_enemyGateOpen)  // the gate is not open, so keep it closed and hide the room after
            {
                Debug.Log("closed gate");
                Anger6_EnemyGate.SetActive(true);
                Anger6_RoomAfterGate.SetActive(false);
            }
            else    // the gate is already open, so get rid of it and show the room after
            {
                Debug.Log("open gate");
                Anger6_EnemyGate.SetActive(false);
                Anger6_RoomAfterGate.SetActive(true);
            }
            #endregion

            #region Hidden Rooms
            Anger2_and6_HiddenOpen = GameStatus.GetInstance().GetHiddenState(currentRoom);
            if (!Anger2_and6_HiddenOpen)  // the wall is closed
            {
                Debug.Log("closed wall");
                Anger2_and6_Hidden_Closed.SetActive(true);
                Anger2_and6_Hidden_Opened.SetActive(false);
            }
            else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
            {
                Debug.Log("open wall");
                Anger2_and6_Hidden_Closed.SetActive(false);
                Anger2_and6_Hidden_Opened.SetActive(true);
            }
            #endregion

            #region Permanent Upgrades: Melee
            Anger2_MeleeUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Melee");
            if (!Anger2_MeleeUpgradeTaken)
            {
                // the health upgrade has not been picked up, so turn it on
                Anger2_MeleeUpgrade.SetActive(true);
                Enemies();
            }
            else
            {
                Anger6_EnemyGate.SetActive(true);
                Anger2_MeleeUpgrade.SetActive(false);
            }
            #endregion
        }

        // if there is an enemy gate, you need this method
        
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
                    GameStatus.GetInstance().SetPlayerPrefs();
                }
            }
        }
    }
}