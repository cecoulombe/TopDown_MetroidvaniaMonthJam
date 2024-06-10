using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Parent base state abstract which will be inherited by state parents and their children
 *
 */

public abstract class BaseState_FlameBoy
{
    public abstract void EnterState(StateManager_FlameBoy stateManager);

    public abstract void UpdateState(StateManager_FlameBoy stateManager);

}
