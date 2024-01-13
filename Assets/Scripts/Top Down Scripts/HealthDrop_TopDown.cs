using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop_TopDown : MonoBehaviour
{
    #region Variables
    public float health = 1;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_TopDown player = collision.GetComponent<PlayerController_TopDown>();
        if (player != null)
        {
            player.Heal(health);
            Destroy(gameObject);
        }
    }
}
