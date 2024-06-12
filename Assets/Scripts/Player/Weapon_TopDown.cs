using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_TopDown : MonoBehaviour
{
    #region Variables
    public float damage = 1;

    public enum WeaponType {Melee, Bullet}
    public WeaponType weaponType;

    //private Collider2D col;

    #endregion

    //private void Start()
    //{
    //    col = GetComponent<Collider2D>();
    //    col.enabled = true;
    //}

    //private void OnEnable()
    //{
    //    col = GetComponent<Collider2D>();
    //    col.enabled = true;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("hitting something");
        EnemyHealth_Manager enemy = collision.GetComponent<EnemyHealth_Manager>();
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

        OWBoss_Health OWBoss = collision.GetComponent<OWBoss_Health>();
        if (OWBoss != null)
        {
            OWBoss.TakeDamage(damage);

            if (weaponType == WeaponType.Bullet)
            {
                Destroy(gameObject);
            }
        }

        AngerBoss_Health AngerBoss = collision.GetComponent<AngerBoss_Health>();
        if (AngerBoss != null)
        {
            AngerBoss.TakeDamage(damage);

            if (weaponType == WeaponType.Bullet)
            {
                Destroy(gameObject);
            }
        }

        SSBoss_Health SticksStones = collision.GetComponent<SSBoss_Health>();
        if (SticksStones != null)
        {
            SticksStones.TakeDamage(damage);

            //col.enabled = false;

            if (weaponType == WeaponType.Bullet)
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
