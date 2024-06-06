using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* AttackPunish: gives the player a very short animation to hit the enemy while it "recovers" from the attack
 * Entered from attack
 * Enters movement and death
 */
public class AttackPunish_Parent : BaseState_Parent
{
    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_Parent stateManager)
    {
        Debug.Log("Punish state entry");
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_Parent stateManager)
    {
        Debug.Log("Punish state update");
    }

}