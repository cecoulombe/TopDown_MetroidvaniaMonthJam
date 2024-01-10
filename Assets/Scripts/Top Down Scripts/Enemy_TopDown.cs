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

    public PlayerController_TopDown playerController;
    //public PlayerHealth_TopDown playerHealth;

    [SerializeField]
    public float knockBackForce;

    [SerializeField]
    public float knockBackCounter;

    public float knockBackTotalTime;

    [SerializeField]
    public bool knockFromRight;
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
            if (knockBackCounter <= 0)  // not being knocked back so you can move
            {
                rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
            }
            else    // will need to update this to be from any direction, so make it knockback force equal to the opposite direction of the enemy
            {
                if (knockFromRight)
                {
                    rb.velocity = new Vector2(-knockBackForce, 0f);
                }
                if (!knockFromRight)
                {
                    rb.velocity = new Vector2(knockBackForce, 0f);
                }

                knockBackCounter -= Time.deltaTime;
            }

            
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerController.knockBackCounter = playerController.knockBackTotalTime;
            if(collision.transform.position.x <= transform.position.x)
            {
                playerController.knockFromRight = true;
            }
            if (collision.transform.position.x >= transform.position.x)
            {
                playerController.knockFromRight = false;
            }
            playerController.TakeDamage(contactDamage);
        }
    }
}

