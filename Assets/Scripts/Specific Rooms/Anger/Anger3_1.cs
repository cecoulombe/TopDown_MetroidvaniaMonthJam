using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Anger3_1 : MonoBehaviour
{
    #region Variables
    private string currentRoom;
    private string previousRoom;

    [Header("Loading zones for each door")]
    [SerializeField]
    private PlayerController_TopDown player;

    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject Anger3_LoadingZone;

    // for every set of variables in this script, make sure to add the corresponding tracker to the game status
    [Header("Gates/doors/chests/breakable walls tracking")]
    [Header("Breakable Walls")]
    [SerializeField]
    private GameObject Anger3_1_Wall_Closed;
    [SerializeField]
    private GameObject Anger3_1_Wall_Opened;
    [SerializeField]
    private bool Anger3_1_WallOpen;

    [Header("Permanent Upgrades")]
    [Header("Health")]
    [SerializeField]
    private GameObject Anger3_1_HealthUpgrade;
    [SerializeField]
    private bool Anger3_1_HealthUpgradeTaken;

    [Header("Ammo")]
    [SerializeField]
    private GameObject Anger3_1_AmmoUpgrade;
    [SerializeField]
    private bool Anger3_1_AmmoUpgradeTaken;
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
        else if (previousRoom == "Anger3_1")   // repeat this for each transition
        {
            player.transform.position = Anger3_LoadingZone.transform.position;
        }
    }

    private void Update()
    {
        currentRoom = SceneManager.GetActiveScene().name;
        #region Breakable Walls
        Anger3_1_WallOpen = GameStatus.GetInstance().GetWallState(currentRoom);
        if (!Anger3_1_WallOpen)  // the wall is closed
        {
            Debug.Log("closed wall");
            Anger3_1_Wall_Closed.SetActive(true);
            Anger3_1_Wall_Opened.SetActive(false);
        }
        else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
        {
            Debug.Log("open wall");
            Anger3_1_Wall_Closed.SetActive(false);
            Anger3_1_Wall_Opened.SetActive(true);
        }
        #endregion

        // Note: permanent upgrades DO NOT work for abilities (i.e. melee)
        #region Permanent Upgrades: Health
        Anger3_1_HealthUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Health");
        if (!Anger3_1_HealthUpgradeTaken)
        {
            // the health upgrade has not been picked up, so turn it on
            Anger3_1_HealthUpgrade.SetActive(true);
        }
        else
        {
            Anger3_1_HealthUpgrade.SetActive(false);
        }
        #endregion

        #region Permanent Upgrades: Ammo
        Anger3_1_AmmoUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Ammo");
        if (!Anger3_1_AmmoUpgradeTaken)
        {
            // the health upgrade has not been picked up, so turn it on
            Anger3_1_AmmoUpgrade.SetActive(true);
        }
        else
        {
            Anger3_1_AmmoUpgrade.SetActive(false);
        }
        #endregion
    }
}