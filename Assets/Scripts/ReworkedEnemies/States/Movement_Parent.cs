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
public class Movement_Parent : BaseState_Parent
{
    bool playerInRange;

    double attackCooldown;
    double healingCooldown;

    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_Parent stateManager)
    {
        Debug.Log("Movement state entry");
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_Parent stateManager)
    {
        Debug.Log("Movement state update");

        if(playerInRange || !stateManager.IsHealthFull())
        {
            if(attackCooldown <= 0)
            {
                stateManager.SwitchState(stateManager.telegraphState);
            }
            else if(healingCooldown <= 0 && !stateManager.IsHealthFull())
            {
                stateManager.SwitchState(stateManager.healingState);
            }
            else
            {
                Movement();     // will be specified by each of the enemies that use it (can be chaser, retreating, maintain, random, or static)
            }
        }

        // if player is not in range and the health is full, return to the idle state
        stateManager.SwitchState(stateManager.idleState);

    }

    public void Movement() { }

}