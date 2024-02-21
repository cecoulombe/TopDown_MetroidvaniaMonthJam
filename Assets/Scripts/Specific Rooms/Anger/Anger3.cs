using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Anger3 : MonoBehaviour
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
    private GameObject Anger2_LoadingZone;
    [SerializeField]
    private GameObject Anger4_LoadingZone;
    [SerializeField]
    private GameObject Anger3_1_LoadingZone;

    [Header("Breakable Walls")]
    [SerializeField]
    private GameObject Anger3_Wall_Closed;
    [SerializeField]
    private GameObject Anger3_Wall_Opened;
    [SerializeField]
    private bool Anger3_WallOpen;
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
        else if (previousRoom == "Anger2")   // repeat this for each transition
        {
            player.transform.position = Anger2_LoadingZone.transform.position;
        }
        else if (previousRoom == "Anger4")   // repeat this for each transition
        {
            player.transform.position = Anger4_LoadingZone.transform.position;
        }
        else if (previousRoom == "Anger3_1")   // repeat this for each transition
        {
            player.transform.position = Anger3_1_LoadingZone.transform.position;
        }
    }

    private void Update()
    {
        currentRoom = SceneManager.GetActiveScene().name;
        Debug.Log("from the room manager: " + currentRoom);
        #region Breakable Walls
        Anger3_WallOpen = GameStatus.GetInstance().GetWallState(currentRoom);
        if (!Anger3_WallOpen)  // the wall is closed
        {
            Debug.Log("closed wall");
            Anger3_Wall_Closed.SetActive(true);
            Anger3_Wall_Opened.SetActive(false);
        }
        else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
        {
            Debug.Log("open wall");
            Anger3_Wall_Closed.SetActive(false);
            Anger3_Wall_Opened.SetActive(true);
        }
        #endregion
    }
}