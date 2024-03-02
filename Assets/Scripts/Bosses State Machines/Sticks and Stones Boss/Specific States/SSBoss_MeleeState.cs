using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSBoss_MeleeState : SSBoss_BaseState
{
    #region Melee Variables
    private float meleeCoolDown = 1f;

    private float meleeAttackTimer = 0f;
    #endregion 
    public override void EnterState(SSBoss_StateManager boss)
    {
        Debug.Log("hello from the melee state");
    }

    public override void UpdateState(SSBoss_StateManager boss, Transform targetLocation, float health, float maxHealth)
    {
        // what happens here
        if (meleeCoolDown <= meleeAttackTimer)
        {
            boss.MeleeAttack();
            boss.SwitchState(boss.initialState);
        }
        // transition(s) to other state(s)
    }

    public override void OnCollisionEnter2D(SSBoss_StateManager boss)
    {

    }
}
