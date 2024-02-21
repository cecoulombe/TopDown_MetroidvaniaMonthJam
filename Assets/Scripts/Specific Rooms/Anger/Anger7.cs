using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Anger7 : MonoBehaviour
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
    private GameObject Anger6_LoadingZone;
    [SerializeField]
    private GameObject Anger8_LoadingZone;
    [SerializeField]
    private GameObject Words1_LoadingZone;

    // for every set of variables in this script, make sure to add the corresponding tracker to the game status
    //[Header("Gates/doors/chests/breakable walls tracking")]

    //[Header("Ammo")]
    //[SerializeField]
    //private GameObject Anger7_AmmoUpgrade;
    //[SerializeField]
    //private bool Anger7_AmmoUpgradeTaken;
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
        else if (previousRoom == "Anger6")   // repeat this for each transition
        {
            player.transform.position = Anger6_LoadingZone.transform.position;
        }
        else if (previousRoom == "Anger8")   // repeat this for each transition
        {
            player.transform.position = Anger8_LoadingZone.transform.position;
        }
        else if (previousRoom == "Words1")   // repeat this for each transition
        {
            player.transform.position = Words1_LoadingZone.transform.position;
        }
    }

    //private void Update()
    //{
    //    currentRoom = SceneManager.GetActiveScene().name;

    //    #region Permanent Upgrades: Ammo
    //    Anger7_AmmoUpgradeTaken = GameStatus.GetInstance().GetUpgradeState(currentRoom, "Ammo");
    //    if (!Anger7_AmmoUpgradeTaken)
    //    {
    //        // the health upgrade has not been picked up, so turn it on
    //        Anger7_AmmoUpgrade.SetActive(true);
    //    }
    //    else
    //    {
    //        Anger7_AmmoUpgrade.SetActive(false);
    //    }
    //    #endregion
    //}
}