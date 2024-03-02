using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerBoss_MinionSpawnState : AngerBoss_BaseState
{
    public override void EnterState(AngerBoss_StateManager boss)
    {
        Debug.Log("hello from the minion spawn state");
    }

    public override void UpdateState(AngerBoss_StateManager boss, float currentHealth, float maxHealth)
    {
        boss.SpawnMinions();

        boss.SwitchState(boss.movementState);
    }

    public override void OnCollisionEnter2D(AngerBoss_StateManager boss)
    {

    }
}
