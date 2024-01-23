using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Room2 : MonoBehaviour
{
    // room after the enemy gate, holds a chest with currency in it and nothing else (no enemies or anything else)

    #region Variables
    private string currentRoom;

    private bool closeEnoughToChest;

    [Header("Gates/doors/chests tracking")]
    [SerializeField]
    private GameObject Room2_Chest_Closed;
    [SerializeField]
    private GameObject Room2_Chest_Opened;
    [SerializeField]
    private bool Room2_ChestOpen;
    #endregion

    private void Start()
    {
        closeEnoughToChest = false;
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
        }

        OpenChest();
    }
    // might want an on trigger, so if the player enters a certain radius and presses the interact button, they will open the chest and get the reward?
    //private void OnTriggerEnter2D(Collider2D Player)
    //{
    //    if (Player.CompareTag("Player"))
    //    {
    //        Debug.Log("player entered the space to open the chest");
    //        closeEnoughToChest = true;
    //    }
    //    else
    //    {
    //        Debug.Log("something else entered the collision");
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player is touching the chest");
            closeEnoughToChest = true;
        }
    }

    private void OpenChest()
    {
        if(closeEnoughToChest && !Room2_ChestOpen && Input.GetKeyDown(KeyCode.Return))
        {
            GameStatus.GetInstance().SetChestState(currentRoom);
            Debug.Log("Opening the chest in room 2 and giving the rewards");
        }
    }
}
