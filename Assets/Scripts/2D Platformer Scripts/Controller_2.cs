using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller_2 : MonoBehaviour
{
    #region
    public GameObject player;
    public Transform respawnPoint;
    private float airTime;
    private bool hardFallUsed = false;

    [Header("Movement Variables")]
    public bool isGrounded;
    private float horizontal;
    public float speed = 4f;
    public float airMoveSpeed = 20f;
    private float jumpingPower = 18f;
    private bool isFacingRight = true;

    [Header("Ability Checks")]
    private bool cantMove = false;
    public bool canLadder = true;
    public bool canJump = true;
    public bool canwallClimb = true;
    public bool hasDash = true;
    public bool canDash = true;
    public bool canDoubleJump = true;

    [Header("Ladder")]
    private float vertical;
    public bool isLadder;
    public bool isClimbing;

    [Header("Dash")]
    public bool isDashing;
    private float dashingPower = 4f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.25f;

    [Header("Wall Slide and Wall Jump")]
    public bool isWalled;
    public bool isWallEnough;
    bool isWallSliding;
    public float wallSlidingSpeed;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 1f; // was 0.4f
    private Vector2 wallJumpingPower = new Vector2(50f, 16f);

    [Header("Double Jump")]
    public bool doubleJumpAvailable;


    [Space(20)]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TrailRenderer tr;

    //Set all of these up in the inspector
    [Header("Checks")]
    [SerializeField] private Transform groundCheck;
    //Size of groundCheck depends on the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.7f, 0.5f);
    [Space(5)]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.5f, 1.5f);
    [SerializeField] private Transform wallEnoughCheck;
    [SerializeField] private Vector2 wallEnoughCheckSize = new Vector2(3f, 1.5f);

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    #endregion


    void Update()
    {
        if (isDashing)
        {
            return;
        }

        // movement
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.7f, 0.03f), 0, groundLayer);
        isWalled = Physics2D.OverlapBox(wallCheck.position, new Vector2(0.5f, 1.5f), 0, groundLayer);
        isWallEnough = Physics2D.OverlapBox(wallEnoughCheck.position, new Vector2(3f, 1.5f), 0, groundLayer);

        // jumping and double jump
        if (isGrounded && canDoubleJump)
        {
            doubleJumpAvailable = true;
        }

        if (isGrounded)
        {
            canDash = true;
        }

        if (isWalled && canDoubleJump)
        {
            doubleJumpAvailable = true;
        }

        if (isWalled)
        {
            canDash = true;
        }

        if (isWallEnough && canDoubleJump)
        {
            doubleJumpAvailable = true;
        }

        // ladder
        if (isLadder && Mathf.Abs(vertical) > 0f && canLadder)
        {
            isClimbing = true;
        }

        WallJump();

        // jump
        if (Input.GetButtonDown("Jump") && canJump)
        {
            Jump();
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }


        // dash
        if (hasDash)
        {
            if (Input.GetKeyDown(KeyCode.J) && canDash)
            {
                StartCoroutine(Dash());
            }
        }

        Flip();
    }


    private void FixedUpdate()  // this is where you keep everything related to physics
    {
        // warp to checkpoint (debugging only - remove after)
        if (Input.GetKey(KeyCode.RightAlt))
        {
            Debug.Log("Warpped back to the respawn point.");
            player.transform.position = respawnPoint.position;
        }

        if (isDashing)
        {
            return;
        }


        Movement();
        Ladder();
        WallSlide();        // might want to change all of this out to be funcitons rather than the actual code in the fixed update


    }

    // movement
    private void Movement()
    {
        if (!cantMove)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else
        {
            hardFallUsed = true;
        }
    }

    // jump and double jump
    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            doubleJumpAvailable = true;
        }
        else if (doubleJumpAvailable && canDoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

            doubleJumpAvailable = false;
        }
    }

    // ladder
    private void Ladder()
    {
        // ladder
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = 3f;
        }
    }

    // flip where the player is facing
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // wall slide
    private void WallSlide()
    {
        if (isWalled && !isGrounded && rb.velocity.y < 0 && horizontal != 0f)    // the player will only slide if they are pushing into the wall
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, wallSlidingSpeed);
            doubleJumpAvailable = true;
        }
    }

    // make it so that 
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
            doubleJumpAvailable = true;
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            doubleJumpAvailable = true;
            isWallJumping = true;
            //rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            // Apply vertical force
            rb.velocity = new Vector2(rb.velocity.x, wallJumpingPower.y);

            // Apply temporary horizontal velocity
            rb.velocity = new Vector2(wallJumpingPower.x, rb.velocity.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
            doubleJumpAvailable = true;
        }

    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }


    //dash
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        //tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        //tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        if (isGrounded || isWallEnough || isWalled)
        {
            canDash = true;
        }
    }
}