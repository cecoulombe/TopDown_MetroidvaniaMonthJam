using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Healing: slowly recovers health while standing still and not attacking
 * Entered from movement
 * Enters movement and death
 */
public class Healing_Parent : BaseState_Parent
{
    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_Parent stateManager)
    {
        Debug.Log("Healing state entry");
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_Parent stateManager)
    {
        Debug.Log("Healing state update");
    }

}