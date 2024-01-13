using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon_TopDown : MonoBehaviour
{
    #region Variables
    public float damage = 1;

    public enum WeaponType { Melee, Bullet }
    public WeaponType weaponType;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_TopDown player = collision.GetComponent<PlayerController_TopDown>();
        if (player != null)
        {
            player.knockBackCounter = player.knockBackTotalTime;
            if (collision.transform.position.x <= transform.position.x)
            {
                player.knockFromRight = true;
            }
            if (collision.transform.position.x >= transform.position.x)
            {
                player.knockFromRight = false;
            }

            player.TakeDamage(damage);

            if (weaponType == WeaponType.Bullet)
            {
                Destroy(gameObject);
            }
        }
    }
}
