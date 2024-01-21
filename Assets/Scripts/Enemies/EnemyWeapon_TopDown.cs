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

            StartCoroutine(player.TakeDamage(damage * damangeMultiplier));
            //StartCoroutine(GameStatus.GetInstance().TakeDamage(damage * damangeMultiplier));

            if (weaponType == WeaponType.Bullet)
            {
                Destroy(gameObject);
            }
        }
    }
}
