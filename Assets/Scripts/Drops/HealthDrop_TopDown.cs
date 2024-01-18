using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop_TopDown : MonoBehaviour
{
    #region Variables
    private float health;

    [SerializeField]
    private float smallHealth = 1;
    [SerializeField]
    private float bigHealth = 2f;

    public enum HealthType { Small, Big }
    public HealthType healthType;
    #endregion

    private void Update()
    {
        if (healthType == HealthType.Small)
        {
            health = smallHealth;
        }
        else if (healthType == HealthType.Big)
        {
            health = bigHealth;
        }
    }

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
