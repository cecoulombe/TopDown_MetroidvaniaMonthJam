using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RangedAbility : MonoBehaviour
{
    // similar to all other pick ups, this will check if the ability has already been picked up, and if so, it will not spawn the object, but if it hasn't it will spawn the object and when collided with, it will tell the game status to give the player that ability
    #region Variables
    private string pickup = "ranged";

    [SerializeField]
    private bool hasPickup;

    private string currentRoom;
    #endregion

    private void Update()
    {
        currentRoom = SceneManager.GetActiveScene().name;
        hasPickup = GameStatus.GetInstance().GetUpgradeState(currentRoom, pickup);
        if (hasPickup)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_TopDown player = collision.GetComponent<PlayerController_TopDown>();
        if (player != null && !hasPickup)
        {
            GameStatus.GetInstance().SetUpgradeState(currentRoom, pickup);
        }
    }
}