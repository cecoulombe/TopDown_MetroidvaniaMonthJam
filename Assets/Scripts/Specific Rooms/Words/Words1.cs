using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Words1 : MonoBehaviour
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
    private GameObject Anger7_LoadingZone;
    [SerializeField]
    private GameObject Words2_LoadingZone;

    // for every set of variables in this script, make sure to add the corresponding tracker to the game status
    [Header("Gates/doors/chests/breakable walls tracking")]
    [Header("Breakable Walls")]
    [SerializeField]
    private GameObject Words1_Wall_Closed;
    [SerializeField]
    private GameObject Words1_Wall_Opened;
    [SerializeField]
    private bool Words1_WallOpen;
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
        else if (previousRoom == "Anger7")   // repeat this for each transition
        {
            player.transform.position = Anger7_LoadingZone.transform.position;
        }
        else if (previousRoom == "Words2")   // repeat this for each transition
        {
            player.transform.position = Words2_LoadingZone.transform.position;
        }
    }

    private void Update()
    {

        currentRoom = SceneManager.GetActiveScene().name;
        #region Breakable Walls
        Words1_WallOpen = GameStatus.GetInstance().GetWallState(currentRoom);
        if (!Words1_WallOpen)  // the wall is closed
        {
            Debug.Log("closed wall");
            Words1_Wall_Closed.SetActive(true);
            Words1_Wall_Opened.SetActive(false);
        }
        else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
        {
            Debug.Log("open wall");
            Words1_Wall_Closed.SetActive(false);
            Words1_Wall_Opened.SetActive(true);
        }
        #endregion
    }
}