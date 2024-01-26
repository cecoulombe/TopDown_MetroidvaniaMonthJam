using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_TopDown : MonoBehaviour
{
    #region Varaibles
    public enum EnemyType { chaser, melee, ranged, mixed, charger }
    public EnemyType enemyType;

    [Header("Movement Variables")]
    [SerializeField]
    private float moveSpeed;
    private float defaultSpeed;
    [SerializeField]
    private float lungeSpeed = 10f;

    [SerializeField]
    public bool isDead;

    [SerializeField]
    private bool isAwake;

    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;

    private float rangeFromTarget;

    [SerializeField]
    private float loseAggroRange;

    private bool isWalking;

    [Header("Enemy Health")]
    [SerializeField]
    private float health = 3f;
    [SerializeField]
    private float maxHealth;

    [Header("Attacking and Player Damage")]
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


    [Header("Health and Ammo Drops")]
    [SerializeField]
    private GameObject bigHealthDrop;
    [SerializeField]
    private float bigHealthChance;
    [SerializeField]
    private GameObject smallHealthDrop;
    [SerializeField]
    private float smallHealthChance;

    [SerializeField]
    private GameObject bigAmmoDrop;
    [SerializeField]
    private float bigAmmoChance;
    [SerializeField]
    private GameObject smallAmmoDrop;
    [SerializeField]
    private float smallAmmoChance;
    #endregion

    #region Melee Variables
    [Header("Melee Variables")]
    public GameObject Melee;

    [SerializeField]
    private bool isAttacking;

    [SerializeField]
    private float attackDuration = 0.3f;

    [SerializeField]
    private float attackTimer = 0f;

    private float meleeAttackRange;
    [SerializeField]
    private float lungeAttackRange;

    private float attackCoolDown;
    [SerializeField]
    private float meleeCoolDown = 200f;
    [SerializeField]
    private float chargerCoolDown = 500f;
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
    private float lungePercent;
    [SerializeField]
    private float minRangePercent;
    [SerializeField]
    private float maxRangePercent;
    #endregion

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        target = GameObject.Find("Player").transform;
        health = maxHealth;
        defaultSpeed = moveSpeed;
        rangeFromTarget = wakeUpPercent * loseAggroRange;
        meleeAttackRange = 1f;
        lungeAttackRange = lungePercent * loseAggroRange;
        rangedAttackRangeMin = minRangePercent * loseAggroRange;
        rangedAttackRangeMax = maxRangePercent * loseAggroRange;
    }

    void Update()
    {
        if (GameStatus.GetInstance().GetHealth() <= 0)
        {
            //moveDirection = new Vector3(0f, 0f, 0f);
            moveSpeed = 0;
            return;
        }

        if (isDead)
        {
            return;
        }

        CheckMeleeTimer();
        shootTimer += Time.deltaTime;

        if (Vector3.Distance(target.position, transform.position) <= rangeFromTarget || health != maxHealth)
        {
            StartCoroutine(DelayBeforeMoving());
            isAwake = true;
        }

        if (isAwake && enemyType == EnemyType.charger)
        {
            ChargerAttacker();
        }

        if (isAwake && enemyType == EnemyType.melee)
        {
            MeleeAttacker();
        }

        if (isAwake && enemyType == EnemyType.ranged)
        {
            RangedAttacker();
        }

        if (isAwake && enemyType == EnemyType.mixed)
        {
            MixedAttacker();
        }

        if (target && isAwake && attackCoolDown <= 100f && (enemyType != EnemyType.ranged || enemyType != EnemyType.mixed))
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
            lastMoveDirection = moveDirection;
            isWalking = true;
        }

        if (Vector3.Distance(target.position, transform.position) >= loseAggroRange && health == maxHealth)
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


    #region Enemy Type Attack Specifics
    private void ChargerAttacker()
    {
        if (attackCoolDown <= 0)
        {
            if (Vector3.Distance(target.position, transform.position) <= lungeAttackRange)
            {
                OnLunge();
                Debug.Log("lunging at player");
                return;
            }
        }
    }
    private void MeleeAttacker()
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
    private void RangedAttacker()
    {
        OnShoot();
        Vector3 direction = (target.position - transform.position).normalized;
        moveDirection = -direction;
        lastMoveDirection = moveDirection;
        isWalking = true;
        return;
    }
    private void MixedAttacker()
    {
        if (attackCoolDown <= 0 && Vector3.Distance(target.position, transform.position) <= meleeAttackRange)
        {
            //if (Vector3.Distance(target.position, transform.position) <= meleeAttackRange)
            //{
            moveDirection = new Vector3(0f, 0f, 0f);
            OnAttack();
            return;
            //}
        }
        else if (Vector3.Distance(target.position, transform.position) >= rangedAttackRangeMin && Vector3.Distance(target.position, transform.position) <= rangedAttackRangeMax)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = -direction;
            lastMoveDirection = moveDirection;
            isWalking = true;
            OnShoot();
            return;
        }
        else
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
            lastMoveDirection = moveDirection;
            isWalking = true;
        }
    }
    #endregion

    private IEnumerator DelayBeforeMoving()
    {
        yield return new WaitForSeconds(0.1f);
    }

    #region Attacking
    private void OnAttack()
    {
        if (!isAttacking)
        {
            moveSpeed = 0;
            Melee.SetActive(true);
            isAttacking = true;
            // call your animator to play your melee attack
        }
        //moveSpeed = defaultSpeed;
    }

    private void OnLunge()
    {
        if (!isAttacking)
        {
            moveSpeed = lungeSpeed;
            isAttacking = true;
            // call your animator to play your lunge attack
        }
        //moveSpeed = defaultSpeed;
    }

    private void CheckMeleeTimer()
    {
        if(!isAttacking && attackCoolDown > 0)
        {
            attackCoolDown -= 1f;
        }
        if(!isAttacking && attackCoolDown <= 0)
        {
            moveSpeed = defaultSpeed;
        }
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDuration)
            {
                if(enemyType == EnemyType.melee)
                {
                    attackCoolDown = meleeCoolDown;
                }
                if (enemyType == EnemyType.charger)
                {
                    attackCoolDown = chargerCoolDown;
                }

                attackTimer = 0;
                isAttacking = false;
                moveSpeed = defaultSpeed;
                Melee.SetActive(false);
            }
            return;
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

            // only drop health if the player is not full (similar to Super metroid)
            if(GameStatus.GetInstance().GetHealth() != GameStatus.GetInstance().GetMaxHealth())
            {
                if (randNum <= bigHealthChance)
                {
                    Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-1f, 1f), Aim.position.y + Random.Range(-1f, 1f), Aim.position.z);
                    Instantiate(bigHealthDrop, dropPos, Aim.rotation);
                }
                else if (randNum > bigHealthChance && randNum <= smallHealthChance)
                {
                    Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-1f, 1f), Aim.position.y + Random.Range(-1f, 1f), Aim.position.z);
                    Instantiate(smallHealthDrop, dropPos, Aim.rotation);
                }
            }
            // make sure the player can shoot before you give bullets, and make sure that they actually need bullets (similar to the health)
            if (GameStatus.GetInstance().HasRanged())    
            {
                if (GameStatus.GetInstance().GetAmmo() != GameStatus.GetInstance().GetMaxAmmo())
                {
                    float randNumAmmo = Random.Range(0f, 10f) / 10f * 100f;
                    if (randNumAmmo <= bigAmmoChance)
                    {
                        Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-1f, 1f), Aim.position.y + Random.Range(-1f, 1f), Aim.position.z);
                        Instantiate(bigAmmoDrop, dropPos, Aim.rotation);
                    }
                    else if (randNumAmmo > bigAmmoChance && randNumAmmo <= smallAmmoChance)
                    {
                        Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-1f, 1f), Aim.position.y + Random.Range(-1f, 1f), Aim.position.z);
                        Instantiate(smallAmmoDrop, dropPos, Aim.rotation);
                    }
                }
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
            //StartCoroutine(playerController.TakeDamage(contactDamage));
            playerController.TakeDamage(contactDamage);
        }
    }
}

