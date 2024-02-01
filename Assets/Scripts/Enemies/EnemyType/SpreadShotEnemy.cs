using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShotEnemy : MonoBehaviour
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
    private float angle;

    private float yValue;
    private float xValue;
    private float roundCounter;
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
            A new attempt at the logic: I want it to move in quadrants similar to the unit circle but doing only
            a semi circle on either size of the player. Lets assume the player is directly above the shooter, that
            is (0, 1). If they are at a 45 degree to the right, that is (1, 1), but if they are at a 45 to the left,
            that is (-1, 1). Now the smallest step I want is 15 degrees, which is (0.33, 1) for 15 to the right, or
            (-0.66, 1) for 30 degrees to the left. Because the largest a value can be is abs(1), if we want to go
            beyond 45 degrees on either side, we cannot change the x axis but instead need to change the y axis.
            This means that 75 degrees to the right would be (1, 0.33) because we have subtracted 2 * 0.33 as we
            move down (I have this all written out in front of me, it makes sense, trust me ;D). A more complicated
            example would be -120 degrees, or 150 degrees to the left, which should be (-1, -0.66). Note that one
            number must always be abs(1) but they can both be abs(1) at the same time.

            This logic allows for <= 24 shots to be fired in a perfect circle, with 15 degrees between each shot.
            Anything more than that will just lead to overlap in the bullets unless we add in a seperate condition
            for the shots to be fired at 7.5 degree increments

            Lets try this out and see how it works
             
             */
            // Check that the total shots are less than or equal to 24(if not, bring down to 24 for processing power)
            // we need an even number of bullets after the first one fires, so I will always round down if it is odd
            if (numberOfBullets > 24)
            {
                numberOfBullets = 24;
            }
            if(numberOfBullets - 1 % 2 == 0)
            {
                numberOfBullets += 1;
            }
            Debug.Log("number of bullets per wave is " + numberOfBullets);
            // break down the shot distribution: there is one up, so fire that one first
            // then there are three with the y as 1, then 3 with the x as 1, each with the other variable changing.
            // the inverse repeats when below zero until they hit 180 or straight down, which should be the 24th shot
            // for each shot to the right, I want the shot to the left to be the opposite.
            for(int bulletNumber = 1; bulletNumber <= numberOfBullets; bulletNumber++)
            {
                roundCounter = 0;
                Debug.Log("firing bullet number " + bulletNumber);
                GameObject intBullet = Instantiate(bullet, Aim.position, target.rotation);
                direction = (target.position - intBullet.transform.position).normalized;

                if (bulletNumber == 1)
                {
                    yValue = 1;
                    xValue = 0f;
                    Debug.Log("product " + (direction.x * xValue) + " xValue " + xValue + " direction.x " + direction.x);
                }

                //if (bulletNumber == 2)
                //{
                //    yValue = 1;
                //    xValue = 1/3f;
                //    Debug.Log("product " + (direction.x * xValue) + " xValue " + xValue + " direction.x " + direction.x);
                //}

                //if (bulletNumber == 3)
                //{
                //    yValue = 1;
                //    xValue = -1/3f;
                //    Debug.Log("product " + (direction.x * xValue) + " xValue " + xValue + " direction.x " + direction.x);
                //}

                if(bulletNumber % 2 == 0)
                {
                    yValue = 1;
                    xValue = (1 / 3f) * roundCounter;
                    roundCounter += 1;
                    Debug.Log("product " + (direction.x * xValue) + " xValue " + xValue + " direction.x " + direction.x);
                }

                if(bulletNumber % 2 != 0)
                {
                    yValue = 1;
                    xValue = (-1 / 3f) * roundCounter;
                    Debug.Log("product " + (direction.x * xValue) + " xValue " + xValue + " direction.x " + direction.x);
                }

                angle = Mathf.Atan2(direction.y + (direction.y * yValue), direction.x + (direction.x * xValue)) * Mathf.Rad2Deg;
                Debug.Log("angle for bullet number " + bulletNumber + " is " + angle);
                intBullet.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
                intBullet.GetComponent<Rigidbody2D>().velocity = direction * fireForce;


                Destroy(intBullet, 4f);

            }







            /* 
            if I am adding 5f to the target for each shot, then the angle will depend on how close the target it,
            but if not, then I can use trig to determine how many shots it would take before the bullets are at
            horizontal, in which case I would want to have them start having a negative y value otherwise they will
            cluster at the horizontals, which is not ideal
                */

            // i is the number of the bullet that has been fired
            //for (int bulletNumber = 0; bulletNumber < numberOfBullets; bulletNumber++)
            //{
            //    GameObject intBullet = Instantiate(bullet, Aim.position, target.rotation);

            //    if (bulletNumber == 0) // this is the first bullet, so fire at the target
            //    {
            //        direction = (target.position - intBullet.transform.position).normalized;
            //        //angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //    }
            //    else if (bulletNumber == 1) // second bullet, fire to the +5
            //    {
            //        direction = (target.position + new Vector3(5, 0, 0) - intBullet.transform.position).normalized;
            //    }
            //    else if(bulletNumber % 2 == 0) // is even
            //    {
            //        direction = (target.position + new Vector3(-5 * (bulletNumber - 1), 0, 0) - intBullet.transform.position).normalized;
            //    }
            //    else if(bulletNumber % 2 == 1) // is odd
            //    {
            //        direction = (target.position + new Vector3(5 * (bulletNumber - 1), 0, 0) - intBullet.transform.position).normalized;
            //    }

            //    // Set the bullet's velocity towards the player
            //    intBullet.GetComponent<Rigidbody2D>().velocity = direction * fireForce;

            //    // Optionally, you can set the rotation of the bullet based on the direction
            //    angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            //    intBullet.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);


            //    Destroy(intBullet, 4f);

            //}
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

