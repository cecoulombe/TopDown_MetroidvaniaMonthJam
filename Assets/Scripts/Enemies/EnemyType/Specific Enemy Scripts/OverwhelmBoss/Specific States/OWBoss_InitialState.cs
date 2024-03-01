using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWBoss_InitialState : OWBoss_BaseState
{
    #region Single Shot Variables
    [SerializeField]
    private float singleShootCoolDown = 0.6f;

    [SerializeField]
    private float singleShootTimer = 0f;
    #endregion

    #region Spread Shot Variables
    [SerializeField]
    private float spreadShootCoolDown = 1.2f;

    [SerializeField]
    private float spreadShootTimer = 0;
    #endregion

    public override void EnterState(OWBoss_StateManager boss)
    {
        Debug.Log("hello from the half health state");
    }

    public override void UpdateState(OWBoss_StateManager boss, float currentHealth, float maxHealth)
    {
        // anything you do here will be called every frame so long as this is the current state
        spreadShootTimer += Time.deltaTime;
        singleShootTimer += Time.deltaTime;

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

        if (currentHealth <= (maxHealth * 0.6))
        {
            boss.SwitchState(boss.halfHealthState);
        }
    }

    public override void OnCollisionEnter2D(OWBoss_StateManager boss)
    {

    }
}
