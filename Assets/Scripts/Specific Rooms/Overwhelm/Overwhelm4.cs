using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Overwhelm4 : MonoBehaviour
{
    #region Variables
    private string currentRoom;
    private string previousRoom;

    [Header("Loading zones for each door")]
    private PlayerController_TopDown player;

    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject words6_LoadingZone;
    [SerializeField]
    private GameObject overwhelm3_LoadingZone;

    // for every set of variables in this script, make sure to add the corresponding tracker to the game status
    [Header("Gates/doors/chests/breakable walls tracking")]

    [Header("Hidden Walls")]
    [SerializeField]
    private GameObject Overwhelm3_4_Hidden_Closed;
    [SerializeField]
    private GameObject Overwhelm3_4_Hidden_Opened;
    [SerializeField]
    private bool Overwhelm3_4_HiddenOpen;

    [Header("Permanent Upgrades")]
    [Header("Health")]
    [SerializeField]
    private GameObject Overwhelm4_HealthUpgrade;
    [SerializeField]
    private bool Overwhelm4_HealthUpgradeTaken;

    [Header("Ammo")]
    [SerializeField]
    private GameObject Overwhelm4_AmmoUpgrade;
    [SerializeField]
    private bool Overwhelm4_AmmoUpgradeTaken;
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
        else if (previousRoom == "Words6")   // repeat this for each transition
        {
            player.transform.position = words6_LoadingZone.transform.position;
        }
        else if (previousRoom == "Overwhelm3")   // repeat this for each transition
        {
            player.transform.position = overwhelm3_LoadingZone.transform.position;
        }
    }

    private void Update()
    {

        currentRoom = SceneManager.GetActiveScene().name;
        #region Hidden Rooms
        Overwhelm3_4_HiddenOpen = GameStatus.GetInstance().GetHiddenState(currentRoom);
        if (!Overwhelm3_4_HiddenOpen)  // the wall is closed
        {
            Debug.Log("closed wall");
            Overwhelm3_4_Hidden_Closed.SetActive(true);
            Overwhelm3_4_Hidden_Opened.SetActive(false);
        }
        else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
        {
            Debug.Log("open wall");
            Overwhelm3_4_Hidden_Closed.SetActive(false);
            Overwhelm3_4_Hidden_Opened.SetActive(true);
        }
        #endregion

        // Note: permanent upgrades DO NOT work for abilities (i.e. melee)
        #region Permanent Upgrades: Health
        Overwhelm4_HealthUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Health");
        if (!Overwhelm4_HealthUpgradeTaken)
        {
            // the health upgrade has not been picked up, so turn it on
            Overwhelm4_HealthUpgrade.SetActive(true);
        }
        else
        {
            Overwhelm4_HealthUpgrade.SetActive(false);
        }
        #endregion

        #region Permanent Upgrades: Ammo
        Overwhelm4_AmmoUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Ammo");
        if (!Overwhelm4_AmmoUpgradeTaken)
        {
            // the health upgrade has not been picked up, so turn it on
            Overwhelm4_AmmoUpgrade.SetActive(true);
        }
        else
        {
            Overwhelm4_AmmoUpgrade.SetActive(false);
        }
        #endregion
    }
}