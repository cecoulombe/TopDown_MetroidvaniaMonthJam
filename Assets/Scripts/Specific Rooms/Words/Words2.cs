using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Words2 : MonoBehaviour
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
    private GameObject Words1_LoadingZone;
    [SerializeField]
    private GameObject Words3_LoadingZone;
    [SerializeField]
    private GameObject Anger6_1_LoadingZone;
    [SerializeField]
    private GameObject Words7_LoadingZone;

    // for every set of variables in this script, make sure to add the corresponding tracker to the game status
    [Header("Gates/doors/chests/breakable walls tracking")]
    [Header("Gates")]
    [SerializeField]
    private GameObject Words2_EnemyGate;
    [SerializeField]
    private GameObject Words2_RoomAfterGate;
    [SerializeField]
    private bool Words2_enemyGateOpen;

    [Header("Breakable Walls")]
    [SerializeField]
    private GameObject Words2_Wall_Closed;
    [SerializeField]
    private GameObject Words2_Wall_Opened;
    [SerializeField]
    private bool Words2_WallOpen;

    [Header("Hidden Walls")]
    [SerializeField]
    private GameObject Words2_Hidden_Closed;
    [SerializeField]
    private GameObject Words2_Hidden_Opened;
    [SerializeField]
    private bool Words2_HiddenOpen;

    [Header("Permanent Upgrades")]
    [Header("Health")]
    [SerializeField]
    private GameObject Words2_HealthUpgrade;
    [SerializeField]
    private bool Words2_HealthUpgradeTaken;
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
        else if (previousRoom == "Words1")   // repeat this for each transition
        {
            player.transform.position = Words1_LoadingZone.transform.position;
        }
        else if (previousRoom == "Words3")   // repeat this for each transition
        {
            player.transform.position = Words3_LoadingZone.transform.position;
        }
        else if (previousRoom == "Anger6_1")   // repeat this for each transition
        {
            player.transform.position = Anger6_1_LoadingZone.transform.position;
        }
        else if (previousRoom == "Words7")   // repeat this for each transition
        {
            player.transform.position = Words7_LoadingZone.transform.position;
        }
    }

    private void Update()
    {

        currentRoom = SceneManager.GetActiveScene().name;
        #region Gates
        Words2_enemyGateOpen = GameStatus.GetInstance().GetGateState(currentRoom);
        if (!Words2_enemyGateOpen)  // the gate is not open, so keep it closed and hide the room after
        {
            Debug.Log("closed gate");
            Words2_EnemyGate.SetActive(true);
            Words2_RoomAfterGate.SetActive(false);
        }
        else    // the gate is already open, so get rid of it and show the room after
        {
            Debug.Log("open gate");
            Words2_EnemyGate.SetActive(false);
            Words2_RoomAfterGate.SetActive(true);
        }
        #endregion

        #region Breakable Walls
        Words2_WallOpen = GameStatus.GetInstance().GetWallState(currentRoom);
        if (!Words2_WallOpen)  // the wall is closed
        {
            Debug.Log("closed wall");
            Words2_Wall_Closed.SetActive(true);
            Words2_Wall_Opened.SetActive(false);
        }
        else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
        {
            Debug.Log("open wall");
            Words2_Wall_Closed.SetActive(false);
            Words2_Wall_Opened.SetActive(true);
        }
        #endregion

        #region Hidden Rooms
        Words2_HiddenOpen = GameStatus.GetInstance().GetHiddenState(currentRoom);
        if (!Words2_HiddenOpen)  // the wall is closed
        {
            Debug.Log("closed wall");
            Words2_Hidden_Closed.SetActive(true);
            Words2_Hidden_Opened.SetActive(false);
        }
        else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
        {
            Debug.Log("open wall");
            Words2_Hidden_Closed.SetActive(false);
            Words2_Hidden_Opened.SetActive(true);
        }
        #endregion

        // Note: permanent upgrades DO NOT work for abilities (i.e. melee)
        #region Permanent Upgrades: Health
        Words2_HealthUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Health");
        if (!Words2_HealthUpgradeTaken)
        {
            // the health upgrade has not been picked up, so turn it on
            Words2_HealthUpgrade.SetActive(true);
        }
        else
        {
            Words2_HealthUpgrade.SetActive(false);
        }
        #endregion

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
                    GameStatus.GetInstance().SetPlayerPrefs();
                }
            }
        }
    }
}