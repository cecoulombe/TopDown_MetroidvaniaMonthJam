using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentHealthUpgrade : MonoBehaviour
{
    #region Variables
    private float health;

    [SerializeField]
    private float addMaxHealth = 3;

    #endregion

    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_TopDown player = collision.GetComponent<PlayerController_TopDown>();
        if (player != null)
        {
            GameStatus.GetInstance().AddMaxHealth(addMaxHealth);
            GameStatus.GetInstance().AddHealth(GameStatus.GetInstance().GetMaxHealth());
            Destroy(gameObject);
        }
    }
}