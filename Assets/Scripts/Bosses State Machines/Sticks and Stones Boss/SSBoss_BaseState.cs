using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SSBoss_BaseState
{
    public abstract void EnterState(SSBoss_StateManager boss);

    public abstract void UpdateState(SSBoss_StateManager boss, Transform targetLocation, float health, float maxHealth);

    public abstract void OnCollisionEnter2D(SSBoss_StateManager boss);
}
