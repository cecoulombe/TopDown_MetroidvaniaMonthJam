using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentAmmoUpgrade : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float addMaxAmmo = 3;

    #endregion

    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_TopDown player = collision.GetComponent<PlayerController_TopDown>();
        if (player != null)
        {
            GameStatus.GetInstance().AddMaxAmmo(addMaxAmmo);
            GameStatus.GetInstance().AddAmmo(GameStatus.GetInstance().GetMaxAmmo());
            Destroy(gameObject);
        }
    }
}