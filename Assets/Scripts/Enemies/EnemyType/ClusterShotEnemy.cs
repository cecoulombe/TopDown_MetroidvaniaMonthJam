using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterShotEnemy : MonoBehaviour
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
        //if (shootTimer > shootCoolDown)
        //{
        //    shootTimer = 0;

        //    // Instantiate the bullet
        //    GameObject intBullet = Instantiate(bullet, Aim.position, target.rotation);

        //    // Calculate the direction towards the player
        //    Vector2 direction = (target.position - intBullet.transform.position).normalized;

        //    // Set the bullet's velocity towards the player
        //    intBullet.GetComponent<Rigidbody2D>().velocity = direction * fireForce;

        //    // Optionally, you can set the rotation of the bullet based on the direction
        //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //    intBullet.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        //    Destroy(intBullet, 4f);
        //}

        // Checks condition and assign subtractOffset a value based on bulletCount
        // need to know the number of shots available, then for each extra bullet, have it go either to the left or the right of the player, then if there are still more, have them go to the side again
        switch (numberOfBullets)
        {
            case 3:
                Debug.Log("3 bullets");
                subtractOffset = 1f;
                break;
            case 4:
                Debug.Log("4 bullets");
                subtractOffset = 1.5f;
                break;
            case 5:
                Debug.Log("5 bullets");
                subtractOffset = 2f;
                break;
        }

        // Shooting code here
        if (shootTimer > shootCoolDown)
        {
            shootTimer = 0;

            // Shoots bullets with calculation: ((i - subtractOffset) * angle)
            // 1st bullet's rotation is ((0 - 1) * 10) = -10, 2nd is 0, 3rd is 10
            for (int i = 0; i < numberOfBullets; i++)
            {
                CreateBullet(new Vector3(0f, 0f, ((i - subtractOffset) * angle)));
            }
        }
    }


    void CreateBullet(Vector3 offsetRotation)
    {
        GameObject intBullet = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
        Vector2 direction = (target.position - intBullet.transform.position).normalized;
        intBullet.GetComponent<Rigidbody2D>().velocity = direction * fireForce;
        intBullet.transform.Rotate(offsetRotation);
        Destroy(intBullet, 4f);
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

