using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSBoss_Health : MonoBehaviour
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
        takingDamage = false;
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

        if (health == Mathf.Round(maxHealth * 0.9f) || health == Mathf.Round(maxHealth * 0.8f) || health == Mathf.Round(maxHealth * 0.7f) || health == Mathf.Round(maxHealth * 0.6f) || health == Mathf.Round(maxHealth * 0.5f) || health == Mathf.Round(maxHealth * 0.4f) || health == Mathf.Round(maxHealth * 0.3f) || health == Mathf.Round(maxHealth * 0.2f) || health == Mathf.Round(maxHealth * 0.1f))
        {
            Debug.Log("dropping stuff for the player");
            float randNum = Random.Range(0f, 10f) * 10f;

            if (randNum <= 50)
            {
                Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-10f, 10f), Aim.position.y + Random.Range(-10f, 10f), Aim.position.z);
                Instantiate(bigHealthDrop, dropPos, Aim.rotation);
            }
            else if (randNum > 50)
            {
                Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-10f, 10f), Aim.position.y + Random.Range(-10f, 10f), Aim.position.z);
                Instantiate(smallHealthDrop, dropPos, Aim.rotation);
            }

            float randNumAmmo = Random.Range(0f, 10f) * 10f;
            if (randNumAmmo <= 50)
            {
                Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-10f, 10f), Aim.position.y + Random.Range(-10f, 10f), Aim.position.z);
                Instantiate(bigAmmoDrop, dropPos, Aim.rotation);
            }
            else if (randNumAmmo > 50)
            {
                Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-10f, 10f), Aim.position.y + Random.Range(-10f, 10f), Aim.position.z);
                Instantiate(smallAmmoDrop, dropPos, Aim.rotation);
            }
        }

        if (health <= 0.1)
        {
            //Destroy(gameObject);
            isDead = true;
            gameObject.SetActive(false);
            int numOfDropsHealth = 0;
            int numOfDropsHealthSmall = 0;
            int numOfDropsAmmo = 0;
            int numOfDropsAmmoSmall = 0;
            float totalDrops = Random.Range(3, 5);

            while(totalDrops < 16)
            {
                if (numOfDropsHealth < 5)
                {
                    numOfDropsHealth += 1;
                    Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-2f, 2f), Aim.position.y + Random.Range(-2f, 2f), Aim.position.z);
                    Instantiate(bigHealthDrop, dropPos, Aim.rotation);
                }
                if (numOfDropsHealthSmall < 5)
                {
                    numOfDropsHealthSmall += 1;
                    Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-2f, 2f), Aim.position.y + Random.Range(-2f, 2f), Aim.position.z);
                    Instantiate(smallHealthDrop, dropPos, Aim.rotation);
                }
                if (numOfDropsAmmo < 5)
                {
                    numOfDropsAmmo += 1;
                    Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-2f, 2f), Aim.position.y + Random.Range(-2f, 2f), Aim.position.z);
                    Instantiate(bigAmmoDrop, dropPos, Aim.rotation);
                }
                if (numOfDropsAmmoSmall < 5)
                {
                    numOfDropsAmmoSmall += 1;
                    Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-2f, 2f), Aim.position.y + Random.Range(-2f, 2f), Aim.position.z);
                    Instantiate(smallAmmoDrop, dropPos, Aim.rotation);
                }
                totalDrops += 1;
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

