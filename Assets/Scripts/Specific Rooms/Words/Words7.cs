using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Words7 : MonoBehaviour
{
    #region Variables
    private string currentRoom;
    private string previousRoom;

    [Header("Loading zones for each door")]
    private PlayerController_TopDown player;

    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject Words5_LoadingZone;
    [SerializeField]
    private GameObject Words8_LoadingZone;
    [SerializeField]
    private GameObject WordsX_LoadingZone;    
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
        else if (previousRoom == "Words5")   // repeat this for each transition
        {
            player.transform.position = Words5_LoadingZone.transform.position;
        }
        else if (previousRoom == "Words8")   // repeat this for each transition
        {
            player.transform.position = Words8_LoadingZone.transform.position;
        }
        else if (previousRoom == "WordsX")   // repeat this for each transition
        {
            player.transform.position = WordsX_LoadingZone.transform.position;
        }
    }
}