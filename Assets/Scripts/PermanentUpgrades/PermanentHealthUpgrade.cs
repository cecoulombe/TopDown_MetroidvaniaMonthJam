using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PermanentHealthUpgrade : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float addMaxHealth = 3;

    private string currentRoom;
    #endregion

    private void Start()
    {
        currentRoom = SceneManager.GetActiveScene().name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_TopDown player = collision.GetComponent<PlayerController_TopDown>();
        if (player != null)
        {
            GameStatus.GetInstance().AddMaxHealth(addMaxHealth);
            GameStatus.GetInstance().AddHealth(GameStatus.GetInstance().GetMaxHealth());
            //Destroy(gameObject);
            GameStatus.GetInstance().SetUpgradeState(currentRoom, "Health");
            GameStatus.GetInstance().SetPlayerPrefs();
        }
    }
}