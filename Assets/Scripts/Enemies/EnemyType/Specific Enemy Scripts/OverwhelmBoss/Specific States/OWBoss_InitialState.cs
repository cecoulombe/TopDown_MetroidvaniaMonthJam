using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWBoss_InitialState : OWBoss_BaseState
{
    #region Variables
    [SerializeField]
    private float shootCoolDown = 300f;

    [SerializeField]
    private float shootTimer = 0f;
    #endregion

    public override void EnterState(OWBoss_StateManager boss)
    {
        Debug.Log("hello from the initial state");
    }

    public override void UpdateState(OWBoss_StateManager boss, float currentHealth, float maxHealth)
    {
        // anything you do here will be called every frame so long as this is the current state

        // for now, to keep it kinda simple, the boss will shoot during the initial state
        shootTimer += 1;

        if(shootTimer > shootCoolDown)
        {
            shootTimer = 0;
            boss.SingleBulletShot();
        }

        // once the criteria to switch states is met, use:
        if(currentHealth <= (maxHealth * 0.35))
        {
            boss.SwitchState(boss.halfHealthState);
        }
    }

    public override void OnCollisionEnter2D(OWBoss_StateManager boss)
    {

    }
}
