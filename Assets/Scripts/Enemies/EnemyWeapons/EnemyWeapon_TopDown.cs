using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon_TopDown : MonoBehaviour
{
    #region Variables
    public float damage = 1;

    private float damangeMultiplier;

    [SerializeField]
    private float meleeDamage = 1f;
    [SerializeField]
    private float bulletDamage = 1f;


    public enum WeaponType { Melee, Bullet }
    public WeaponType weaponType;

    //private Collider2D col;
    #endregion

    //private void OnEnable()
    //{
    //    col = GetComponent<Collider2D>();
    //    col.enabled = true;
    //}

    private void Update()
    {
        if(weaponType == WeaponType.Melee)
        {
            damangeMultiplier = meleeDamage;
        }
        else if (weaponType == WeaponType.Bullet)
        {
            damangeMultiplier = bulletDamage;
        }
    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //   if(other.gameObject.CompareTag("Wall"))
    //    {
    //        if(weaponType == WeaponType.Bullet)
    //        {
    //            Debug.Log("shooting a wall");
    //            Destroy(gameObject);
    //        }
    //    }
    //}

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

            player.TakeDamage(damage * damangeMultiplier);
            //col.enabled = false;

            if (weaponType == WeaponType.Bullet)
            {
                Destroy(gameObject);
            }
        }

        bool isWall = collision.CompareTag("Wall");
        if (isWall)
        {
            if (weaponType == WeaponType.Bullet)
            {
                Debug.Log("shooting a wall");
                Destroy(gameObject);
            }
        }
    }
}
