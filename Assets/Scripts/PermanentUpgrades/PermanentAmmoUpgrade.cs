using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PermanentAmmoUpgrade : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float addMaxAmmo = 3;

    private string currentRoom;

    [SerializeField]
    private GameObject ammoPopup;
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
            GameStatus.GetInstance().AddMaxAmmo(addMaxAmmo);
            GameStatus.GetInstance().AddAmmo(GameStatus.GetInstance().GetMaxAmmo());
            //Destroy(gameObject);
            GameStatus.GetInstance().SetUpgradeState(currentRoom, "Ammo");
            GameStatus.GetInstance().SetPlayerPrefs();
            ammoPopup.SetActive(true);

        }
    }
}