using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Room2 : MonoBehaviour
{
    // room after the enemy gate, holds a chest with currency in it and nothing else (no enemies or anything else)

    #region Variables
    private string currentRoom;

    [Header("Gates/doors/chests tracking")]
    [SerializeField]
    private GameObject Room2_Chest_Closed;
    [SerializeField]
    private GameObject Room2_Chest_Opened;
    [SerializeField]
    private bool Room2_ChestOpen;
    #endregion

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
    }
}
