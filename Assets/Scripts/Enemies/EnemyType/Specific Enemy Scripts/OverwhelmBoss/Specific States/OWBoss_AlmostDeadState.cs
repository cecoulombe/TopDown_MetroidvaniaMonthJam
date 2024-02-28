using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWBoss_AlmostDeadState : OWBoss_BaseState
{
    #region Single Shot Variables
    [SerializeField]
    private float singleShootCoolDown = 500f;

    [SerializeField]
    private float singleShootTimer = 0f;
    #endregion

    #region Spread Shot Variables
    [SerializeField]
    private float spreadShootCoolDown = 800f;

    [SerializeField]
    private float spreadShootTimer = 400f;
    #endregion

    #region Alternate Spread Shot Variables
    [SerializeField]
    private float alternateSpreadShootCoolDown = 500f;

    [SerializeField]
    private float alternateSpreadShootTimer = 250f;
    #endregion

    public override void EnterState(OWBoss_StateManager boss)
    {
        Debug.Log("hello from the almost dead state");
    }

    public override void UpdateState(OWBoss_StateManager boss, float currentHealth, float maxHealth)
    {
        // anything you do here will be called every frame so long as this is the current state
        spreadShootTimer += 1;
        alternateSpreadShootTimer += 1;
        singleShootTimer += 1;

        if (spreadShootTimer > spreadShootCoolDown)
        {
            spreadShootTimer = 0;
            boss.SpreadShot();
        }

        if (alternateSpreadShootTimer > alternateSpreadShootCoolDown)
        {
            alternateSpreadShootTimer = 0;
            boss.AlternateSpreadShot();
        }

        if (singleShootTimer > singleShootCoolDown)
        {
            singleShootTimer = 0;
            boss.SingleBulletShot();
        }
        // anything you do here will be called every frame so long as this is the current state
    }

    public override void OnCollisionEnter2D(OWBoss_StateManager boss)
    {

    }
}
