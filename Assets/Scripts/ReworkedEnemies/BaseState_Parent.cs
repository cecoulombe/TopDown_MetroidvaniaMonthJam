using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Parent base state abstract which will be inherited by state parents and their children
 *
 */

public abstract class BaseState_Parent
{
    public abstract void EnterState(StateManager_Parent stateManager);

    public abstract void UpdateState(StateManager_Parent stateManager);

}