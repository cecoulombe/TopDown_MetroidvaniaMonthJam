using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Anger2 : MonoBehaviour
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
    private GameObject Anger3_LoadingZone;
    [SerializeField]
    private GameObject Anger6_LoadingZone;

    // for every set of variables in this script, make sure to add the corresponding tracker to the game status
    [Header("Gates/doors/chests/breakable walls tracking")]
    [Header("Hidden Walls")]
    [SerializeField]
    private GameObject Anger2_Hidden_Closed;
    [SerializeField]
    private GameObject Anger2_Hidden_Opened;
    [SerializeField]
    private bool Anger2_HiddenOpen;
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
        else if (previousRoom == "Anger3")   // repeat this for each transition
        {
            player.transform.position = Anger3_LoadingZone.transform.position;
        }
        else if (previousRoom == "Anger6")   // repeat this for each transition
        {
            player.transform.position = Anger6_LoadingZone.transform.position;
        }
    }

    private void Update()
    {

        currentRoom = SceneManager.GetActiveScene().name;
        if (currentRoom == "Anger2")
        {
            #region Hidden Rooms
            Anger2_HiddenOpen = GameStatus.GetInstance().GetHiddenState(currentRoom);
            if (!Anger2_HiddenOpen)  // the wall is closed
            {
                Debug.Log("closed wall");
                Anger2_Hidden_Closed.SetActive(true);
                Anger2_Hidden_Opened.SetActive(false);
            }
            else    // open the wall and keep it visually opened after (it will stay open after leaving the room)
            {
                Debug.Log("open wall");
                Anger2_Hidden_Closed.SetActive(false);
                Anger2_Hidden_Opened.SetActive(true);
            }
            #endregion
        }
    }
}