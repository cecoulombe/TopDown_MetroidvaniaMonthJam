using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSBoss_ShootingState : SSBoss_BaseState
{
    #region Single Shot Variables
    private float singleShootCoolDown = 0.6f;

    private float singleShootTimer = 0f;
    #endregion  

    public override void EnterState(SSBoss_StateManager boss)
    {
        Debug.Log("hello from the Shooting state");
        singleShootCoolDown = 0.6f;
    }

    public override void UpdateState(SSBoss_StateManager boss, Transform targetLocation, float health, float maxHealth)
    {
        // what happens here


        if (singleShootCoolDown <= singleShootTimer)
        {
            boss.SingleBulletShot();
            boss.SwitchState(boss.initialState);
        }
        // transition(s) to other state(s)
    }

    public override void OnCollisionEnter2D(SSBoss_StateManager boss)
    {

    }
}
