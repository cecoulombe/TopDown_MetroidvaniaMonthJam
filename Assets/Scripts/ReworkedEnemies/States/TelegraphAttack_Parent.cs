using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Telegraph: gives the player a very short animation to show what attack they are about to do
 * Entered from movement
 * Enters attack and death
 */
public class TelegraphAttack_Parent : BaseState_Parent
{
    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_Parent stateManager)
    {
        Debug.Log("Telegraph state entry");
        // run the animation and pause for the appropriate amount of time (i.e. can't move into the attack until the anim is completed)
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_Parent stateManager)
    {
        Debug.Log("Telegraph state update");
        stateManager.SwitchState(stateManager.attackState);
    }

}
