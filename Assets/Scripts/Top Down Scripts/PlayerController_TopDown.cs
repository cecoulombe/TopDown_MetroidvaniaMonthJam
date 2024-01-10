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

    #endregion

    #region Movement Variables
    [Header("Movement Variables")]
    // how fast the player should move
    [SerializeField]
    protected float walkSpeed;
    
    // float that checks how much value in the horizontal direction the input is receiving to better calculate speed
    private float horizontalInput;
    private float verticalInput;
    #endregion

    void Start()
    {
        Initialization();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        Movement();
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
        rb.velocity = new Vector2(horizontalInput * walkSpeed, verticalInput * walkSpeed);
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
