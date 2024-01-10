using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_TopDown : MonoBehaviour
{
    #region Set Up Variables
    // reference to the rigidbody, collider, and player scripts
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected Player player;

    [Header("Player state bools")]
    [SerializeField]
    private bool isWalking;
    [SerializeField]
    private bool isDashing;

    public bool isFacingRight = true;
    private Vector2 lastMoveDirection;

    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;
    #endregion

    #region Movement Variables
    [Header("Movement Variables")]
    private float activeMoveSpeed;

    // how fast the player should move
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
    private float knockBackForce;

    [SerializeField]
    private float knockBackCounter;

    private float knockBackTotalTime;

    [SerializeField]
    private bool knockFromRight;
    #endregion

    void Start()
    {
        Initialization();
        maxHealth = health;
        activeMoveSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        Flip();
        Movement();
        Dash();

        // dash with i-frames that lets them go through thin walls/enemies/projectiles
        // sneak (slow down movement but dont get detected by enemy ai?)
    }

    private void FixedUpdate()
    {
        if (isWalking)
        {
            Vector3 vector3 = Vector3.left * input.x + Vector3.down * input.y;
            Aim.rotation = Quaternion.LookRotation(Vector3.forward, vector3);
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
    #endregion

    #region Movement controller
    private void Movement()
    {
        if (knockBackCounter <= 0)
        {
            if (input.x != 0 && input.y != 0)
            {
                input *= 0.7f;
            }
            rb.velocity = input * activeMoveSpeed;
        }
        else
        {
            if(knockFromRight)
            {
                rb.velocity = new Vector2(-knockBackForce, knockBackForce);
            }
            if(!knockFromRight)
            {
                rb.velocity = new Vector2(knockBackForce, knockBackForce);
            }
            knockBackCounter -= Time.deltaTime;
        }
    }
    #endregion

    #region Initialize
    // this function will initialize all of our variables, which will run on start and be used by other scripts
    protected virtual void Initialization()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        player = GetComponent<Player>();
    }
    #endregion
}
