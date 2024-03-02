using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSBoss_HealingState : SSBoss_BaseState
{
    private float healingCoolCounter;

    public override void EnterState(SSBoss_StateManager boss)
    {
        Debug.Log("hello from the healing state");
    }

    public override void UpdateState(SSBoss_StateManager boss, Transform targetLocation, float health, float maxHealth)
    {
        boss.Healing();
        // transition(s) to other state(s)
        if(health >= maxHealth)
        {
            boss.SwitchState(boss.initialState);
        }
    }

    public override void OnCollisionEnter2D(SSBoss_StateManager boss)
    {

    }
}
