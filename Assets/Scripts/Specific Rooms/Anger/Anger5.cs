using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Anger5 : MonoBehaviour
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
    private GameObject Anger4_LoadingZone;
    [SerializeField]
    private GameObject Anger6_LoadingZone;
    [SerializeField]
    private GameObject AngerUnknown_LoadingZone;

    // for every set of variables in this script, make sure to add the corresponding tracker to the game status
    [Header("Gates/doors/chests/breakable walls tracking")]
    [Header("Gates")]
    [SerializeField]
    private GameObject Anger5_EnemyGate;
    [SerializeField]
    private GameObject Anger5_RoomAfterGate;
    [SerializeField]
    private bool Anger5_enemyGateOpen;

    [Header("Permanent Upgrades")]
    [Header("Ammo")]
    [SerializeField]
    private GameObject Anger5_AmmoUpgrade;
    [SerializeField]
    private bool Anger5_AmmoUpgradeTaken;
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
        else if (previousRoom == "Anger4")   // repeat this for each transition
        {
            player.transform.position = Anger4_LoadingZone.transform.position;
        }
        else if (previousRoom == "Anger6")   // repeat this for each transition
        {
            player.transform.position = Anger6_LoadingZone.transform.position;
        }
        else if (previousRoom == "AngerUnknown")   // repeat this for each transition
        {
            player.transform.position = AngerUnknown_LoadingZone.transform.position;
        }
    }

    private void Update()
    {

        currentRoom = SceneManager.GetActiveScene().name;
        if (currentRoom == "Anger5")
        {
            #region Gates
            Anger5_enemyGateOpen = GameStatus.GetInstance().GetGateState(currentRoom);
            if (!Anger5_enemyGateOpen)  // the gate is not open, so keep it closed and hide the room after
            {
                Debug.Log("closed gate");
                Anger5_EnemyGate.SetActive(true);
                Anger5_RoomAfterGate.SetActive(false);
            }
            else    // the gate is already open, so get rid of it and show the room after
            {
                Debug.Log("open gate");
                Anger5_EnemyGate.SetActive(false);
                Anger5_RoomAfterGate.SetActive(true);
            }
            #endregion

            #region Permanent Upgrades: Ammo
            Anger5_AmmoUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Ammo");
            if (!Anger5_AmmoUpgradeTaken)
            {
                // the health upgrade has not been picked up, so turn it on
                Anger5_AmmoUpgrade.SetActive(true);
            }
            else
            {
                Anger5_AmmoUpgrade.SetActive(false);
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