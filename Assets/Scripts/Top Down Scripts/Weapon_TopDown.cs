using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_TopDown : MonoBehaviour
{
    #region Variables
    public float damage = 1;

    public enum WeaponType {Melee, Bullet}
    public WeaponType weaponType;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy_TopDown enemy = collision.GetComponent<Enemy_TopDown>();
        if(enemy != null)
        {
            enemy.knockBackCounter = enemy.knockBackTotalTime;
            if (collision.transform.position.x <= transform.position.x)
            {
                enemy.knockFromRight = true;
            }
            if (collision.transform.position.x >= transform.position.x)
            {
                enemy.knockFromRight = false;
            }

            enemy.TakeDamage(damage);

            if(weaponType == WeaponType.Bullet)
            {
                Destroy(gameObject);
            }
        }
    }
}
