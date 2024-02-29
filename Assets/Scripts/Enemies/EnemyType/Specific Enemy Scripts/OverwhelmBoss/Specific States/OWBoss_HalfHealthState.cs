using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWBoss_HalfHealthState : OWBoss_BaseState
{
    #region Single Shot Variables
    [SerializeField]
    private float singleShootCoolDown = 300f;

    [SerializeField]
    private float singleShootTimer = 0f;
    #endregion

    #region Spread Shot Variables
    [SerializeField]
    private float spreadShootCoolDown = 500f;

    [SerializeField]
    private float spreadShootTimer = 400f;
    #endregion

    public override void EnterState(OWBoss_StateManager boss)
    {
        Debug.Log("hello from the half health state");
    }

    public override void UpdateState(OWBoss_StateManager boss, float currentHealth, float maxHealth)
    {
        // anything you do here will be called every frame so long as this is the current state
        spreadShootTimer += 1;
        singleShootTimer += 1;

        if (spreadShootTimer > spreadShootCoolDown)
        {
            spreadShootTimer = 0;
            boss.SpreadShot();
        }

        if (singleShootTimer > singleShootCoolDown)
        {
            singleShootTimer = 0;
            boss.SingleBulletShot();
        }
        // once the criteria to switch states is met, use:

        if (currentHealth <= (maxHealth * 0.1))
        {
            boss.SwitchState(boss.almostDeadState);
        }
    }

    public override void OnCollisionEnter2D(OWBoss_StateManager boss)
    {

    }
}
