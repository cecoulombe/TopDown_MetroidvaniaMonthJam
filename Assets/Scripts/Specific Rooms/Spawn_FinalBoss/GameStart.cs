using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
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
    private GameObject Anger1_LoadingZone;
    [SerializeField]
    private GameObject Overwhelm1_LoadingZone;

    // for every set of variables in this script, make sure to add the corresponding tracker to the game status
    [Header("Gates/doors/chests/breakable walls tracking")]
    [Header("Breakable Walls")]
    [SerializeField]
    private GameObject GameStart_Wall_Closed;
    [SerializeField]
    private GameObject GameStart_Wall_Opened;
    [SerializeField]
    private bool GameStart_WallOpen;

    [Header("Hidden Walls")]
    [SerializeField]
    private GameObject GameStart_Overwhelm1_Hidden_Closed;
    [SerializeField]
    private GameObject GameStart_Overwhelm1_Hidden_Opened;
    [SerializeField]
    private bool GameStart_Overwhelm1_HiddenOpen;
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
        else if (previousRoom == "Anger1")   // repeat this for each transition
        {
            player.transform.position = Anger1_LoadingZone.transform.position;
        }
        else if (previousRoom == "Overwhelm1")   // repeat this for each transition
        {
            player.transform.position = Overwhelm1_LoadingZone.transform.position;
        }
    }

    private void Update()
    {

        currentRoom = SceneManager.GetActiveScene().name;
        if (currentRoom == "GameStart")
        {
            #region Breakable Walls
            GameStart_WallOpen = GameStatus.GetInstance().GetWallState(currentRoom);
            if (!GameStart_WallOpen)  // the wall is closed
            {
                Debug.Log("closed wall");
                GameStart_Wall_Closed.SetActive(true);
                GameStart_Wall_Opened.SetActive(false);
            }
            else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
            {
                Debug.Log("open wall");
                GameStart_Wall_Closed.SetActive(false);
                GameStart_Wall_Opened.SetActive(true);
            }
            #endregion

            #region Hidden Rooms
            GameStart_Overwhelm1_HiddenOpen = GameStatus.GetInstance().GetHiddenState(currentRoom);
            if (!GameStart_Overwhelm1_HiddenOpen)  // the wall is closed
            {
                Debug.Log("closed wall");
                GameStart_Overwhelm1_Hidden_Closed.SetActive(true);
                GameStart_Overwhelm1_Hidden_Opened.SetActive(false);
            }
            else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
            {
                Debug.Log("open wall");
                GameStart_Overwhelm1_Hidden_Closed.SetActive(false);
                GameStart_Overwhelm1_Hidden_Opened.SetActive(true);
            }
            #endregion
        }
    }
}