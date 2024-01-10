using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TopDown : MonoBehaviour
{
    #region Varaibles
    [SerializeField]
    private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;

    [SerializeField]
    private float health = 3f;
    [SerializeField]
    private float maxHealth = 3f;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        target = GameObject.Find("Player").transform;
        health = maxHealth;
    }

    void Update()
    {
        if(target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
        }
    }

    private void FixedUpdate()
    {
        if(target)
        {
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
