using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Attack: attacks the player
 * Entered from telegraph
 * Enters attackPunish and death
 */
public class Attack_Parent : BaseState_Parent
{
    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_Parent stateManager)
    {
        Debug.Log("Attack state entry");
        // do the attack and the anim while waiting for the update to run until after the anim has run
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_Parent stateManager)
    {
        Debug.Log("Attack state update");
        stateManager.SwitchState(stateManager.punishState);
    }

}