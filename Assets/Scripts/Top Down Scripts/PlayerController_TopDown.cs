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
    protected bool isDashing;

    public bool isFacingRight = true;

    #endregion

    #region Movement Variables
    [Header("Movement Variables")]
    private float activeMoveSpeed;

    // how fast the player should move
    [SerializeField]
    protected float walkSpeed;
    
    // float that checks how much value in the horizontal direction the input is receiving to better calculate speed
    private float horizontalInput;
    private float verticalInput;

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

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private Direction lastInputDirection = Direction.Right;

    void Start()
    {
        Initialization();
        activeMoveSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        Flip();
        Movement();
        Dash();
    }

    #region Inputs
    private void Inputs()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }
        // if no input or taking damage, horizontalInput = 0
        else
        {
            horizontalInput = 0;
        }
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            verticalInput = Input.GetAxisRaw("Vertical");
        }
        // if no input or taking damage, horizontalInput = 0
        else
        {
            verticalInput = 0;
        }

        // Determine the last input direction
        if (horizontalInput > 0)
        {
            lastInputDirection = Direction.Right;
        }
        else if (horizontalInput < 0)
        {
            lastInputDirection = Direction.Left;
        }
        else if (verticalInput > 0)
        {
            lastInputDirection = Direction.Up;
        }
        else if (verticalInput < 0)
        {
            lastInputDirection = Direction.Down;
        }
    }
    #endregion

    #region Flip
    // flip where the player is facing
    private void Flip()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
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
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
            }
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
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
        if(horizontalInput != 0 && verticalInput != 0) 
        {
            horizontalInput *= 0.7f;
            verticalInput *= 0.7f;
        }
        rb.velocity = new Vector2(horizontalInput * activeMoveSpeed, verticalInput * activeMoveSpeed);
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
