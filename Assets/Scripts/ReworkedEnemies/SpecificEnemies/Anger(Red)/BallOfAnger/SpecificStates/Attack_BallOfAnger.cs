using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Attack: attacks the player
 * Entered from telegraph
 * Enters attackPunish and death
 */
public class Attack_BallOfAnger : BaseState_BallOfAnger
{
    // variables
    private float attackDuration = 0.5f;
    private float attackTimer = 0f;
    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_BallOfAnger stateManager)
    {
        Debug.Log("Attack state entry");
        // do the attack and the anim while waiting for the update to run until after the anim has run
        stateManager.TurnMeleeOn();
        attackTimer = attackDuration;
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_BallOfAnger stateManager)
    {
        Debug.Log("Attack state update");
        attackTimer -= Time.deltaTime;

        if(attackTimer <= 0)
        {
            stateManager.TurnMeleeOff();
            stateManager.SwitchState(stateManager.punishState);
        }
    }

}