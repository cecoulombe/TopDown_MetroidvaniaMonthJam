using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    #region Varaibles
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

    private float attackCoolDown;

    [SerializeField]
    private float meleeCoolDown;
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

        if (amAwake)
        {
            MeleeAttacker();
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (!amAwake && isPatternWalker)
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

    private void CheckMeleeTimer()
    {
        if (!isAttacking && attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
            myHealth.attackCoolDown -= Time.deltaTime;
        }
        if (!isAttacking && attackCoolDown <= 0)
        {
            myHealth.moveSpeed = myHealth.defaultSpeed;
        }
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDuration)
            {
                attackCoolDown = meleeCoolDown;
                myHealth.attackCoolDown = meleeCoolDown;

                attackTimer = 0;
                isAttacking = false;
                myHealth.moveSpeed = myHealth.defaultSpeed;
                Melee.SetActive(false);
            }
            return;
        }
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController_TopDown>();

            collision.gameObject.GetComponent<PlayerController_TopDown>().knockBackCounter = collision.gameObject.GetComponent<PlayerController_TopDown>().knockBackTotalTime;
            if (collision.transform.position.x <= transform.position.x)
            {
                collision.gameObject.GetComponent<PlayerController_TopDown>().knockFromRight = true;
            }
            if (collision.transform.position.x >= transform.position.x)
            {
                collision.gameObject.GetComponent<PlayerController_TopDown>().knockFromRight = false;
            }
            //StartCoroutine(playerController.TakeDamage(contactDamage));
            collision.gameObject.GetComponent<PlayerController_TopDown>().TakeDamage(contactDamage);
        }
    }
}

