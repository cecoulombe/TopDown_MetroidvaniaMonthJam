using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerEmeny : MonoBehaviour
{
    #region Varaibles
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

    #region Charge Variables
    [Header("Charge Variables")]

    [SerializeField]
    private bool isAttacking;

    [SerializeField]
    private float attackDuration = 0.3f;

    [SerializeField]
    private float attackTimer = 0f;

    [SerializeField]
    private float lungeAttackRange;

    private float attackCoolDown;

    [SerializeField]
    private float chargerCoolDown = 500f;
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
            ChargerAttacker();
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
    #endregion

    #region Attacking
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
        if (!isAttacking && attackCoolDown > 0)
        {
            attackCoolDown -= 1f;
            myHealth.attackCoolDown -= 1f;
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
                attackCoolDown = chargerCoolDown;
                myHealth.attackCoolDown = chargerCoolDown;

                attackTimer = 0;
                isAttacking = false;
                myHealth.moveSpeed = myHealth.defaultSpeed;
            }
            return;
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

