using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Words5 : MonoBehaviour
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
    private GameObject Words4_LoadingZone;
    [SerializeField]
    private GameObject Words6_LoadingZone;
    [SerializeField]
    private GameObject Words7_LoadingZone;

    // for every set of variables in this script, make sure to add the corresponding tracker to the game status
    [Header("Gates/doors/chests/breakable walls tracking")]
    [Header("Gates")]
    [SerializeField]
    private GameObject Words5_EnemyGate;
    [SerializeField]
    private GameObject Words5_RoomAfterGate;
    [SerializeField]
    private bool Words5_enemyGateOpen;

    [Header("Breakable Walls")]
    [SerializeField]
    private GameObject Words5_Wall_Closed;
    [SerializeField]
    private GameObject Words5_Wall_Opened;
    [SerializeField]
    private bool Words5_WallOpen;

    [Header("Permanent Upgrades")]
    [Header("Health")]
    [SerializeField]
    private GameObject Words5_HealthUpgrade;
    [SerializeField]
    private bool Words5_HealthUpgradeTaken;

    private bool sticksDead;

    private SSBoss_Health sticks;
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
        else if (previousRoom == "Words4")   // repeat this for each transition
        {
            player.transform.position = Words4_LoadingZone.transform.position;
        }
        else if (previousRoom == "Words6")   // repeat this for each transition
        {
            player.transform.position = Words6_LoadingZone.transform.position;
        }
        else if (previousRoom == "Words7")   // repeat this for each transition
        {
            player.transform.position = Words7_LoadingZone.transform.position;
        }
        sticks = FindObjectOfType<SSBoss_Health>();
    }

    private void Update()
    {
        sticksDead = sticks.isDead;

        currentRoom = SceneManager.GetActiveScene().name;
        #region Gates
        Words5_enemyGateOpen = GameStatus.GetInstance().GetGateState(currentRoom);
        if (!Words5_enemyGateOpen)  // the gate is not open, so keep it closed and hide the room after
        {
            Debug.Log("closed gate");
            Words5_EnemyGate.SetActive(true);
            Words5_RoomAfterGate.SetActive(false);
        }
        else    // the gate is already open, so get rid of it and show the room after
        {
            Debug.Log("open gate");
            Words5_EnemyGate.SetActive(false);
            Words5_RoomAfterGate.SetActive(true);
        }
        #endregion

        #region Breakable Walls
        Words5_WallOpen = GameStatus.GetInstance().GetWallState(currentRoom);
        if (!Words5_WallOpen)  // the wall is closed
        {
            Debug.Log("closed wall");
            Words5_Wall_Closed.SetActive(true);
            Words5_Wall_Opened.SetActive(false);
        }
        else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
        {
            Debug.Log("open wall");
            Words5_Wall_Closed.SetActive(false);
            Words5_Wall_Opened.SetActive(true);
        }
        #endregion

        // Note: permanent upgrades DO NOT work for abilities (i.e. melee)
        #region Permanent Upgrades: Health
        Words5_HealthUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Health");
        if (!Words5_HealthUpgradeTaken)
        {
            // the health upgrade has not been picked up, so turn it on
            Words5_HealthUpgrade.SetActive(true);
        }
        else
        {
            Words5_HealthUpgrade.SetActive(false);
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
                Debug.Log(enemyDeathCounter);
                if (enemyDeathCounter == enemyList.Length && sticksDead)
                {
                    Debug.Log("all enemies dead, opening the gate");
                    GameStatus.GetInstance().SetGateState(currentRoom);
                    GameStatus.GetInstance().SetPlayerPrefs();
                }
            }
        }
    }
}