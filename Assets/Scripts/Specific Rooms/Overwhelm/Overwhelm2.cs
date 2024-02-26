using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Overwhelm2 : MonoBehaviour
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
    private GameObject Overwhelm1_LoadingZone;
    [SerializeField]
    private GameObject Overwhelm3_LoadingZone;
    [SerializeField]
    private GameObject OverwhelmUnknown_LoadingZone;

    // for every set of variables in this script, make sure to add the corresponding tracker to the game status
    [Header("Gates/doors/chests/breakable walls tracking")]
    [Header("Gates")]
    [SerializeField]
    private GameObject Overwhelm2_EnemyGate;
    [SerializeField]
    private GameObject Overwhelm2_RoomAfterGate;
    [SerializeField]
    private bool Overwhelm2_enemyGateOpen;

    [Header("Breakable Walls")]
    [SerializeField]
    private GameObject Overwhelm2_Wall_Closed;
    [SerializeField]
    private GameObject Overwhelm2_Wall_Opened;
    [SerializeField]
    private bool Overwhelm2_WallOpen;

    [Header("Permanent Upgrades")]
    [Header("Ammo")]
    [SerializeField]
    private GameObject Overwhelm2_AmmoUpgrade;
    [SerializeField]
    private bool Overwhelm2_AmmoUpgradeTaken;
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
        else if (previousRoom == "Overwhelm1")   // repeat this for each transition
        {
            player.transform.position = Overwhelm1_LoadingZone.transform.position;
        }
        else if (previousRoom == "Overwhelm3")   // repeat this for each transition
        {
            player.transform.position = Overwhelm3_LoadingZone.transform.position;
        }
        else if (previousRoom == "OverwhelmUnknown")   // repeat this for each transition
        {
            player.transform.position = OverwhelmUnknown_LoadingZone.transform.position;
        }
    }

    private void Update()
    {

        currentRoom = SceneManager.GetActiveScene().name;
        #region Gates
        Overwhelm2_enemyGateOpen = GameStatus.GetInstance().GetGateState(currentRoom);
        if (!Overwhelm2_enemyGateOpen)  // the gate is not open, so keep it closed and hide the room after
        {
            Debug.Log("closed gate");
            Overwhelm2_EnemyGate.SetActive(true);
            Overwhelm2_RoomAfterGate.SetActive(false);
        }
        else    // the gate is already open, so get rid of it and show the room after
        {
            Debug.Log("open gate");
            Overwhelm2_EnemyGate.SetActive(false);
            Overwhelm2_RoomAfterGate.SetActive(true);
        }
        #endregion

        #region Breakable Walls
        Overwhelm2_WallOpen = GameStatus.GetInstance().GetWallState(currentRoom);
        if (!Overwhelm2_WallOpen)  // the wall is closed
        {
            Debug.Log("closed wall");
            Overwhelm2_Wall_Closed.SetActive(true);
            Overwhelm2_Wall_Opened.SetActive(false);
        }
        else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
        {
            Debug.Log("open wall");
            Overwhelm2_Wall_Closed.SetActive(false);
            Overwhelm2_Wall_Opened.SetActive(true);
        }
        #endregion

        #region Permanent Upgrades: Ammo
        Overwhelm2_AmmoUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Ammo");
        if (!Overwhelm2_AmmoUpgradeTaken)
        {
            // the health upgrade has not been picked up, so turn it on
            Overwhelm2_AmmoUpgrade.SetActive(true);
        }
        else
        {
            Overwhelm2_AmmoUpgrade.SetActive(false);
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