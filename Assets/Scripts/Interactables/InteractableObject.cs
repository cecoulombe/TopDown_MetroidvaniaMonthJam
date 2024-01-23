using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableObject : MonoBehaviour
{
    // have a function for each type of interactable object (i.e. door, chest)

    private string currentRoom;
    private bool isOpen;

    private void Update()
    {
        currentRoom = SceneManager.GetActiveScene().name;
    }

    #region Chest
    public void CheckChest()
    {
        // chest if the chest we are trying to interact with is open or closed
        // if closed, open it up

        isOpen = GameStatus.GetInstance().GetChestState(currentRoom);
        if (!isOpen)
        {
            OpenChest();
        }
        else
        {
            Debug.Log("chest is already open, nothing to do");
        }
    }

    private void OpenChest()
    {
        // run the code to open the chest and give the player whatever the contents were
        Debug.Log("opening the chest and giving the reward");
        GameStatus.GetInstance().SetChestState(currentRoom);
    }
    #endregion

    #region Door
    public void CheckDoor()
    {
        // chest if the chest we are trying to interact with is open or closed
        // if closed, open it up

        isOpen = GameStatus.GetInstance().GetChestState(currentRoom);
        if (!isOpen)
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("door is already open, nothing to do");
        }
    }

    private void OpenDoor()
    {
        // run the code to open the chest and give the player whatever the contents were
        Debug.Log("opening the door");
        GameStatus.GetInstance().SetChestState(currentRoom);
    }
    #endregion
}
