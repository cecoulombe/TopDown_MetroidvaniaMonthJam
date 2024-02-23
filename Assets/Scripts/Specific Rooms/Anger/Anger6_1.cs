using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Anger6_1 : MonoBehaviour
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
    private GameObject Words2_LoadingZone;

    // for every set of variables in this script, make sure to add the corresponding tracker to the game status
    [Header("Gates/doors/chests/breakable walls tracking")]
    [Header("Gates")]
    [SerializeField]
    private GameObject Anger6_1_EnemyGate;
    [SerializeField]
    private GameObject Anger6_1_RoomAfterGate;
    [SerializeField]
    private bool Anger6_1_enemyGateOpen;

    [Header("Permanent Upgrades")]
    [Header("Ammo")]
    [SerializeField]
    private GameObject Anger6_1_AmmoUpgrade;
    [SerializeField]
    private bool Anger6_1_AmmoUpgradeTaken;
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
        else if (previousRoom == "Words2")   // repeat this for each transition
        {
            player.transform.position = Words2_LoadingZone.transform.position;
        }
    }

    private void Update()
    {

        currentRoom = SceneManager.GetActiveScene().name;
        #region Gates
        Anger6_1_enemyGateOpen = GameStatus.GetInstance().GetGateState(currentRoom);
        if (!Anger6_1_enemyGateOpen)  // the gate is not open, so keep it closed and hide the room after
        {
            Debug.Log("closed gate");
            Anger6_1_EnemyGate.SetActive(true);
            Anger6_1_RoomAfterGate.SetActive(false);
        }
        else    // the gate is already open, so get rid of it and show the room after
        {
            Debug.Log("open gate");
            Anger6_1_EnemyGate.SetActive(false);
            Anger6_1_RoomAfterGate.SetActive(true);
        }
        #endregion

        #region Permanent Upgrades: Ammo
        Anger6_1_AmmoUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Ammo");
        if (!Anger6_1_AmmoUpgradeTaken)
        {
            // the health upgrade has not been picked up, so turn it on
            Anger6_1_AmmoUpgrade.SetActive(true);
        }
        else
        {
            Anger6_1_AmmoUpgrade.SetActive(false);
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