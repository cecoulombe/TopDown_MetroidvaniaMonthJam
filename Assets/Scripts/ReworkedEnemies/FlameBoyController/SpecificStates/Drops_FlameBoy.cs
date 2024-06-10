using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Drops: scatters the drops about the position of the enemy
 * Entered from death
 * Enters nothing
 */
public class Drops_FlameBoy : BaseState_FlameBoy
{
    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_FlameBoy stateManager)
    {
        Debug.Log("Drops state entry");
        // scatter drops (determine what kind of drops and at what drop rate, and scatter within some range of the enemy sprite)
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_FlameBoy stateManager)
    {
        Debug.Log("Drops state update");
        // no transition out, this is the final state and the enemy stays here becuase it is dead
    }

}