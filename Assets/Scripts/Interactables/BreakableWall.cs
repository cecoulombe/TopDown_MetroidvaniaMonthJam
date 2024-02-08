using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    #region Varaibles
    [SerializeField]
    public bool isDead;

    [Header("Wall Health")]
    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth = 1f;
    #endregion


    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("taking damage");
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("wall is broken");
            //Destroy(gameObject);
            isDead = true;
            GameStatus.GetInstance().SetWallState(GameStatus.GetInstance().GetCurrentRoom());
        }
    }
}

