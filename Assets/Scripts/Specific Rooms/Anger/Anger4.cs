using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Anger4 : MonoBehaviour
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
    private GameObject Anger3_LoadingZone;
    [SerializeField]
    private GameObject Anger5_LoadingZone;

    // for every set of variables in this script, make sure to add the corresponding tracker to the game status
    [Header("Gates/doors/chests/breakable walls tracking")]
    [Header("Gates")]
    [SerializeField]
    private GameObject Anger4_EnemyGate;
    [SerializeField]
    private GameObject Anger4_RoomAfterGate;
    [SerializeField]
    private bool Anger4_enemyGateOpen;

    [Header("Hidden Walls")]
    [SerializeField]
    private GameObject Anger4_Hidden_Closed;
    [SerializeField]
    private GameObject Anger4_Hidden_Opened;
    [SerializeField]
    private bool Anger4_HiddenOpen;

    [Header("Permanent Upgrades")]
    [Header("Health")]
    [SerializeField]
    private GameObject Anger4_HealthUpgrade;
    [SerializeField]
    private bool Anger4_HealthUpgradeTaken;
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
        else if (previousRoom == "Anger3")   // repeat this for each transition
        {
            player.transform.position = Anger3_LoadingZone.transform.position;
        }
        else if (previousRoom == "Anger5")   // repeat this for each transition
        {
            player.transform.position = Anger5_LoadingZone.transform.position;
        }
    }

    private void Update()
    {

        currentRoom = SceneManager.GetActiveScene().name;
        if (currentRoom == "Anger4")
        {
            #region Gates
            Anger4_enemyGateOpen = GameStatus.GetInstance().GetGateState(currentRoom);
            if (!Anger4_enemyGateOpen)  // the gate is not open, so keep it closed and hide the room after
            {
                Debug.Log("closed gate");
                Anger4_EnemyGate.SetActive(true);
                Anger4_RoomAfterGate.SetActive(false);
            }
            else    // the gate is already open, so get rid of it and show the room after
            {
                Debug.Log("open gate");
                Anger4_EnemyGate.SetActive(false);
                Anger4_RoomAfterGate.SetActive(true);
            }
            #endregion

            #region Hidden Rooms
            Anger4_HiddenOpen = GameStatus.GetInstance().GetHiddenState(currentRoom);
            if (!Anger4_HiddenOpen)  // the wall is closed
            {
                Debug.Log("closed wall");
                Anger4_Hidden_Closed.SetActive(true);
                Anger4_Hidden_Opened.SetActive(false);
            }
            else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
            {
                Debug.Log("open wall");
                Anger4_Hidden_Closed.SetActive(false);
                Anger4_Hidden_Opened.SetActive(true);
            }
            #endregion

            #region Permanent Upgrades: Health
            Anger4_HealthUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Health");
            if (!Anger4_HealthUpgradeTaken)
            {
                // the health upgrade has not been picked up, so turn it on
                Anger4_HealthUpgrade.SetActive(true);
            }
            else
            {
                Anger4_HealthUpgrade.SetActive(false);
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
                    GameStatus.GetInstance().SetPlayerPrefs();
                }
            }
        }
    }
}