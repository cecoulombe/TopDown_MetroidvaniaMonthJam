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
    float shotCooldown = 0.25f;
    float shotTimer = 0.5f;

    StateManager_MagmaBall stateManager;

    //---------------------------------------------------------------------------
    // EnterState(stateManager) provide the first frame instructions for this state
    //---------------------------------------------------------------------------
    public override void EnterState(StateManager_MagmaBall stateManager)
    {
        Debug.Log("Attack state entry");
        // do the attack and the anim while waiting for the update to run until after the anim has run
        
    }

    //---------------------------------------------------------------------------
    // UpdateState(stateManager) provide the frame-by-frame actions that are to be executed while in this state
    //---------------------------------------------------------------------------
    public override void UpdateState(StateManager_MagmaBall stateManager)
    {
        Debug.Log("Attack state update");
        this.stateManager = stateManager;
        RangedAttack();
    }

    //---------------------------------------------------------------------------
    // RangedAttack() fires bullets and counts how many shots have been fired
    //---------------------------------------------------------------------------
    private void RangedAttack()
    {
        int count = 0;

        while(count < 3)
        {
            shotTimer += Time.deltaTime;

            if(shotTimer > shotCooldown)
            {
                shotTimer = 0;
                stateManager.FireBullet();
                count++;
            }
        }
        stateManager.SwitchState(stateManager.punishState);
    }
}