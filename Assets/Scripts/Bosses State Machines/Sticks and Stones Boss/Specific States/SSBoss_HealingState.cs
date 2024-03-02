using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSBoss_HealingState : SSBoss_BaseState
{
    private float healingCountdown;

    public override void EnterState(SSBoss_StateManager boss)
    {
        Debug.Log("hello from the healing state");
        healingCountdown = 2f;
    }

    public override void UpdateState(SSBoss_StateManager boss, Transform targetLocation, float health, float maxHealth)
    {
        healingCountdown -= Time.deltaTime;

        if (healingCountdown > 0 && health < maxHealth)
        {
            boss.healingAnim.SetActive(true);
            boss.Healing();
        }
        else if(healingCountdown <= 0 || health >= maxHealth)
        {
            boss.healingCoolCounter = boss.healingCooldown + Random.Range(1f, 10f);
            boss.healingAnim.SetActive(false);
            boss.SwitchState(boss.initialState);
        }
    }

    public override void OnCollisionEnter2D(SSBoss_StateManager boss)
    {

    }
}
