using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSBoss_ShootingState : SSBoss_BaseState
{
    public override void EnterState(SSBoss_StateManager boss)
    {
        Debug.Log("hello from the Shooting state");
    }

    public override void UpdateState(SSBoss_StateManager boss, Transform targetLocation, float health, float maxHealth)
    {
        // what happens here
        

        if (boss.singleShootCoolDown <= 0)
        {
            boss.SingleBulletShot();
            boss.SwitchState(boss.initialState);
        }
        else
        {
            boss.SwitchState(boss.initialState);
        }
        // transition(s) to other state(s)
    }

    public override void OnCollisionEnter2D(SSBoss_StateManager boss)
    {

    }
}
