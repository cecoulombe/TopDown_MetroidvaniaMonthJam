using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Idle: player is not within the aggro range of the enemy and the enemy is at full health.
 * They will be in sentry mode and waiting for something to "wake them up"
 * 
 * Starting state
 * Can be entered from movement
 * Can enter movement and death
 */
public class Idle_Parent : BaseState_Parent
{
    private bool playerInRange;
    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_Parent stateManager)
    {
        Debug.Log("Idle state entry");
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_Parent stateManager)
    {
        Debug.Log("Idle state update");

        // calculate the range of the player

        if(playerInRange)
        {
            stateManager.SwitchState(stateManager.movementState);
        }

        // idle movements
        IdleMovement();
    }

    void IdleMovement() { }
}
