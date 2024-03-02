using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerBoss_MovementState : AngerBoss_BaseState
{

    [Header("Minion Spawn Variables")]
    private float minionSpawnTimer = 3f;

    public override void EnterState(AngerBoss_StateManager boss)
    {
        Debug.Log("hello from the movement state");
    }

    public override void UpdateState(AngerBoss_StateManager boss, float currentHealth, float maxHealth)
    {
        // anything you do here will be called every frame so long as this is the current state


        boss.Movement();

        // once the criteria to switch states is met, use:


        minionSpawnTimer -= Time.deltaTime;

        if (minionSpawnTimer <= 0)
        {
            //spawn a flame boy minion
            boss.SwitchState(boss.minionSpawnState);

            minionSpawnTimer = Random.Range(1f, 4.5f);
        }
    }

    public override void OnCollisionEnter2D(AngerBoss_StateManager boss)
    {

    }
}
