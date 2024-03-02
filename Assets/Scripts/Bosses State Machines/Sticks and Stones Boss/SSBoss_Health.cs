using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSBoss_Health : MonoBehaviour
{
    #region Varaibles
    [SerializeField]
    public bool isDead;

    [SerializeField]
    private Transform Aim;

    private SpriteRenderer sprite;

    [Header("Movement Variables")]

    private Transform target;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;

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
    private GameObject smallHealthDrop;

    [SerializeField]
    private GameObject bigAmmoDrop;
    [SerializeField]
    private GameObject smallAmmoDrop;

    public float iFrames;

    [SerializeField]
    private float defaultIFrames = 0.32f;
    #endregion

    void Start()
    {
        target = GameObject.Find("Player").transform;
        sprite = GetComponent<SpriteRenderer>();
        health = maxHealth;
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

        Aim.rotation = Quaternion.LookRotation(Vector3.forward, -lastMoveDirection);

        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            moveDirection = direction;
            lastMoveDirection = moveDirection;
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
            iFrames = defaultIFrames;
        }

        if (health == Mathf.Round(maxHealth * 0.9f) || health == Mathf.Round(maxHealth * 0.8f) || health == Mathf.Round(maxHealth * 0.7f) || health == Mathf.Round(maxHealth * 0.6f) || health == Mathf.Round(maxHealth * 0.5f) || health == Mathf.Round(maxHealth * 0.4f) || health == Mathf.Round(maxHealth * 0.3f) || health == Mathf.Round(maxHealth * 0.2f) || health == Mathf.Round(maxHealth * 0.1f))
        {
            Debug.Log("dropping stuff for the player");
            float randNum = Random.Range(0f, 10f) * 10f;

            if (randNum <= 50)
            {
                Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-8f, 8f), Aim.position.y + Random.Range(-8f, 8f), Aim.position.z);
                Instantiate(bigHealthDrop, dropPos, Aim.rotation);
            }
            else if (randNum > 50)
            {
                Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-8f, 8f), Aim.position.y + Random.Range(-8f, 8f), Aim.position.z);
                Instantiate(smallHealthDrop, dropPos, Aim.rotation);
            }

            float randNumAmmo = Random.Range(0f, 10f) * 10f;
            if (randNumAmmo <= 50)
            {
                Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-8f, 8f), Aim.position.y + Random.Range(-8f, 8f), Aim.position.z);
                Instantiate(bigAmmoDrop, dropPos, Aim.rotation);
            }
            else if (randNumAmmo > 50)
            {
                Vector3 dropPos = new Vector3(Aim.position.x + Random.Range(-8f, 8f), Aim.position.y + Random.Range(-8f, 8f), Aim.position.z);
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
}

