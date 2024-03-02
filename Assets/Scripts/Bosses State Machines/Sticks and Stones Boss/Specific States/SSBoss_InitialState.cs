using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSBoss_InitialState : SSBoss_BaseState
{
    //#region Healing Variables
    //[Header("Healing Variables")]
    //[SerializeField]
    //private float healingCooldown;

    //[SerializeField]
    //private float healingCoolCounter;
    //#endregion

    public override void EnterState(SSBoss_StateManager boss)
    {
        Debug.Log("hello from the initial state");
    }

    public override void UpdateState(SSBoss_StateManager boss, Transform targetLocation, float health, float maxHealth)
    {
        // check if can melee
        if(Vector3.Distance(targetLocation.position, boss.transform.position) < boss.meleeAttackRange)
        {
            boss.SwitchState(boss.meleeState);
        }

        // check if can ranged
        if (Vector3.Distance(targetLocation.position, boss.transform.position) >= boss.rangedAttackRangeMin && Vector3.Distance(targetLocation.position, boss.transform.position) <= boss.rangedAttackRangeMax)
        {
            boss.SwitchState(boss.shootingState);
        }

        // can melee if the player is in range and the cool down is less than thelimit, and the limit is being checked in the melee script, so just see if they are in range and then switch states
        // check if can heal
        boss.healingCoolCounter -= Time.deltaTime;

        if (health < maxHealth)
        {
            if (boss.healingCoolCounter <= 0)
            {
                boss.SwitchState(boss.healingState);
            }
            else
            {
                boss.healingAnim.SetActive(false);
            }
        }
        else
        {
            boss.healingCoolCounter = boss.healingCooldown + Random.Range(1f, 10f);   // this should make it so that they can't insta heal after taking damage and so that they can reach max health before the counter starts again
            boss.healingAnim.SetActive(false);
        }
        // transition(s) to other state(s)
    }

    public override void OnCollisionEnter2D(SSBoss_StateManager boss)
    {

    }
}
