using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Parent base state abstract which will be inherited by state parents and their children
 *
 */

public abstract class BaseState_BallOfAnger
{
    public abstract void EnterState(StateManager_BallOfAnger stateManager);

    public abstract void UpdateState(StateManager_BallOfAnger stateManager);

}
