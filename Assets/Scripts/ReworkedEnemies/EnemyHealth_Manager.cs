using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// bare bones health tracker than all enemies can use so that they can be reacognized by the player's weapons
public class EnemyHealth_Manager : MonoBehaviour
{
    // health variables

    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private float maxHealth;



    private SpriteRenderer sprite;

    private Rigidbody2D rb;

    //// knockback and damage variables
    //[SerializeField]
    //private float knockBackForce;

    public float knockBackCounter;

    public float knockBackTotalTime;

    //public bool knockFromRight;

    [SerializeField]
    private float defaultIFrames = 0.32f;

    private float iFrames;

    // set the iFrames counter to equal zero when the enemy first loads into the scene
    private void Start()
    {
        iFrames = 0;
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        colourChange();
        iFrames -= Time.deltaTime;
        knockBackCounter -= Time.deltaTime;
    }

    //--------------------------------------------------------------------------
    // GetCurrentHealth() returns the current health values
    //--------------------------------------------------------------------------
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    //--------------------------------------------------------------------------
    // GetMaxHealth() returns the max health for the enemy
    //--------------------------------------------------------------------------
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    //--------------------------------------------------------------------------
    // TakeDamage()
    //--------------------------------------------------------------------------
    public void TakeDamage(float damage)
    {
        if (iFrames <= 0)
        {
            currentHealth -= damage;
            iFrames = defaultIFrames;
            knockBackCounter = knockBackTotalTime;
            //Knockback();
        }
    }


    //--------------------------------------------------------------------------
    // colourChange() changes the colour of the enemy based on their current health (more health = lighter, less health = darker)
    //--------------------------------------------------------------------------
    private void colourChange()
    {
        float percentOfMaxHealth = currentHealth / maxHealth;

        sprite.color = new Color(percentOfMaxHealth, percentOfMaxHealth, percentOfMaxHealth, 1);
    }

    ////--------------------------------------------------------------------------
    //// Knockback() prevents the 
    ////--------------------------------------------------------------------------
    //private void Knockback()
    //{
    //    if (knockBackCounter >= 0)
    //    {
    //        //knockBackCounter -= Time.deltaTime;

    //        if (knockFromRight)
    //        {
    //            //rb.velocity = new Vector2(-knockBackForce, 0f);
    //            rb.AddForce(new Vector2(-knockBackForce, 0f));
    //        }
    //        if (!knockFromRight)
    //        {
    //            rb.AddForce(new Vector2(knockBackForce, 0f));
    //        }
    //    }

    //    //rb.velocity = new Vector2(0f, 0f);
    //}
}