using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller3 : MonoBehaviour
{
    #region Set Up Variables
    // reference to the rigidbody, collider, and player scripts
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected Player player;

    // to save the gravityScale in
    private float originalGravity;

    [Header("Checks Variables")]

    //ground check
    [SerializeField]
    protected float distanceToColliderGround = 0.02f;
    [SerializeField]
    protected LayerMask collisionLayerGround;

    //wall check
    [SerializeField]
    protected float distanceToColliderWall = 0.1f;
    [SerializeField]
    protected LayerMask collisionLayerWall;

    [Header("Player States Bools")]
    // determine the ground state, controlled by other scripts
    [SerializeField]
    public bool isGrounded;

    // determine the ground state, controlled by other scripts
    [SerializeField]
    public bool isWalled;

    // determine if the player is jumping (will be controlled externally)
    [SerializeField]
    public bool isJumping;
    [SerializeField]
    public bool isDashing;
    [SerializeField]
    public bool isWallJumping;
    [SerializeField]
    public bool isWallSliding;
    [SerializeField]
    public bool isWallClimbing;
    [SerializeField]
    private bool isFalling;

    // to determine which direction the player is moving and flip them accordingly
    [SerializeField]
    public bool isFacingRight = true;
    #endregion

    #region Horizontal Movement Variables
    [Header("Horizontal Movement Variables")]
    // how fast the player should move
    [SerializeField]
    protected float walkSpeed = 500f;

    // how far away from the platform the player can be to see if it is a walkable object
    [SerializeField]
    protected float distanceToColliderWalk = 0.02f;

    // the layers the player should check and see for movement restrictions
    [SerializeField]
    protected LayerMask collisionLayerWalk;

    // float that checks how much value in the horizontal direction the input is receiving to better calculate speed
    private float horizontalInput;
    private float verticalInput;

    [Header("Dash Variables")]
    // related to dashing
    [SerializeField]
    private float dashForceHor = 800f;

    [SerializeField]
    private float dashForceNeutral = 800f;

    // how fast the dash can go
    [SerializeField]
    private float maxDashSpeed = 75f;

    // how fast the dash can go
    [SerializeField]
    private float dashVertSpeed = 10f;

    //// how long the dash lasts
    [SerializeField]
    private float dashingTime = 0.2f;

    //// amount of time between dashes when grounded
    [SerializeField]
    private float dashingCooldown = 0.25f;

    // dashing bools
    [SerializeField]
    private bool canDash;
    #endregion

    #region Jump Variables
    [Header("Jump Variables")]
    // total number of jumps allowed
    [SerializeField]
    private int maxJumps = 1;

    // how high the player should go when the jump button is pressed
    [SerializeField]
    private float jumpForce = 300f;

    // how high the player should go when the jump button is held
    [SerializeField]
    private float heldJumpForce = 50f;

    // how long the jump button should be held to perform maximum jump height
    [SerializeField]
    private float maxButtonHoldTime = 0.25f;

    // how close the player needs to be to a platform to be considered grounded
    private float distanceToColliderJump = 0.08f;

    // how fast the player can rise while jumping
    [SerializeField]
    private float maxJumpSpeed = 6f;

    // decrease jump height based on a shorter button press (from JumpMovement script)
    private float lowJumpMultiplier = 6f;

    //what objects the player should consider the ground
    [SerializeField]
    private LayerMask collisionLayerJump;

    // checks if the input for jump is pressed
    private bool jumpPressed;
    // checks to see if the input for jump is held down
    private bool jumpHeld;
    //how long the jump button has been held
    private float buttonHoldTime;
    // the number of jumps the player can perform after the initial jump
    private int numberOfJumpsLeft;

    #endregion

    #region Wall Slide Variables
    [Header("Wall Slide Variables")]
    // how fast they will slide while on the wall
    [SerializeField]
    private float wallSlidingSpeed = -3f;

    [SerializeField]
    private float wallClimbSpeed = 8f;
    #endregion

    #region Wall Jump Variables
    [Header("Wall Jump Variables")]
    [SerializeField]
    private float wallJumpingDirection;
    [SerializeField]
    private float wallJumpingTime = 0.2f;
    [SerializeField]
    private float wallJumpingCounter;
    [SerializeField]
    private float wallJumpingDuration = 1f; // was 0.4f
    [SerializeField]
    private Vector2 wallJumpingPower = new Vector2(50f, 16f);
    #endregion

    #region Falling variables
    [Header("Falling Variables")]
    // how fast the player can fall; this prevents the vertical velocity from getting decreasing as the player falls
    [SerializeField]
    private float maxFallSpeed = -20f;

    // how fast the players vertical velocity needs to be before considered falling
    [SerializeField]
    private float fallSpeed = 10f;

    // how much the gravity should be changed for certain things
    [SerializeField]
    private float gravityMultiplier = 6f;
    #endregion

    // this means that we wont have to initialize the variables in each script, keeping it a bit cleaner
    void Start()
    {
        Initialization();
        // sets the originalGravity to save the current rb.gravity scale
        originalGravity = rb.gravityScale;
        // sets up the buttonHoldTime to the original max value
        buttonHoldTime = maxButtonHoldTime;
        // sets the total number of jumps left back to the max value
        numberOfJumpsLeft = maxJumps;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        GroundCheck();
        WallCheck();

        // sets the horizontalInput value to the float value for Input.GetAxis("Horizontal") when not taking damage
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

        Flip();
        Falling();

        if (isGrounded || isWalled)
        {
            canDash = true;
            Debug.Log("Grounded or walled, can dash");
        }

        if (Input.GetKeyDown(KeyCode.J) && canDash)
        {
            Debug.Log("Pressed J, starting to dash");
            StartCoroutine(Dash());
        }
        else if (Input.GetKeyDown(KeyCode.J) && !canDash)
        {
            Debug.Log("Pressed J but canot dash");
        }

        // checks if the jump button is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
            Debug.Log("pressed jump");
        }
        else
        {
            jumpPressed = false;
        }

        // checks if the jump button is held down
        if (Input.GetKey(KeyCode.Space))
        {
            jumpHeld = true;
            Debug.Log("held jump");
        }
        else
        {
            jumpHeld = false;
        }

        CheckForJump();
        Jump();
        WallSlide();
        WallJump();
    }

    // handles all of the logic and physics for the rigidbody movement
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        // basic movement based on speed
        rb.velocity = new Vector2(horizontalInput * walkSpeed * Time.deltaTime, rb.velocity.y);
    }


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
    private IEnumerator Dash()
    {
        rb.velocity = Vector2.zero;
        canDash = false;
        isDashing = true;
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (horizontalInput != 0 && verticalInput != 0)
        {
            rb.velocity = Vector2.zero;
            Debug.Log("verticalInput = " + verticalInput);
            rb.AddForce(new Vector2(horizontalInput, verticalInput) * 0.75f * dashForceHor, ForceMode2D.Impulse);
        }
        else
        {
            rb.velocity = Vector2.zero;
            Debug.Log("verticalInput = " + verticalInput);
            rb.AddForce(new Vector2(horizontalInput, verticalInput) * dashForceHor, ForceMode2D.Impulse);
        }
        if (horizontalInput == 0 && verticalInput == 0)
        {
            rb.AddForce((isFacingRight ? Vector2.right : Vector2.left) * dashForceHor, ForceMode2D.Impulse);
        }

        //limits jump vertical velocity so multiple dashes performed quickly don't propel the player forward
        if (Mathf.Abs(rb.velocity.x) >= maxDashSpeed)
        {
            rb.velocity = new Vector2(isFacingRight ? maxDashSpeed : -maxDashSpeed, rb.velocity.y);
        }

        //rb.velocity = new Vector2(rb.velocity.x, verticalInput * dashVertSpeed);

        Debug.Log("dash velocities = " + rb.velocity);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        //rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashingCooldown);
        //animator.SetBool("isDashing", false);
        if (isGrounded)
        {
            canDash = true;
        }
    }
    #endregion

    #region Check For Jump
    private void CheckForJump()
    {
        if (jumpPressed) // && !isWalled)
        {
            // if the character is not grounded and hasn't performed an initial jump yet, they cannot jump
            if (!isGrounded && numberOfJumpsLeft == maxJumps)
            {
                // doesn't allow the jump and returns out of method
                isJumping = false;
                Debug.Log("is not able to jump");
                return;
            }

            // reduces number of jumps left by 1
            numberOfJumpsLeft--;

            // if the number of jumps left is not currently negative
            if (numberOfJumpsLeft >= 0)
            {
                // resets the velocity for fresh jump
                rb.velocity = new Vector2(rb.velocity.x, 0);

                // resets buttonHoldTime back to max value for a fresh jump
                buttonHoldTime = maxButtonHoldTime;

                // sets isJumping bool on the player script to true to enter the jumping state
                isJumping = true;
                Debug.Log("is able to jump");
            }
        }
    }
    #endregion

    #region Jump
    // handles rigidbody2d calculations for the jump
    private void Jump()
    {
        if (!isDashing)
        {
            // checks if player is injump state
            if (isJumping)
            {
                // applied initial jump force
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                // checks for additional air if holding down the jump button
                AdditionalAir();

                Debug.Log("jump velocities = " + rb.velocity);
            }
            else
            {
                isJumping = false;
            }
            // limits jump vertical velocity so multiple jumps performed quickly don't propel the player upwards
            if (rb.velocity.y >= maxJumpSpeed)
            {
                // sets the vertical velocity to the jump speed limit
                rb.velocity = new Vector2(rb.velocity.x, maxJumpSpeed);
            }
        }
        else
        {
            isJumping = false;
        }
    }
    #endregion

    #region Additional Air
    //additional jump force based on holding down the jump button
    private void AdditionalAir()
    {
        if (jumpHeld)
        {
            // negates the buttonHoldTime value by time
            buttonHoldTime -= Time.deltaTime;

            // if buttonHoldTime is 0 or less than 0
            if (buttonHoldTime <= 0)
            {
                // sets the buttonHoldTime to 0
                buttonHoldTime = 0;

                // get the character out of the jumping state
                isJumping = false;
            }
            else if (rb.velocity.y > 0 && Input.GetKey(KeyCode.Space))
            {
                rb.velocity = new Vector2(rb.velocity.x, heldJumpForce);
            }
        }
        // if not holding down the jump button any longer and buttonHoldTime is greater than 0
        else
        {
            // get the player out of the jumping state
            isJumping = false;
        }
    }
    #endregion

    #region Wall Slide
    private void WallSlide()
    {
        if (isWalled && !isGrounded && rb.velocity.y < 0 && horizontalInput != 0f)    // the player will only slide if they are pushing into the wall
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
        }
    }
    #endregion

    #region Wall Jump
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (jumpPressed && wallJumpingCounter > 0f)
        {
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
        }

    }

    private void StopWallJumping()
    {
        isWallJumping = false;
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

    #region Collision Check
    // create a method which will check if the player is in contact with another game object, which is how this version of the physics will determine the isGrounded state
    protected virtual bool CollisionCheck(Vector2 direction, float distance, LayerMask collision)
    {
        // set up an array of hits so if the player is colliding with multiple objects, it can sort through each one to look for the one it wants
        RaycastHit2D[] hits = new RaycastHit2D[10];

        // an int to help sort the hits variable so that player can run a for loop and check the values of each collisions
        int numHits = col.Cast(direction, hits, distance);

        // for loop that sorts hits with the int value it receives, based on the collider2D.Cast() method
        for (int i = 0; i < numHits; i++)
        {
            // if there is at elast 1 lyer that has ben setup by a child script of a layer it should look out for
            if ((1 << hits[i].collider.gameObject.layer & collision) != 0)
            {
                // returns this method as true if the above statement is true
                return true;
            }
        }
        // if the logic makes it to here, then there aren't any layers that this method should be looking out for
        return false;
    }
    #endregion

    #region Ground Check
    private void GroundCheck()
    {
        //method found in the player script to check collisions
        if (CollisionCheck(Vector2.down, distanceToColliderGround, collisionLayerGround))
        {
            // players enter a grounded state
            isGrounded = true;

            // sets a parameter on the animator to allow for ground based animations to play
            //anim.SetBool("Grounded", true);

            //reset the numberOfJumpsLeft back to the max value
            numberOfJumpsLeft = maxJumps;

            Debug.Log("Grounded");
        }
        // if the above statement returns false, then the character is not grounded or in the jumping state
        else
        {
            //player is not grounded
            isGrounded = false;
            Debug.Log("Not grounded");
        }
    }
    #endregion

    #region Wall Check
    private void WallCheck()
    {
        //method found in the player script to check collisions
        if (CollisionCheck(Vector2.right, distanceToColliderWall, collisionLayerWall))
        {
            // players enter a grounded state
            isWalled = true;
            Debug.Log("wall on right");

            //resets gravity back to the original value
            rb.gravityScale = originalGravity;
        }
        // if the above statement returns false, then the character is not grounded or in the jumping state
        else if (CollisionCheck(Vector2.left, distanceToColliderWall, collisionLayerWall))
        {
            // players enter a grounded state
            isWalled = true;
            Debug.Log("wall on left");

            // resets gravity back to the original value
            rb.gravityScale = originalGravity;
        }
        else
        {
            //player is not walled
            isWalled = false;
            Debug.Log("not walled");
        }
    }
    #endregion

    #region Falling
    private void Falling()
    {
        if (!isGrounded && !isWalled && !isDashing && !isJumping && !isWallSliding && !isWallJumping)
        {
            // if the vertical velocity is officially in the falling state
            if (rb.velocity.y < fallSpeed)
            {
                isFalling = true;
                // pushes the player down a bit faster and makes the jump a bit less floaty
                rb.gravityScale = gravityMultiplier;
                Debug.Log("Falling");
            }
            else
            {
                isFalling = false;
                Debug.Log("Not Falling");
            }

            // if the vertical velocity is less than the fastest the player should be falling
            if (rb.velocity.y < maxFallSpeed)
            {
                // set the verticla velocity to the max fall speed allowed
                rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
            }
        }
        else
        {
            isFalling = false;
            rb.gravityScale = originalGravity;
            Debug.Log("Not Falling");
        }
    }
    #endregion
}
