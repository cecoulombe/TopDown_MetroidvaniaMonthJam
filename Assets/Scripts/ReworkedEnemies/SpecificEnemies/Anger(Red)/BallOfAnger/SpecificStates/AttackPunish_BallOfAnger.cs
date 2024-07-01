using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* AttackPunish: gives the player a very short animation to hit the enemy while it "recovers" from the attack
 * Entered from attack
 * Enters movement and death
 */
public class AttackPunish_BallOfAnger : BaseState_BallOfAnger
{
    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_BallOfAnger stateManager)
    {
        Debug.Log("Punish state entry");
        // anim and pause for a moment so the player can punish the attack (will probably have a different punish for each anim)
        // use a corourtine again with the anim
        // he'll get extra small and still for a second 
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_BallOfAnger stateManager)
    {
        Debug.Log("Punish state update");
        stateManager.SwitchState(stateManager.movementState);
    }
}