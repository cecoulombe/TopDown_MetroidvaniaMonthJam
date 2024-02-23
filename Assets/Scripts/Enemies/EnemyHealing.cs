using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealing : MonoBehaviour
{
    #region Variables
    private EnemyHealth myHealth;
    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float healingCooldown;

    [SerializeField]
    private float healingCoolCounter;

    [SerializeField]
    private float healingAmount;

    [SerializeField]
    public GameObject healingAnim;

    [SerializeField]
    private bool canHeal;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        myHealth = GetComponent<EnemyHealth>();

        maxHealth = myHealth.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(!canHeal)
        {
            return;
        }

        health = myHealth.health;

        healingCoolCounter -= Time.deltaTime;

        if(health < maxHealth)
        {
            if(healingCoolCounter <= 0)
            {
                healingAnim.SetActive(true);
                Healing();
            }
            else
            {
                healingAnim.SetActive(false);
            }
        }
        else
        {
            healingCoolCounter = healingCooldown + Random.Range(1f, 10f);   // this should make it so that they can't insta heal after taking damage and so that they can reach max health before the counter starts again
            healingAnim.SetActive(false);

        }
    }

    #region Healing
    private void Healing()
    {
        myHealth.moveSpeed = 0f;
        myHealth.health += Time.deltaTime * healingAmount;
    }
    #endregion
}
