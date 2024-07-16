using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Punish: gives the player a chance to punish the enemy after they attack
 * Enters attack
 */
public class AttackPunish_MagmaBall : BaseState_MagmaBall
{
    private IEnumerator coroutine;
    StateManager_MagmaBall stateManager;

    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_MagmaBall stateManager)
    {
        Debug.Log("Punish state entry");
        coroutine = PunishCoroutine(stateManager.punishDuration);
        this.stateManager = stateManager;
        stateManager.StartCoroutine(coroutine);
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_MagmaBall stateManager)
    {
        Debug.Log("Punish state update");
    }

    //---------------------------------------------------------------------------
    // PunishCoroutine() provides a delay before switching to the attack state
    //---------------------------------------------------------------------------
    private IEnumerator PunishCoroutine(float timer)
    {
        yield return new WaitForSeconds(timer);
        stateManager.SwitchState(stateManager.movementState);
    }
}
