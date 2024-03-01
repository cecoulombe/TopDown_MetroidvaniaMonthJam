using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerBoss : MonoBehaviour
{   // this is for the boss that will appear after the player picks up melee. He will wander around randomly and will randomly spit out flameboys

    #region Varaibles
    private bool amDead;
    private bool amDamaged;

    [SerializeField]
    private EnemyHealth myHealth;

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sprite;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;

    private bool amAwake;

    [Header("Movement Variables")]
    [SerializeField]
    private float changeDirectionTimer;

    private Vector2 targetDirection;

    [Header("Minion Spawn Variables")]
    [SerializeField]
    private float minionSpawnTimer;

    [SerializeField]
    private GameObject minion;

    [Header("Attacking and Player Damage")]
    [SerializeField]
    private float contactDamage;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        targetDirection = transform.right;
    }

    void Update()
    {
        if (GameStatus.GetInstance().HasMelee())
        {
            col.enabled = true;
            sprite.enabled = true;
            amAwake = true;
        }
        else
        {
            col.enabled = false;
            sprite.enabled = false;
            amAwake = false;
        }

        myHealth.isAwake = false;   // I want the boss to be unaware of the player, they just move around randomly

        if (myHealth.isDead)
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        if(amAwake)
        {
            Movement();
            SpawnMinions();
        }
    }

    private void Movement()
    {
        changeDirectionTimer -= Time.deltaTime;

        if (changeDirectionTimer <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            targetDirection = rotation * targetDirection;
            changeDirectionTimer = Random.Range(1f, 2.5f);
        }
        // wander randomly, regardless of the player
        rb.velocity = new Vector2(targetDirection.x, targetDirection.y) * (myHealth.moveSpeed + Random.Range(0f, 2f));
    }

    private void SpawnMinions()
    {
        minionSpawnTimer -= Time.deltaTime;

        if(minionSpawnTimer <= 0)
        {
            //spawn a flame boy minion
            Vector3 spawnPos = new Vector3(transform.position.x + Random.Range(-4f, 4f), transform.position.y + Random.Range(-4f, 4f), transform.position.z);
            Instantiate(minion, spawnPos, transform.rotation);

            minionSpawnTimer = Random.Range(1f, 6f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            // change directions
            changeDirectionTimer = 0f;
            targetDirection = -targetDirection;
        }

        if (collision.gameObject.tag == "Player")
        {
            changeDirectionTimer = 0f;
            targetDirection = -targetDirection;

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

