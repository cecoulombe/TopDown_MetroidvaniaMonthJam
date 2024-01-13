using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_TopDown : MonoBehaviour
{
    #region Varaibles
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private bool isAwake;

    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;

    [SerializeField]
    private float health = 3f;
    [SerializeField]
    private float maxHealth;

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

    private bool isWalking;
    #endregion

    #region Melee Variables
    [Header("Melee Variables")]
    public GameObject Melee;

    [SerializeField]
    private bool isAttacking;

    [SerializeField]
    private float timeToAttack = 0.3f;

    [SerializeField]
    private float attackDuration = 0.3f;

    [SerializeField]
    private float attackTimer = 0f;

    private float meleeAttackRange;

    [SerializeField]
    private float attackCoolDown;
    #endregion

    #region Ranged Variables
    [Header("Ranged Variables")]

    [SerializeField]
    private Transform Aim;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float fireForce = 10f;

    [SerializeField]
    private float shootCoolDown = 0.25f;

    [SerializeField]
    private float shootTimer = 0.5f;

    private float rangedAttackRangeMin;
    private float rangedAttackRangeMax;
    #endregion

    public enum EnemyType { chaser, melee, ranged, mixed}
    public EnemyType enemyType;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        target = GameObject.Find("Player").transform;
        health = maxHealth;
        rangeFromTarget = 0.7f * loseAggroRange;
        meleeAttackRange = 0.3f * rangeFromTarget;
        rangedAttackRangeMin = 0.68f * loseAggroRange;
        rangedAttackRangeMin = 0.9f * loseAggroRange;
    }

    void Update()
    {
        CheckMeleeTimer();
        shootTimer += Time.deltaTime;

        if (Vector3.Distance(target.position, transform.position) <= rangeFromTarget)
        {
            isAwake = true;
        }

        if (enemyType == EnemyType.melee || enemyType == EnemyType.mixed)
        {
            if (attackCoolDown <= 0)
            {
                if (Vector3.Distance(target.position, transform.position) <= meleeAttackRange)
                {
                    moveDirection = new Vector3(0f, 0f, 0f);
                    OnAttack();
                    return;
                }
            }
        }

        if (enemyType == EnemyType.ranged || enemyType == EnemyType.mixed)
        {
            if (Vector3.Distance(target.position, transform.position) >= rangedAttackRangeMin && Vector3.Distance(target.position, transform.position) <= rangedAttackRangeMax)
            {
                moveDirection = new Vector3(0f, 0f, 0f);
                OnShoot();
                return;
            }
        }

        if (target && isAwake && attackCoolDown <= 100f)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
            lastMoveDirection = moveDirection;
            isWalking = true;
        }

        if (Vector3.Distance(target.position, transform.position) >= loseAggroRange)
        {
            isAwake = false;
            isWalking = false;
            moveDirection = new Vector3(0f, 0f, 0f);
        }
    }

    private void FixedUpdate()
    {
        if (isWalking)
        {
            //Vector3 vector3 = Vector3.left * moveDirection.x + Vector3.down * moveDirection.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, -lastMoveDirection);
        }

        if (target)
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

    #region Attacking
    private void OnAttack()
    {
        if (!isAttacking)
        {
            Melee.SetActive(true);
            isAttacking = true;
            // call your animator to play your melee attack
        }
    }

    private void CheckMeleeTimer()
    {
        if(!isAttacking && attackCoolDown > 0)
        {
            attackCoolDown -= 1f;
        }
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDuration)
            {
                attackCoolDown = 300f;
                attackTimer = 0;
                isAttacking = false;
                Melee.SetActive(false);
            }
        }
    }

    private void OnShoot()
    {
        if (shootTimer > shootCoolDown)
        {
            shootTimer = 0;
            GameObject intBullet = Instantiate(bullet, Aim.position, Aim.rotation);
            intBullet.GetComponent<Rigidbody2D>().AddForce(-Aim.up * fireForce, ForceMode2D.Impulse);
            Destroy(intBullet, 4f);
        }
    }
    #endregion

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerController.knockBackCounter = playerController.knockBackTotalTime;
            if (collision.transform.position.x <= transform.position.x)
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

    //private void PlayerKnockback()
    //{
    //    if(playerKnockback = true)
    //    {

    //    }
    //}
}

