using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_TopDown : MonoBehaviour
{
    #region Varaibles
    public enum EnemyType { chaser, melee, ranged, mixed, charger, turret, patternWalker }
    public EnemyType enemyType;

    [SerializeField]
    private float lungeSpeed = 10f;
    [SerializeField]
    private float patternSpeed = 3.0f;

    private bool amDead;
    private bool amDamaged;

    [SerializeField]
    private EnemyHealth myHealth;

    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;

    [Header("Walk Pattern")]
    private bool isPatternWalker;

    [SerializeField]
    private Transform pointA;

    [SerializeField]
    private Transform pointB;

    private bool switching = false;
    private Transform walkPath;
    private bool amAwake;

    [Header("Attacking and Player Damage")]
    [SerializeField]
    private float contactDamage;

    public PlayerController_TopDown playerController;
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

    [SerializeField]
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
        isPatternWalker = myHealth.isPatternWalker;
    }

    void Update()
    {
        amDamaged = myHealth.isDamaged;
        amAwake = myHealth.isAwake;

        if (myHealth.isDead)
        {
            return;
        }

        CheckMeleeTimer();
        shootTimer += Time.deltaTime;

        if (amAwake && enemyType == EnemyType.charger)
        {
            ChargerAttacker();
        }

        if (amAwake && enemyType == EnemyType.melee)
        {
            MeleeAttacker();
        }

        if (amAwake && enemyType == EnemyType.ranged)
        {
            RangedAttacker();
        }

        if (amAwake && enemyType == EnemyType.mixed)
        {
            MixedAttacker();
        }

        if (amAwake && enemyType == EnemyType.turret)
        {
            TurretAttacker();
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if ((!amAwake && isPatternWalker) || enemyType == EnemyType.patternWalker && amAwake)
        {
            if (!switching)
            {
                walkPath = pointB;
            }
            else if (switching)
            {
                walkPath = pointA;
            }

            if (transform.position == pointB.position)
            {
                switching = true;
            }
            else if (transform.position == pointA.position)
            {
                switching = false;
            }
            transform.position = Vector3.MoveTowards(transform.position, walkPath.position, patternSpeed * Time.deltaTime);
        }
    }

    #region Enemy Type Specific Attack 
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
        myHealth.isWalking = true;
        return;
    }
    private void MixedAttacker()
    {
        if (attackCoolDown <= 0 && Vector3.Distance(target.position, transform.position) <= meleeAttackRange)
        {
            moveDirection = new Vector3(0f, 0f, 0f);
            OnAttack();
            return;
        }
        else if (Vector3.Distance(target.position, transform.position) >= rangedAttackRangeMin && Vector3.Distance(target.position, transform.position) <= rangedAttackRangeMax)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = -direction;
            lastMoveDirection = moveDirection;
            myHealth.isWalking = true;
            OnShoot();
            return;
        }
        else
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
            lastMoveDirection = moveDirection;
            myHealth.isWalking = true;
        }
    }

    private void TurretAttacker()
    {
        OnShoot();
        myHealth.isWalking = false;
        return;
    }
    #endregion

    #region Attacking
    private void OnAttack()
    {
        if (!isAttacking)
        {
            myHealth.moveSpeed = 0;
            Melee.SetActive(true);
            isAttacking = true;
            // call your animator to play your melee attack
        }
    }

    private void OnLunge()
    {
        if (!isAttacking)
        {
            myHealth.moveSpeed = lungeSpeed;
            isAttacking = true;
            // call your animator to play your lunge attack
        }
    }

    private void CheckMeleeTimer()
    {
        if(!isAttacking && attackCoolDown > 0)
        {
            attackCoolDown -= 1f;
            myHealth.attackCoolDown -= 1f;
        }
        if(!isAttacking && attackCoolDown <= 0)
        {
            myHealth.moveSpeed = myHealth.defaultSpeed;
        }
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDuration)
            {
                if(enemyType == EnemyType.melee)
                {
                    attackCoolDown = meleeCoolDown;
                    myHealth.attackCoolDown = meleeCoolDown;
                }
                if (enemyType == EnemyType.charger)
                {
                    attackCoolDown = chargerCoolDown;
                    myHealth.attackCoolDown = chargerCoolDown;
                }

                attackTimer = 0;
                isAttacking = false;
                myHealth.moveSpeed = myHealth.defaultSpeed;
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

