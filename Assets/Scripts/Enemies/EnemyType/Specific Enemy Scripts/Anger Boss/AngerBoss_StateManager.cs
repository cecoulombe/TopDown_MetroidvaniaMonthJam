using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerBoss_StateManager : MonoBehaviour
{
    #region State Machine Variables
    AngerBoss_BaseState currentState;

    public AngerBoss_MovementState movementState = new AngerBoss_MovementState();

    public AngerBoss_MinionSpawnState minionSpawnState = new AngerBoss_MinionSpawnState();
    #endregion

    #region Other Variables
    private AngerBoss_Health myHealth;

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sprite;

    [Header("Movement Variables")]
    [SerializeField]
    private float changeDirectionTimer;

    private Vector2 targetDirection;

    [SerializeField]
    private GameObject minion;

    [Header("Attacking and Player Damage")]
    [SerializeField]
    private float contactDamage;
    #endregion


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        myHealth = GetComponent<AngerBoss_Health>();
        targetDirection = transform.right;
        // set the starting state for the machine
        currentState = movementState;

        currentState.EnterState(this);
    }

    void Update()
    {
        if (GameStatus.GetInstance().HasMelee())
        {
            col.enabled = true;
            sprite.enabled = true;
            currentState.UpdateState(this, myHealth.health, myHealth.maxHealth);

        }
        else
        {
            col.enabled = false;
            sprite.enabled = false;
        }
    }

    public void SwitchState(AngerBoss_BaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void Movement()
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

    public void SpawnMinions()
    {
        Vector3 spawnPos = new Vector3(transform.position.x + Random.Range(-4f, 4f), transform.position.y + Random.Range(-4f, 4f), transform.position.z);
        Instantiate(minion, spawnPos, transform.rotation);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
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
