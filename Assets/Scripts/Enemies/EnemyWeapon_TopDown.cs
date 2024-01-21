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
    #endregion


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

            //StartCoroutine(player.TakeDamage(damage * damangeMultiplier));
            player.TakeDamage(damage * damangeMultiplier);
            if (weaponType == WeaponType.Melee)
            {
                // turn of the weapon after it hits the player so that it doesn't stack attack them
                //this.gameObject.SetActive(false);
                this.enabled = false;
            }

            if (weaponType == WeaponType.Bullet)
            {
                Destroy(gameObject);
            }
        }
    }
}
