using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Telegraph: gives the player a very short animation to show what attack they are about to do
 * Entered from movement
 * Enters attack and death
 */
public class TelegraphAttack_BallOfAnger : BaseState_BallOfAnger
{
    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_BallOfAnger stateManager)
    {
        Debug.Log("Telegraph state entry");
        // use a coroutine to delay
        // he'll stop and vibrate for a moment, then the spikes will come out
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_BallOfAnger stateManager)
    {
        Debug.Log("Telegraph state update");
        stateManager.SwitchState(stateManager.attackState);
        //stateManager.SwitchState(stateManager.movementState);
    }

    

}
