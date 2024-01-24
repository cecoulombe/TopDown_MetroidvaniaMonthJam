using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    #region Variables
    private int ammo;

    [SerializeField]
    private int smallAmmo = 1;
    [SerializeField]
    private int bigAmmo = 2;

    public enum HealthType { Small, Big }
    public HealthType healthType;
    #endregion

    private void Update()
    {
        if (healthType == HealthType.Small)
        {
            ammo = smallAmmo;
        }
        else if (healthType == HealthType.Big)
        {
            ammo = bigAmmo;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_TopDown player = collision.GetComponent<PlayerController_TopDown>();
        if (player != null)
        {
            GameStatus.GetInstance().AddAmmo(ammo);
            Destroy(gameObject);
        }
    }
}
