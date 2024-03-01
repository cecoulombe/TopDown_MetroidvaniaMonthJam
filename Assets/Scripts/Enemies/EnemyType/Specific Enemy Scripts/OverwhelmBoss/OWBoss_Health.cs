using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWBoss_Health : MonoBehaviour
{
    #region Varaibles
    [SerializeField]
    public bool isDead;

    public bool isAwake;

    public bool isDamaged;

    private Rigidbody2D rb;
    [SerializeField]
    private Transform Aim;

    private SpriteRenderer sprite;

    [Header("Movement Variables")]
    [SerializeField]
    public float moveSpeed;
    [SerializeField]
    public float defaultSpeed;

    private Transform target;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;

    public float attackCoolDown;

    private float wakeUpRange;

    [SerializeField]
    public float loseAggroRange;

    public bool isWalking;

    public bool inAwakeRange;
    public bool inLoseAggroRange;

    [SerializeField]
    private float wakeUpPercent;

    [Header("Enemy Health")]
    [SerializeField]
    public float health;
    [SerializeField]
    public float maxHealth;

    public bool takingDamage;

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

    [Header("Knockback Variables")]
    [SerializeField]
    public float knockBackForce;

    public float knockBackCounter;

    public float knockBackTotalTime;

    public bool knockFromRight;

    public float iFrames;

    [SerializeField]
    private float defaultIFrames = 0.32f;
    #endregion



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        target = GameObject.Find("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        health = maxHealth;
        moveSpeed = defaultSpeed;
        wakeUpRange = wakeUpPercent * loseAggroRange;
        iFrames = 0;
    }

    void Update()
    {
        colourChanges();

        if (isDead)
        {
            //Debug.Log("enemy is dead");
            return;
        }

        iFrames -= Time.deltaTime;

        inAwakeRange = Vector3.Distance(target.position, transform.position) <= wakeUpRange;
        inLoseAggroRange = Vector3.Distance(target.position, transform.position) <= loseAggroRange;

        IsDamaged();

        if (inAwakeRange || isDamaged)
        {
            isAwake = true;
        }

        if (target && isAwake && attackCoolDown <= 100f)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
            lastMoveDirection = moveDirection;
            isWalking = true;
        }
        if (!inLoseAggroRange && !isDamaged)
        {
            isAwake = false;
            isWalking = false;
            moveDirection = new Vector3(0f, 0f, 0f);
        }
    }

    private void FixedUpdate()
    {
        Movement();
        takingDamage = false;
    }

    private void Movement()
    {

        if (isWalking)
        {
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, -lastMoveDirection);
        }

        if (isAwake) //target && 
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

    #region Colour change based on health
    private void colourChanges()
    {
        float percentOfMaxHealth = health / maxHealth;

        sprite.color = new Color(percentOfMaxHealth, percentOfMaxHealth, percentOfMaxHealth, 1);
    }
    #endregion

    public void TakeDamage(float damage)
    {
        if (iFrames <= 0)
        {
            health -= damage;
            isAwake = true;
            iFrames = defaultIFrames;
        }

        if (health <= 0.1)
        {
            //Destroy(gameObject);
            isDead = true;
            gameObject.SetActive(false);
            float randNum = Random.Range(0f, 10f) / 10f * 100f;

            // only drop health if the player is not full (similar to Super metroid)
            if (GameStatus.GetInstance().GetHealth() != GameStatus.GetInstance().GetMaxHealth())
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



    public void IsDamaged()
    {
        if (health < maxHealth)
        {
            isDamaged = true;
        }
        else
        {
            isDamaged = false;
        }
    }
}

