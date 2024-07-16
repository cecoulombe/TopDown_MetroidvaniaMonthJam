using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* BallOfAnger is a melee enemy type in anger. He can either be static or pathwalking when idle
 * and will follow the player and hit them with his melee weapon when aggrod.
 */

public class StateManager_BallOfAnger : MonoBehaviour
{
    // State Machine Variables
    BaseState_BallOfAnger currentState;

    public Idle_BallOfAnger idleState = new Idle_BallOfAnger();
    public Movement_BallOfAnger movementState = new Movement_BallOfAnger();
     public TelegraphAttack_BallOfAnger telegraphState = new TelegraphAttack_BallOfAnger();
    public Attack_BallOfAnger attackState = new Attack_BallOfAnger();
    public AttackPunish_BallOfAnger punishState = new AttackPunish_BallOfAnger();
    // public Healing_Parent healingState = new Healing_Parent();
    public Death_BallOfAnger deathState = new Death_BallOfAnger();
    public Drops_BallOfAnger dropsState = new Drops_BallOfAnger();

    // list out the various states that can be called (this might be left to the children to do for their own enemy type, but list out the guaranteed ones (i.e. death and drops))


    // Object variables

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sprite;  // this may need to change depending on how you set up anims

    private EnemyHealth_Manager healthManager;

    private Transform player;

    // Health and ammo drops
    [SerializeField]
    private GameObject bigHealthDrop;
    [SerializeField]
    private float bigHealthChance;
    [SerializeField]
    private GameObject smallHealthDrop;
    [SerializeField]
    private float smallHealthChance;

    [SerializeField]
    private GameObject bigAmmoDrop;
    [SerializeField]
    private float bigAmmoChance;
    [SerializeField]
    private GameObject smallAmmoDrop;
    [SerializeField]
    private float smallAmmoChance;

    // Idle movement type
    [SerializeField]
    private bool isStaticIdle;

    [SerializeField]
    private Transform Aim;

    [SerializeField]
    private Transform pointA;
    [SerializeField]
    private Transform pointB;
    private bool switching = false;
    private Transform walkTarget;

    [SerializeField]
    private float moveSpeedIdle;

    // Aggro move variables
    [SerializeField]
    private float moveSpeedAggro;

    // Aggro range
    [SerializeField]
    private float aggroRange;

    // Attack variables
    [SerializeField]
    public float telegraphDuration;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private GameObject Melee;

    [SerializeField]
    public float punishDuration;

    [SerializeField]
    private float contactDamage;

    //---------------------------------------------------------------------------
    //Start() initialize the variables for the rigidbody, collider, and sprite.
    //Make the first call to the starting state
    //---------------------------------------------------------------------------
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        player = GameObject.Find("Player").transform;

        healthManager = GetComponent<EnemyHealth_Manager>();

        currentState = idleState;
        currentState.EnterState(this);
    }

    //---------------------------------------------------------------------------
    //Update() frame-by-frame update based on the current state while ensuring that the enemy is alive
    //---------------------------------------------------------------------------
    void Update()
    {
        Aim.rotation = Quaternion.LookRotation(Vector3.forward, (transform.position - player.position).normalized);

        if (healthManager.GetCurrentHealth() <= 0 && currentState != deathState && currentState != dropsState)
        {
            SwitchState(deathState);
        }

        currentState.UpdateState(this);
    }

    //---------------------------------------------------------------------------
    //SwitchState(newState) changes the current state and executes the new state
    //---------------------------------------------------------------------------
    public void SwitchState(BaseState_BallOfAnger newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    //---------------------------------------------------------------------------
    // IsHealthFull() returns true if the enemy is at full health
    //---------------------------------------------------------------------------
    public bool IsHealthFull()
    {
        return healthManager.GetCurrentHealth() >= healthManager.GetMaxHealth();
    }

    //---------------------------------------------------------------------------
    // GetIdleMovement() returns if the enemy is static or moving when idle
    //---------------------------------------------------------------------------
    public bool GetIdleMovement()
    {
        return isStaticIdle;
    }

    //---------------------------------------------------------------------------
    // PathWalking() walks the enemy from point A to point B when idle
    //---------------------------------------------------------------------------
    public void PathWalking()
    {
        if(!switching)
        {
            walkTarget = pointB;
        }
        else if(switching)
        {
            walkTarget = pointA;
        }

        if(transform.position == pointB.position)
        {
            switching = true;
        }
        else if(transform.position == pointA.position)
        {
            switching = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, walkTarget.position, moveSpeedIdle * Time.deltaTime);
    }
    

    //---------------------------------------------------------------------------
    // PlayerInRange() returns if the player is within the aggro range or not
    //---------------------------------------------------------------------------
    public bool PlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= aggroRange;
    }

    //---------------------------------------------------------------------------
    // InAttackRange() returns if the player is within the attack range or not
    //---------------------------------------------------------------------------
    public bool InAttackRange()
    {
        return Vector3.Distance(transform.position, player.position) <= attackRange;
    }

    //---------------------------------------------------------------------------
    // FollowPlayer() has the enemy walk towards the player at a consistent speed
    //---------------------------------------------------------------------------
    public void FollowPlayer()
    {
        if (healthManager.knockBackCounter <= 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeedAggro * Time.deltaTime);
        }
    }

    //---------------------------------------------------------------------------
    // TurnMeleeOn() turns on the melee weapon anim and hitbox
    //---------------------------------------------------------------------------
    public void TurnMeleeOn()
    {
        Melee.SetActive(true);
    }

    //---------------------------------------------------------------------------
    // TurnMeleeOff() turns off the melee weapon anim and hitbox
    //---------------------------------------------------------------------------
    public void TurnMeleeOff()
    {
        Melee.SetActive(false);
    }

    //---------------------------------------------------------------------------
    // DisableSprite() turns off the enemy sprite and hit box when they are dead
    //---------------------------------------------------------------------------
    public void DisableSprite()
    {
        //sprite.enabled = false;
        //col.enabled = false;

        gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------
    // HealthDrops() spreads the heal drops around where the enemy sprite was
    //---------------------------------------------------------------------------
    public void HealthDrops()
    {
        float randNum = Random.Range(0f, 10f) / 10f * 100f;

        if(randNum <= bigHealthChance)
        {
            Vector3 dropsPos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
            Instantiate(bigHealthDrop, dropsPos, transform.rotation);
        }
        else if (randNum > bigHealthChance && randNum <= smallHealthChance)
        {
            Vector3 dropsPos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
            Instantiate(smallHealthDrop, dropsPos, transform.rotation);
        }
    }

    //---------------------------------------------------------------------------
    // AmmoDrops() spreads the ammo drops around where the enemy sprite was
    //---------------------------------------------------------------------------
    public void AmmoDrops()
    {
        float randNum = Random.Range(0f, 10f) / 10f * 100f;

        if (randNum <= bigAmmoChance)
        {
            Vector3 dropsPos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
            Instantiate(bigAmmoDrop, dropsPos, transform.rotation);
        }
        else if (randNum > bigAmmoChance && randNum <= smallHealthChance)
        {
            Vector3 dropsPos = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
            Instantiate(smallAmmoDrop, dropsPos, transform.rotation);
        }
    }


    //---------------------------------------------------------------------------
    // OnCollisionEnter2D() collision box for player to take contact damage
    //---------------------------------------------------------------------------
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerController_TopDown player = collision.gameObject.GetComponent<PlayerController_TopDown>();

            player.knockBackCounter = player.knockBackTotalTime;

            if(collision.transform.position.x <= transform.position.x)
            {
                player.knockFromRight = true;
            }
            if(collision.transform.position.x >= transform.position.x)
            {
                player.knockFromRight = false;
            }

            player.TakeDamage(contactDamage);
        }
    }

}
