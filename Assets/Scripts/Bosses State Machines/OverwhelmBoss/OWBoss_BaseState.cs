using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OWBoss_BaseState
{
    public abstract void EnterState(OWBoss_StateManager boss);

    public abstract void UpdateState(OWBoss_StateManager boss, float currentHealth, float maxHealth);

    public abstract void OnCollisionEnter2D(OWBoss_StateManager boss);
}
