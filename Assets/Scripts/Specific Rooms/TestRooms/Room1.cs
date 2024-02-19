using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room1 : MonoBehaviour
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
    private GameObject room2_LoadingZone;

    [Header("Enemy Gates")]
    [SerializeField]
    private GameObject Room1_EnemyGate;
    [SerializeField]
    private GameObject Room1_RoomAfterGate;
    [SerializeField]
    private bool room1_enemyGateOpen;

    [Header("Hidden Walls")]
    [SerializeField]
    private GameObject Room1_Hidden_Closed;
    [SerializeField]
    private GameObject Room1_Hidden_Opened;
    [SerializeField]
    private bool Room1_HiddenOpen;

    [Header("Permanent Upgrades")]
    [Header("Health")]
    [SerializeField]
    private GameObject room1_HealthUpgrade;
    [SerializeField]
    private bool room1_HealthUpgradeTaken;

    [Header("Ammo")]
    [SerializeField]
    private GameObject room1_AmmoUpgrade;
    [SerializeField]
    private bool room1_AmmoUpgradeTaken;
    #endregion

    private void Start()
    {
        // I want to know the room the player came from so that I can load them in at the right spot
        previousRoom = GameStatus.GetInstance().GetPreviousRoom();
        // I think I want to set the players load position here? Before any thing else can happen in the room? Lets give that a try
        if (previousRoom == "spawn")
        {
            player.transform.position = spawnPoint.transform.position;
        }
        else if (previousRoom == "MainMenu")    // if starting a new file from the main menu, then you will spawn into room 1
            // if you are continuing a previous file, you will spawn in the last room you were in at the main menu location
        {
            player.transform.position = spawnPoint.transform.position;
        }
        else if (previousRoom == "Room2")
        {
            player.transform.position = room2_LoadingZone.transform.position;
        }
    }

    private void Update()
    {

        currentRoom = SceneManager.GetActiveScene().name;
        if (currentRoom == "Room1")     // this is how I will track whatever rooom we are in without looking at every single room in the game (hopefully)
        {
            //use this to check any/all gates and everything else in the room
            room1_enemyGateOpen = GameStatus.GetInstance().GetGateState(currentRoom);
            if (!room1_enemyGateOpen)  // the gate is not open, so keep it closed and hide the room after
            {
                Debug.Log("closed gate");
                Room1_EnemyGate.SetActive(true);
                Room1_RoomAfterGate.SetActive(false);
            }
            else    // the gate is already open, so get rid of it and show the room after
            {
                Debug.Log("open gate");
                Room1_EnemyGate.SetActive(false);
                Room1_RoomAfterGate.SetActive(true);
            }

            #region Hidden Rooms
            Room1_HiddenOpen = GameStatus.GetInstance().GetHiddenState(currentRoom);
            if (!Room1_HiddenOpen)  // the wall is closed
            {
                Debug.Log("closed hidden");
                Room1_Hidden_Closed.SetActive(true);
                Room1_Hidden_Opened.SetActive(false);
            }
            else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
            {
                Debug.Log("open hidden");
                Room1_Hidden_Closed.SetActive(false);
                Room1_Hidden_Opened.SetActive(true);
            }
            #endregion

            #region Permanent Upgrades: Health
            room1_HealthUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Health");
            Debug.Log("Room 1 health and ammo resp: " + room1_HealthUpgradeTaken + room1_AmmoUpgradeTaken);
            if (!room1_HealthUpgradeTaken)
            {
                // the health upgrade has not been picked up, so turn it on
                room1_HealthUpgrade.SetActive(true);
            }
            else
            {
                room1_HealthUpgrade.SetActive(false);
            }

            room1_AmmoUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Ammo");
            if (!room1_AmmoUpgradeTaken)
            {
                // the health upgrade has not been picked up, so turn it on
                room1_AmmoUpgrade.SetActive(true);
            }
            else
            {
                room1_AmmoUpgrade.SetActive(false);
            }
            #endregion
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
                    GameStatus.GetInstance().SetPlayerPrefs();
                }
            }
        }
    }
}