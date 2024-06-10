using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Parent state manager which will be used to make more specific state managers for the majority of enemy types.
 * Each state manager will include references to rigid bodies, collisions, and sprite renderers.
 * Controls the frame-by-frame actions through the update method which will depend on what the current state is.
 */

public class StateManager_FlameBoy : MonoBehaviour
{
    // State Machine Variables
    BaseState_FlameBoy currentState;

    public Idle_FlameBoy idleState = new Idle_FlameBoy();
    public Movement_FlameBoy movementState = new Movement_FlameBoy();
    // public TelegraphAttack_Parent telegraphState = new TelegraphAttack_Parent();
    // public Attack_Parent attackState = new Attack_Parent();
    // public AttackPunish_Parent punishState = new AttackPunish_Parent();
    // public Healing_Parent healingState = new Healing_Parent();
    public Death_FlameBoy deathState = new Death_FlameBoy();
    public Drops_FlameBoy dropsState = new Drops_FlameBoy();

    // list out the various states that can be called (this might be left to the children to do for their own enemy type, but list out the guaranteed ones (i.e. death and drops))


    // Object variables

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sprite;  // this may need to change depending on how you set up anims

    private int myHealth;
    private int maxHealth;

    //---------------------------------------------------------------------------
    //Start() initialize the variables for the rigidbody, collider, and sprite.
    //Make the first call to the starting state
    //---------------------------------------------------------------------------
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        myHealth = maxHealth;

        currentState = idleState;
        currentState.EnterState(this);
    }

    //---------------------------------------------------------------------------
    //Update() frame-by-frame update based on the current state while ensuring that the enemy is alive
    //---------------------------------------------------------------------------
    void Update()
    {
        if(myHealth <= 0 && currentState != deathState && currentState != dropsState)
        {
            SwitchState(deathState);
        }

        currentState.UpdateState(this);
    }

    //---------------------------------------------------------------------------
    //SwitchState(newState) changes the current state and executes the new state
    //---------------------------------------------------------------------------
    public void SwitchState(BaseState_FlameBoy newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }


    //---------------------------------------------------------------------------
    // IsHealthFull() returns true if the enemy is at full health
    //---------------------------------------------------------------------------
    public bool IsHealthFull()
    {
        return myHealth >= maxHealth;
    }
}
