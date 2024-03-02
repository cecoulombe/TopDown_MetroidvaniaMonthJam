using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AngerBoss_BaseState
{
    public abstract void EnterState(AngerBoss_StateManager boss);

    public abstract void UpdateState(AngerBoss_StateManager boss, float currentHealth, float maxHealth);

    public abstract void OnCollisionEnter2D(AngerBoss_StateManager boss);
}
