using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternWalkerEnemy : MonoBehaviour
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

