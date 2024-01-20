using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_TopDown : MonoBehaviour
{
    #region Varaibles
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    public bool isDead;

    [SerializeField]
    private bool isAwake;

    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;

    private Vector3 startPosition;

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

    [SerializeField]
    private GameObject bigHealthDrop;
    [SerializeField]
    private float bigHealthChance;
    [SerializeField]
    private GameObject smallHealthDrop;
    [SerializeField]
    private float smallHealthChance;
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

    [SerializeField]
    private float wakeUpPercent;
    [SerializeField]
    private float meleePercent;
    [SerializeField]
    private float minRangePercent;
    [SerializeField]
    private float maxRangePercent;
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
        rangeFromTarget = wakeUpPercent * loseAggroRange;
        meleeAttackRange = meleePercent * rangeFromTarget;
        rangedAttackRangeMin = minRangePercent * loseAggroRange;
        rangedAttackRangeMax = maxRangePercent * loseAggroRange;

        startPosition = gameObject.transform.position;
    }

    void Update()
    {
        if (GameStatus.GetInstance().GetHealth() <= 0)
        {
            return;
        }

        if (isDead)
        {
            return;
        }

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

        if (isAwake && enemyType == EnemyType.ranged)
        {
            OnShoot();
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = -direction;
            lastMoveDirection = moveDirection;
            isWalking = true;
            return;
        }

        if (isAwake && enemyType == EnemyType.mixed)
        {
            if (Vector3.Distance(target.position, transform.position) >= rangedAttackRangeMin && Vector3.Distance(target.position, transform.position) <= rangedAttackRangeMax)
            {
                OnShoot();
                Vector3 direction = (target.position - transform.position).normalized;
                moveDirection = -direction;
                lastMoveDirection = moveDirection;
                isWalking = true;
                return;
            }

        }

        if (target && isAwake && attackCoolDown <= 100f && (enemyType != EnemyType.ranged))
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

            // Instantiate the bullet
            GameObject intBullet = Instantiate(bullet, Aim.position, target.rotation);

            // Calculate the direction towards the player
            Vector2 direction = (target.position - intBullet.transform.position).normalized;

            // Set the bullet's velocity towards the player
            intBullet.GetComponent<Rigidbody2D>().velocity = direction * fireForce;

            // Optionally, you can set the rotation of the bullet based on the direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            intBullet.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

            Destroy(intBullet, 4f);
        }
    }
    #endregion

    public void TakeDamage(float damage)
    {
        health -= damage;
        isAwake = true;
        if (health <= 0)
        {
            //Destroy(gameObject);
            isDead = true;
            gameObject.SetActive(false);
            float randNum = Random.Range(0f, 10f) / 10f * 100f;
            if(randNum <= bigHealthChance)
            {
                Instantiate(bigHealthDrop, Aim.position, Aim.rotation);
            }
            else if (randNum > bigHealthChance && randNum <= smallHealthChance)
            {
                Instantiate(smallHealthDrop, Aim.position, Aim.rotation);
            }
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
            StartCoroutine(playerController.TakeDamage(contactDamage));
        }
    }
}

