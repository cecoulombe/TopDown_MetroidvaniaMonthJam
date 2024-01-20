using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController_TopDown : MonoBehaviour
{
    #region Set Up Variables
    // reference to the rigidbody, collider, and player scripts
    protected Rigidbody2D rb;
    protected Collider2D col;

    [Header("Player state bools")]
    [SerializeField]
    private bool isWalking;
    [SerializeField]
    private bool hasDashing;
    [SerializeField]
    private bool isDashing;
    [SerializeField]
    public bool canMelee;
    [SerializeField]
    public bool canRanged;

    public GameManager_TopDown gameManager;

    public bool isFacingRight = true;
    private Vector2 lastMoveDirection;

    //[SerializeField]
    public float playerHealth;
    //[SerializeField]
    private float playerMaxHealth;
    public Image healthBar;
    #endregion

    #region Movement Variables
    [Header("Movement Variables")]
    private float activeMoveSpeed;

    // how fast thel player should move
    [SerializeField]
    protected float walkSpeed;

    // float that checks how much value in the horizontal direction the input is receiving to better calculate speed
    private Vector2 input;

    [Header("Dash Variables")]
    [SerializeField]
    private float dashSpeed;

    [SerializeField]
    private float dashLength;

    [SerializeField]
    private float dashCooldown;

    private float dashCounter;

    private float dashCoolCounter;
    #endregion

    #region Attack Variables
    [Header("Attack Variables")]
    [SerializeField]
    private Transform Aim;
    #endregion

    #region Damage and Health Variables
    [SerializeField]
    public float knockBackForce;

    [SerializeField]
    public float knockBackCounter;

    public float knockBackTotalTime;

    [SerializeField]
    public bool knockFromRight;
    #endregion

    void Start()
    {
        Initialization();
        //maxHealth = health;
        activeMoveSpeed = walkSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        playerHealth = GameStatus.GetInstance().GetHealth();
        playerMaxHealth = GameStatus.GetInstance().GetMaxHealth();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TakeDamage(playerMaxHealth);
        }

        Inputs();
        Flip();
        //Movement();
        Dash();

        // Posisble ideas of abilities and mechanics that could be added? not necessarily for the jam but just in general in case this is an idea I want to role with beyond Feb.
        // dash with i-frames that lets them go through thin walls/enemies/projectiles
        // sneak (slow down movement but dont get detected by enemy ai?)
        // dash attacks - you dash towards enemies to close the gap and it increases the size of the attack
        // elemental attacks that you have to use to break elemental barriers (i.e. flame thrower vs an ice wall)
        // charge attacks
        // area attakcs/spread shot rather than just a single bullet?
        // reflect projectiles back at the enemy? (like a parry maneuver?)
        // teleportors? maybe for a quick travel idea but idk in general
        // destroyable walls locked behind a specific ability
        // ice melt
        // locks and keys, bridges that get triggered by switches but block off other areas, elevators,
        // interactive map would be ideal, could have a day/night cycle depending on the upgrades or game progress, which could open up certain paths or hidden areas?
    }

    private void FixedUpdate()
    {
        if (isWalking)
        {
            Vector3 vector3 = Vector3.left * input.x + Vector3.down * input.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }

        if (knockBackCounter <= 0)  // not being knocked back so you can move
        {
            if (input.x != 0 && input.y != 0)
            {
                input *= 0.7f;
            }
            rb.velocity = input * activeMoveSpeed;
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

    #region Inputs
    private void Inputs()
    {
        //store the last move direction
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if(moveX == 0 && moveY == 0 && (input.x != 0 || input.y != 0))
        {
            isWalking = false;
            lastMoveDirection = input;
            Vector3 vector3 = Vector3.left * lastMoveDirection.x + Vector3.down * lastMoveDirection.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
        }
        else if(moveX != 0 || moveY != 0)
        {
            isWalking = true;
        }

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");


    }
    #endregion

    #region Flip
    // flip where the player is facing
    private void Flip()
    {
        if (isFacingRight && input.x < 0f || !isFacingRight && input.x > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    #endregion

    #region Dash
    private void Dash()
    {
        if (hasDashing)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    isDashing = true;
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;
                }
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;

                if (dashCounter <= 0)
                {
                    isDashing = false;
                    activeMoveSpeed = walkSpeed;
                    dashCoolCounter = dashCooldown;
                }
            }

            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }
        }
    }
    #endregion

    #region Take Damage
    public void TakeDamage(float damage)
    {
        //playerHealth -= damage;
        //healthBar.fillAmount = playerHealth / playerMaxHealth;
        StartCoroutine(GameStatus.GetInstance().LoseHealth(damage));
        //if (playerHealth <= 0)
        //{
        //    //GameStatus.GetInstance().AddDeath();
        //    yield return new WaitForSeconds(0.25f);
        //    //Destroy(gameObject);
        //    gameManager.Death();
        //}
    }
    #endregion

    #region Heal
    public void Heal(float healAmount)
    {
        //playerHealth += healAmount;
        //playerHealth = Mathf.Clamp(playerHealth, 0, playerMaxHealth);
        GameStatus.GetInstance().AddHealth(healAmount);
        //healthBar.fillAmount = playerHealth / playerMaxHealth;
    }
    #endregion

    #region Initialize
    // this function will initialize all of our variables, which will run on start and be used by other scripts
    protected virtual void Initialization()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
    #endregion
}
