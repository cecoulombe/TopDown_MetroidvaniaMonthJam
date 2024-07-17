using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Attack: attacks the player
 * Entered from telegraph
 * Enters attackPunish and death
 */
public class Attack_MagmaBall : BaseState_MagmaBall
{
    // variables
    float shotCooldown = 0.5f;
    float shotTimer = 1.5f;

    StateManager_MagmaBall stateManager;

    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_MagmaBall stateManager)
    {
        Debug.Log("Attack state entry");
        // do the attack and the anim while waiting for the update to run until after the anim has run
        this.stateManager = stateManager;
        stateManager.StartCoroutine(RangedAttack());
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_MagmaBall stateManager)
    {
        Debug.Log("Attack state update");
    }

    //---------------------------------------------------------------------------
    // RangedAttack() fires bullets and counts how many shots have been fired
    //---------------------------------------------------------------------------
    private IEnumerator RangedAttack()
    {
        int count = 0;
        float shotCooldown = 0.5f; // cooldown between shots

        while (count < 3)
        {
            stateManager.FireBullet();
            count++;
            Debug.Log("shot fired count: " + count);
            yield return new WaitForSeconds(shotCooldown); // wait for the cooldown before firing the next shot
        }

        stateManager.SwitchState(stateManager.punishState); // switch to punish state after firing 3 shots
    }
}