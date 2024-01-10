using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_TopDown : MonoBehaviour
{
    #region Variables

    public float damage = 1;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy_TopDown enemy = collision.GetComponent<Enemy_TopDown>();
        if(enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
