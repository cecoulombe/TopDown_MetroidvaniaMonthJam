using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Room2 : MonoBehaviour
{
    // room after the enemy gate, holds a chest with money and refills in it as well as a permanent health and ammo upgrade

    #region Variables
    private string currentRoom;
    private string previousRoom;

    [Header("Loading zones for each door")]
    [SerializeField]
    private PlayerController_TopDown player;

    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject room1_LoadingZone;


    [Header("Gates/doors/chests tracking")]
    [SerializeField]
    private GameObject Room2_Chest_Closed;
    [SerializeField]
    private GameObject Room2_Chest_Opened;
    [SerializeField]
    private bool Room2_ChestOpen;

    [SerializeField]
    private GameObject room2_HealthUpgrade;
    [SerializeField]
    private bool room2_HealthUpgradeTaken;

    [SerializeField]
    private GameObject room2_AmmoUpgrade;
    [SerializeField]
    private bool room2_AmmoUpgradeTaken;
    #endregion

    private void Start()
    {
        // I want to know the room the player came from so that I can load them in at the right spot
        previousRoom = GameStatus.GetInstance().GetPreviousRoom();

        // I think I want to set the players load position here? Before any thing else can happen in the room? Lets give that a try
        if (previousRoom == "spawn")
        {
            // this will be used if the player loads into this room specifically, mainly for testing
            player.transform.position = spawnPoint.transform.position;
        }
        else if (previousRoom == "Room1")
        {
            // room 1 is a bottom transition, so set the position of the player to the position of the room1 loading zone which is a game object in the scene?
            player.transform.position = room1_LoadingZone.transform.position;
        }
    }

    private void Update()
    {
        currentRoom = SceneManager.GetActiveScene().name;
        if (currentRoom == "Room2")     
        {
            //use this to check any/all chests and everything else in the room
            Room2_ChestOpen = GameStatus.GetInstance().GetChestState(currentRoom);
            if (!Room2_ChestOpen)  // the chest is closed
            {
                Debug.Log("closed chest");
                Room2_Chest_Closed.SetActive(true);
                Room2_Chest_Opened.SetActive(false);
            }
            else    // open the chest and keep it visually opened after (it will stay open after leaving the room)
            {
                Debug.Log("open chest");
                Room2_Chest_Closed.SetActive(false);
                Room2_Chest_Opened.SetActive(true);
            }


            room2_HealthUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Health");
            if(!room2_HealthUpgradeTaken)
            {
                // the health upgrade has not been picked up, so turn it on
                room2_HealthUpgrade.SetActive(true);
            }
            else
            {
                room2_HealthUpgrade.SetActive(false);
            }

            room2_AmmoUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Ammo");
            if (!room2_AmmoUpgradeTaken)
            {
                // the health upgrade has not been picked up, so turn it on
                room2_AmmoUpgrade.SetActive(true);
            }
            else
            {
                room2_AmmoUpgrade.SetActive(false);
            }
        }
    }
}
