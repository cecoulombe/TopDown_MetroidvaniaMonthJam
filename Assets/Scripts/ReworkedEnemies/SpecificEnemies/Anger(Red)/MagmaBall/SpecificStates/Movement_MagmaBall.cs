using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Movement: based on the type of enemy, will determine how to move once they are awake
 * Ensures the player is within range to stay awake
 * Will also keep track of the attack and healing cooldowns (acts as the default state when awake)
 * Selects the attack that will be executed after the cooldown is finished 
 * Can be entered from idle, attackPunish, and heal
 * Can enter idle, telegraphAttack, healing, and death
 */
public class Movement_MagmaBall : BaseState_MagmaBall
{
    // Attack Variables
    private float attackCooldown = 1.2f;

    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_MagmaBall stateManager)
    {
        Debug.Log("Movement state entry");

        attackCooldown += Random.Range(-0.7f, 0.7f);
        //Debug.Log(attackCooldown + "seconds");
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_MagmaBall stateManager)
    {
        Debug.Log("Movement state update");

        attackCooldown -= Time.deltaTime;

        if (!stateManager.PlayerInRange() && stateManager.IsHealthFull())
        {
            stateManager.SwitchState(stateManager.idleState);
        }

        if(attackCooldown <= 0f && stateManager.InAttackRange())
        {
            attackCooldown = 1.2f;
            stateManager.SwitchState(stateManager.telegraphState);
        }

        stateManager.RetreatFromPlayer();

    }
}