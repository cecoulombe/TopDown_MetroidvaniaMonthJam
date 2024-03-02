using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSBoss_StateManager : MonoBehaviour
{
    #region State Machine Variables
    SSBoss_BaseState currentState;

    public SSBoss_InitialState initialState = new SSBoss_InitialState();
    public SSBoss_MeleeState meleeState = new SSBoss_MeleeState();
    public SSBoss_ShootingState shootingState = new SSBoss_ShootingState();
    public SSBoss_HealingState healingState = new SSBoss_HealingState();
    #endregion

    #region Other Variables
    private SSBoss_Health myHealth;

    private Transform target;

    private Collider2D col;
    private SpriteRenderer sprite;
    #endregion

    #region Single Shot Variables
    [Header("Single Shot Variables")]

    [SerializeField]
    private Transform Aim;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float fireForce = 8f;

    [SerializeField]
    public float rangedAttackRangeMin;

    [SerializeField]
    public float rangedAttackRangeMax;
    #endregion

    #region Healing Variables
    [Header("Healing variables")]
    [SerializeField]
    private float healingAmount;

    [SerializeField]
    public GameObject healingAnim;

    #endregion

    #region Melee Variables
    [Header("Melee Variables")]
    public GameObject Melee;

    [SerializeField]
    public float meleeAttackRange;
    #endregion

    void Start()
    {
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        myHealth = GetComponent<SSBoss_Health>();
        target = GameObject.Find("Player").transform;
        // set the starting state for the machine
        currentState = initialState;

        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this, target, myHealth.health, myHealth.maxHealth);
    }

    public void SwitchState(SSBoss_BaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void SingleBulletShot()
    {
        // Instantiate the bullet
        GameObject intBullet = Instantiate(bullet, Aim.position, target.rotation);

        // Calculate the direction towards the player
        Vector2 direction = (target.position - intBullet.transform.position).normalized;

        // Set the bullet's velocity towards the player
        intBullet.GetComponent<Rigidbody2D>().velocity = direction * fireForce;

        // Optionally, you can set the rotation of the bullet based on the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        intBullet.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        Destroy(intBullet, 4f);
    }

    public void MeleeAttack()
    {

    }

    public void Healing()
    {
        myHealth.health += Time.deltaTime * healingAmount;
    }
}
