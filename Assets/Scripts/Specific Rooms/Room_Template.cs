using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room_Template : MonoBehaviour
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

    [Header("Gates/doors/chests/breakable walls tracking")]
    [Header("Gates")]
    [SerializeField]
    private GameObject RoomNumber_EnemyGate;
    [SerializeField]
    private GameObject RoomNumer_RoomAfterGate;
    [SerializeField]
    private bool roomNumber_enemyGateOpen;

    [Header("Chests")]
    [SerializeField]
    private GameObject RoomNumer_Chest_Closed;
    [SerializeField]
    private GameObject RoomNumber_Chest_Opened;
    [SerializeField]
    private bool RoomNumber_ChestOpen;

    [Header("Permanent Upgrades")]
    [Header("Health")]
    [SerializeField]
    private GameObject roomNumber_HealthUpgrade;
    [SerializeField]
    private bool roomNumber_HealthUpgradeTaken;

    [Header("Ammo")]
    [SerializeField]
    private GameObject roomNumber_AmmoUpgrade;
    [SerializeField]
    private bool roomNumber_AmmoUpgradeTaken;
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
        else if (previousRoom == "Room2")   // repeat this for each transition
        {
            player.transform.position = room2_LoadingZone.transform.position;
        }
    }

    private void Update()
    {

        currentRoom = SceneManager.GetActiveScene().name;
        if (currentRoom == "room name")
        {
            #region Gates
            roomNumber_enemyGateOpen = GameStatus.GetInstance().GetGateState(currentRoom);
            if (!roomNumber_enemyGateOpen)  // the gate is not open, so keep it closed and hide the room after
            {
                Debug.Log("closed gate");
                RoomNumber_EnemyGate.SetActive(true);
                RoomNumer_RoomAfterGate.SetActive(false);
            }
            else    // the gate is already open, so get rid of it and show the room after
            {
                Debug.Log("open gate");
                RoomNumber_EnemyGate.SetActive(false);
                RoomNumer_RoomAfterGate.SetActive(true);
            }
            #endregion

            #region Chests
            RoomNumber_ChestOpen = GameStatus.GetInstance().GetChestState(currentRoom);
            if (!RoomNumber_ChestOpen)  // the chest is closed
            {
                Debug.Log("closed chest");
                RoomNumer_Chest_Closed.SetActive(true);
                RoomNumber_Chest_Opened.SetActive(false);
            }
            else    // open the chest and keep it visually opened after (it will stay open after leaving the room)
            {
                Debug.Log("open chest");
                RoomNumer_Chest_Closed.SetActive(false);
                RoomNumber_Chest_Opened.SetActive(true);
            }
            #endregion

            #region Permanent Upgrades: Health
            roomNumber_HealthUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Health");
            if (!roomNumber_HealthUpgradeTaken)
            {
                // the health upgrade has not been picked up, so turn it on
                roomNumber_HealthUpgrade.SetActive(true);
            }
            else
            {
                roomNumber_HealthUpgrade.SetActive(false);
            }
            #endregion

            #region Permanent Upgrades: Ammo
            roomNumber_AmmoUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Ammo");
            if (!roomNumber_AmmoUpgradeTaken)
            {
                // the health upgrade has not been picked up, so turn it on
                roomNumber_AmmoUpgrade.SetActive(true);
            }
            else
            {
                roomNumber_AmmoUpgrade.SetActive(false);
            }
            #endregion
        }

        // if there is an enemy gate, you need this method
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