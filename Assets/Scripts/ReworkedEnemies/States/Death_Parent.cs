using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Death: the animation and state that occurs when the enemy health is <= 0
 * Entered from any other state (except for drops)
 * Enters drops
 */
public class Death_Parent : BaseState_Parent
{
    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_Parent stateManager)
    {
        Debug.Log("Death state entry");
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_Parent stateManager)
    {
        Debug.Log("Death state update");
    }

}