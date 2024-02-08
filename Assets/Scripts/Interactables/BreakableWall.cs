using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    #region Varaibles
    [SerializeField]
    public bool isDead;

    private Rigidbody2D rb;

    [Header("Wall Health")]
    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth = 1f;
    #endregion



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (isDead)
        {
            //Debug.Log("wall is broken");
            return;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //Destroy(gameObject);
            isDead = true;
            gameObject.SetActive(false);
        }
    }
}

