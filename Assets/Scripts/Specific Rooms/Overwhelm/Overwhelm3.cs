using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Overwhelm3 : MonoBehaviour
{
    #region Variables
    public EnemyHealth[] enemyList;

    private float enemyDeathCounter;

    private string currentRoom;
    private string previousRoom;

    [Header("Loading zones for each door")]
    private PlayerController_TopDown player;

    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject Overwhelm2_LoadingZone;
    [SerializeField]
    private GameObject Overwhelm4_LoadingZone;
    [SerializeField]
    private GameObject Overwhelm5_LoadingZone;



    // for every set of variables in this script, make sure to add the corresponding tracker to the game status
    [Header("Gates/doors/chests/breakable walls tracking")]
    [Header("Gates")]
    [SerializeField]
    private GameObject Overwhelm3_EnemyGate;
    [SerializeField]
    private GameObject Overwhelm3_RoomAfterGate;
    [SerializeField]
    private bool Overwhelm3_enemyGateOpen;

    [Header("Hidden Walls")]
    [SerializeField]
    private GameObject Overwhelm3_and4_Hidden_Closed;
    [SerializeField]
    private GameObject Overwhelm3_and4_Hidden_Opened;
    [SerializeField]
    private bool Overwhelm3_and4_HiddenOpen;

    [Header("Permanent Upgrades")]
    [Header("Ranged")]
    [SerializeField]
    private bool hasRanged;
    #endregion

    private void Start()
    {
        player = FindObjectOfType<PlayerController_TopDown>();

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
        else if (previousRoom == "Overwhelm2")   // repeat this for each transition
        {
            player.transform.position = Overwhelm2_LoadingZone.transform.position;
        }
        else if (previousRoom == "Overwhelm4")   // repeat this for each transition
        {
            player.transform.position = Overwhelm4_LoadingZone.transform.position;
        }
        else if (previousRoom == "Overwhelm4")   // repeat this for each transition
        {
            player.transform.position = Overwhelm5_LoadingZone.transform.position;
        }
    }

    private void Update()
    {
        currentRoom = SceneManager.GetActiveScene().name;

        #region Hidden Rooms
        Overwhelm3_and4_HiddenOpen = GameStatus.GetInstance().GetHiddenState(currentRoom);
        if (!Overwhelm3_and4_HiddenOpen)  // the wall is closed
        {
            Debug.Log("closed wall");
            Overwhelm3_and4_Hidden_Closed.SetActive(true);
            Overwhelm3_and4_Hidden_Opened.SetActive(false);
        }
        else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
        {
            Debug.Log("open wall");
            Overwhelm3_and4_Hidden_Closed.SetActive(false);
            Overwhelm3_and4_Hidden_Opened.SetActive(true);
        }
        #endregion
        #region Trigger enemy gates after picking up melee
        hasRanged = GameStatus.GetInstance().HasRanged();
        if (hasRanged)
        {
            Debug.Log("has ranged");
            #region Gates
            Overwhelm3_enemyGateOpen = GameStatus.GetInstance().GetGateState(currentRoom);
            if (!Overwhelm3_enemyGateOpen)  // the gate is not open, so keep it closed and hide the room after
            {
                Debug.Log("closed gate");
                Overwhelm3_EnemyGate.SetActive(true);
                Overwhelm3_RoomAfterGate.SetActive(false);
            }
            else    // the gate is already open, so get rid of it and show the room after
            {
                Debug.Log("open gate");
                Overwhelm3_EnemyGate.SetActive(false);
                Overwhelm3_RoomAfterGate.SetActive(true);
            }
            #endregion
        }
        else
        {
            Debug.Log("does not have ranged");
            Overwhelm3_enemyGateOpen = false;
            Overwhelm3_EnemyGate.SetActive(false);
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