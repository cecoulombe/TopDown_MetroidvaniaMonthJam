using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerBoss_Health : MonoBehaviour
{
    #region Varaibles
    private bool isDead;

    private Rigidbody2D rb;

    private SpriteRenderer sprite;

    [Header("Enemy Health")]
    [SerializeField]
    public float health;
    [SerializeField]
    public float maxHealth;

    public bool takingDamage;

    [SerializeField]
    public float moveSpeed;

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
    #endregion



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    void Update()
    {
        colourChanges();

        if (isDead)
        {
            //Debug.Log("enemy is dead");
            return;
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
        health -= damage;
        moveSpeed += 0.5f;
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
                    Vector3 dropPos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
                    Instantiate(bigHealthDrop, dropPos, transform.rotation);
                }
                else if (randNum > bigHealthChance && randNum <= smallHealthChance)
                {
                    Vector3 dropPos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
                    Instantiate(smallHealthDrop, dropPos, transform.rotation);
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
                        Vector3 dropPos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
                        Instantiate(bigAmmoDrop, dropPos, transform.rotation);
                    }
                    else if (randNumAmmo > bigAmmoChance && randNumAmmo <= smallAmmoChance)
                    {
                        Vector3 dropPos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
                        Instantiate(smallAmmoDrop, dropPos, transform.rotation);
                    }
                }
            }
        }
    }
}

