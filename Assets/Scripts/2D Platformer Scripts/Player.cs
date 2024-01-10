using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    // reference to the rigidbody, collider, and player scripts
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected Player player;

    // determine the ground state, controlled by other scripts
    [HideInInspector]
    public bool isGrounded;

    // determine the ground state, controlled by other scripts
    [HideInInspector]
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

    // to determine which direction the player is moving and flip them accordingly
    //[HideInInspector]
    public bool isFacingRight = true;
    #endregion

    // this means that we wont have to initialize the variables in each script, keeping it a bit cleaner
    void Start()
    {
        Initialization();
    }

    // this function will initialize all of our variables, which will run on start and be used by other scripts
    protected virtual void Initialization()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        player = GetComponent<Player>();
    }

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
}