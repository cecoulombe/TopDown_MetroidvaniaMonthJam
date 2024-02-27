using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController_TopDown : MonoBehaviour
{
    // Set Up Variables contain all variables relating to the rigid body, collider, and player state bools. Also contains the references to managers in the scene and the direction the player is moving
    #region Set Up Variables    
    // reference to the rigidbody, collider, and player scripts
    protected Rigidbody2D rb;
    protected Collider2D col;

    Animator anim;

    [Header("Player state bools")]
    [SerializeField]
    private bool isWalking;
    [SerializeField]
    private bool isDashing;
    [SerializeField]
    private bool isInvincibleDashing;

    //public GameManager_TopDown gameManager;
    public DeathManager deathManager;
    public InteractionManager interactionManager;

    public bool isFacingRight = true;
    private Vector2 lastMoveDirection;

    public float playerHealth;
    private float playerMaxHealth;
    public Image healthBar;
    #endregion

    // Movement variables contain the active movement speed, the walk speed, and the dash variables
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

    [SerializeField]
    private float iDashCooldown;

    private float iDashCoolCounter;
    #endregion    

    #region Knockback Variables
    [SerializeField]
    public float knockBackForce;

    [SerializeField]
    public float knockBackCounter;

    public float knockBackTotalTime;

    [SerializeField]
    public bool knockFromRight;
    #endregion

    #region Healing Variables
    [Header("Healing Variables")]
    [SerializeReference]
    private float healthGain;

    [SerializeField]
    private float ammoCost;

    [SerializeField]
    private float buttonHeldMinimum;

    private float buttonHeldDuration;

    public GameObject healingAnim;

    #endregion

    // Attack variables contains all variables for both types of attacks done by the player (melee and ranged)
    #region Attack Variables
    [Header("Attack Variables")]
    [SerializeField]
    private Transform Aim;

    #region Melee Variables
    [Header("Melee Variables")]
    public GameObject Melee;

    [SerializeField]
    private bool isAttacking;

    [SerializeField]
    private float timeToAttack = 0.3f;

    [SerializeField]
    private float attackDuration = 0.3f;

    [SerializeField]
    private float attackTimer = 0f;
    #endregion

    #region Ranged Variables
    [Header("Ranged Variables")]

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float fireForce = 10f;

    [SerializeField]
    private float shootCoolDown = 0.25f;

    [SerializeField]
    private float shootTimer = 0.5f;
    #endregion
    #endregion

    void Awake()
    {
        Initialization();
        anim = GetComponent<Animator>();
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

        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("pausing the game");
            GameStatus.GetInstance().SetGamePaused(true);
        }

        if (Input.GetKey(KeyCode.H))
        {
            buttonHeldDuration += 1f;
            Healing();
            return;
        }
        else
        {
            buttonHeldDuration = 0;
            healingAnim.SetActive(false);
        }

        Inputs();
        Animate();
        Flip();
        
        Dash();

        if (playerHealth <= 0.01)
        {
            deathManager.Death();
        }

        CheckMeleeTimer();
        shootTimer += Time.deltaTime;
        OnAttack();
        OnShoot();
        Interaction();
        

        // Posisble ideas of abilities and mechanics that could be added? not necessarily for the jam but just in general in case this is an idea I want to role with beyond Feb.
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

    #region Animations
    private void Animate()
    {
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
        anim.SetFloat("MoveMagnitude", input.magnitude);
        anim.SetFloat("LastMoveX", lastMoveDirection.x);
        anim.SetFloat("LastMoveY", lastMoveDirection.y);


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

    #region Healing
    private void Healing()
    {
        // heal the player at the cost of ammo
        if(GameStatus.GetInstance().HasHealing() && GameStatus.GetInstance().GetAmmo() > 0 && playerHealth != playerMaxHealth)
        {
            if(buttonHeldDuration >= buttonHeldMinimum)
            {
                //start healing and losing ammo
                healingAnim.SetActive(true);
                GameStatus.GetInstance().AddHealth(healthGain);
                GameStatus.GetInstance().ShootBullets(ammoCost);
            }
        }
        else
        {
            healingAnim.SetActive(false);
        }
    }
    #endregion


    #region Dash
    private void Dash()     // the base dash will allow the player to "jump" gaps and evade attacks but will not give i-frames
    {
        if (GameStatus.GetInstance().HasDash())
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if (dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    isDashing = true;
                    activeMoveSpeed = dashSpeed;
                    // add invincibility for the dash if the player has picked up iframes (will allow them to dash through small walls as well)
                    if(GameStatus.GetInstance().HasInvincibleDash() && iDashCoolCounter <= 0)
                    {
                        Debug.Log("starting the idash");
                        //do the iframe dash
                        col.enabled = false;
                        isInvincibleDashing = true;
                    }
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
                    if(isInvincibleDashing)
                    {
                        Debug.Log("end of idash, start the cooldown for that, the hit box should be back on now");
                        col.enabled = true;
                        isInvincibleDashing = false;
                        iDashCoolCounter = iDashCooldown;
                    }
                }
            }

            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }
            if(iDashCoolCounter > 0)
            {
                iDashCoolCounter -= Time.deltaTime;
            }
            else
            {
                Debug.Log("idash has cooled down and can go again");
            }
        }
    }
    #endregion

    #region Take Damage
    public void TakeDamage(float damage)
    {
        GameStatus.GetInstance().LoseHealth(damage);
        //if (playerHealth <= 0.01)
        //{
        //    //yield return new WaitForSeconds(0.25f);
        //    deathManager.Death();
        //}
    }
    #endregion

    #region Heal
    public void Heal(float healAmount)
    {
        GameStatus.GetInstance().AddHealth(healAmount);
    }
    #endregion

    #region Attacking
    private void OnAttack()
    {
        if ((Input.GetKeyDown(KeyCode.K)) && GameStatus.GetInstance().HasMelee())
        {
            if (!isAttacking)
            {
                Melee.SetActive(true);
                isAttacking = true;
                // call your animator to play your melee attack
            }
        }
    }

    private void CheckMeleeTimer()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDuration)
            {
                attackTimer = 0;
                isAttacking = false;
                Melee.SetActive(false);
            }
        }
    }

    private void OnShoot()
    {
        if ((Input.GetKeyDown(KeyCode.L)) && GameStatus.GetInstance().HasRanged() && GameStatus.GetInstance().GetAmmo() > 0)
        {
            if (shootTimer > shootCoolDown)
            {
                shootTimer = 0;
                GameObject intBullet = Instantiate(bullet, Aim.position, Aim.rotation);
                intBullet.GetComponent<Rigidbody2D>().AddForce(-Aim.up * fireForce, ForceMode2D.Impulse);
                GameStatus.GetInstance().ShootBullets(1);
                Destroy(intBullet, 2f);
            }
        }
        else if(GameStatus.GetInstance().GetAmmo() <= 0)
        {
            Debug.Log("no ammo left, need to refill");
        }
    }
    #endregion

    #region Interacting
    private void Interaction()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            // send out the beam to see if there is an interactable in front of you
            // if there is, start the interaction for the appropriate object type
            interactionManager.IsInteracting();
        }
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
