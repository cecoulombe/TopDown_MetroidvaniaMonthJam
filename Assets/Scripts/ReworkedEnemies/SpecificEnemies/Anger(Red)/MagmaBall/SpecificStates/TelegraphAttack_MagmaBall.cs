using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Telegraph: gives the player a very short animation to show what attack they are about to do
 * Entered from movement
 * Enters attack and death
 */
public class TelegraphAttack_MagmaBall : BaseState_MagmaBall
{
    private IEnumerator coroutine;
    StateManager_MagmaBall stateManager;

    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_MagmaBall stateManager)
    {
        Debug.Log("Telegraph state entry");
        coroutine = TelegraphCoroutine(stateManager.telegraphDuration);
        this.stateManager = stateManager;
        stateManager.StartCoroutine(coroutine);
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_MagmaBall stateManager)
    {
        Debug.Log("Telegraph state update");
    }

    //---------------------------------------------------------------------------
    // TelegraphCoroutine() provides a delay before switching to the attack state
    //---------------------------------------------------------------------------
    private IEnumerator TelegraphCoroutine(float timer)
    {
        yield return new WaitForSeconds(timer);
        stateManager.SwitchState(stateManager.attackState);
    }
}
