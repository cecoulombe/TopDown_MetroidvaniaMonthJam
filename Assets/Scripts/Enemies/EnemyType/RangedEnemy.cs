using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
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

    public PlayerController_TopDown playerController;
    #endregion

    #region Ranged Variables
    [Header("Ranged Variables")]

    [SerializeField]
    private Transform Aim;

    // this needs to be an odd number, and I need to find a way to make each bullet evenly spaced around the character
    [SerializeField]
    private float numberOfBullets;

    [SerializeField]
    private float angle;

    private float subtractOffset;

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

    private Transform bulletTarget;

    private Vector2 direction;
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
        shootTimer += Time.deltaTime;

        if (amAwake)
        {
            RangedAttacker();
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
    private void RangedAttacker()
    {
        OnShoot();
        Vector3 direction = (target.position - transform.position).normalized;
        moveDirection = -direction;
        lastMoveDirection = moveDirection;
        myHealth.isWalking = true;
        return;
    }
    #endregion

    #region Attacking
    private void OnShoot()
    {
        // Shooting code here
        if (shootTimer > shootCoolDown)     // if you are not cooling down from the last shot, fire another
        {
            shootTimer = 0;

            /* 

            for each bullet: if it is the first one, fire at the target, from then on out, if it is the second shot,
            fire at the target +5f, but if it is odd, first at the same spot times -1, from then on, evens are at *-1,
            +5f so that they are always on the same side
            
             
            for now, while I am trying to figure this out, maybe set it so that it only shoots on a cardinal direction
            (i.e. up (0, 0, 0) so that I can then have it shoot relative to up, then eventually I can change it so that
            up is actually the direction of the player?)
             
             */

            // i is the number of the bullet that has been fired
            for (int bulletNumber = 0; bulletNumber < numberOfBullets; bulletNumber++)
            {
                GameObject intBullet = Instantiate(bullet, Aim.position, target.rotation);

                if (bulletNumber == 0) // this is the first bullet, so fire at the target
                {
                    direction = (target.position - intBullet.transform.position).normalized;
                }
                else if (bulletNumber == 1) // second bullet, fire to the +5
                {
                    direction = (target.position + new Vector3(5, 0, 0) - intBullet.transform.position).normalized;
                }
                else if (bulletNumber % 2 == 0) // is even
                {
                    direction = (target.position + new Vector3(-5 * (bulletNumber - 1), 0, 0) - intBullet.transform.position).normalized;
                }
                else if (bulletNumber % 2 == 1) // is odd
                {
                    direction = (target.position + new Vector3(5 * (bulletNumber - 1), 0, 0) - intBullet.transform.position).normalized;
                }

                // Instantiate the bullet
                //GameObject intBullet = Instantiate(bullet, Aim.position, target.rotation);

                // Calculate the direction towards the player
                //Vector2 direction = (target.position - intBullet.transform.position).normalized;

                // Set the bullet's velocity towards the player
                intBullet.GetComponent<Rigidbody2D>().velocity = direction * fireForce;

                Destroy(intBullet, 4f);

            }
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

