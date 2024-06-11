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

    [SerializeField]
    private float defaultIFrames = 0.32f;

    private float iFrames;

    private SpriteRenderer sprite;

    // set the iFrames counter to equal zero when the enemy first loads into the scene
    private void Start()
    {
        iFrames = 0;
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        colourChange();
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
        if(iFrames <= 0)
        {
            currentHealth -= damage;
            iFrames = defaultIFrames;
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


}
