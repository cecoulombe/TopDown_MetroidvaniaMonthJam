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
        Debug.Log("hitting something");
        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
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

        BreakableWall wall = collision.GetComponent<BreakableWall>();
        if (wall != null)
        {
            Debug.Log("hitting the wall");
            wall.TakeDamage(damage);

            if (weaponType == WeaponType.Bullet)
            {
                Destroy(gameObject);
            }
        }
    }
}
