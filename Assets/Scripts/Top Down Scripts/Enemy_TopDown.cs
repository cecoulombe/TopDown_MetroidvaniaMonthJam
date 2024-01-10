using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TopDown : MonoBehaviour
{
    #region Varaibles
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private bool isAwake;

    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;

    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float rangeFromTarget;

    [SerializeField]
    private float loseAggroRange;

    [SerializeField]
    private float contactDamage;

    [SerializeField]
    private float knockbackForce;
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
        if(Vector3.Distance(target.position, transform.position) <= rangeFromTarget)
        {
            isAwake = true;
        }

        if (target && isAwake)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
        }

        if(Vector3.Distance(target.position, transform.position) >= loseAggroRange)
        {
            isAwake = false;
            moveDirection = new Vector3(0f, 0f, 0f);
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

    #region Contact Damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        IDamagable damagable = collider.GetComponent<IDamagable>();

        if(damagable != null)
        {
            Vector2 direction = (collider.transform.position - transform.position).normalized;

            Vector2 knockback = direction * knockbackForce;

            damagable.OnHit(contactDamage, knockback);
        }
    }
    #endregion
}

