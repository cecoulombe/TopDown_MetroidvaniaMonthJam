using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OWBoss_StateManager : MonoBehaviour
{
    #region State Machine Variables
    OWBoss_BaseState currentState;

    public OWBoss_InitialState initialState = new OWBoss_InitialState();
    public OWBoss_HalfHealthState halfHealthState = new OWBoss_HalfHealthState();
    public OWBoss_AlmostDeadState almostDeadState = new OWBoss_AlmostDeadState();
    #endregion

    #region Other Variables
    private OWBoss_Health myHealth;

    private Transform target;
    #endregion

    #region Single Shot Variables
    [Header("Single Shot Variables")]

    [SerializeField]
    private Transform Aim;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private float fireForce = 10f;
    #endregion

    #region 8 Guns Spread Shot Variables
    [Header("Spread Shot Variables")]

    [SerializeField]
    private GunScript[] guns;
    #endregion

    #region Alternate Spread Shot Variables
    [Header("Alterante Spread Shot Variables")]

    [SerializeField]
    private GunScript[] alternateGuns;
    #endregion

    void Start()
    {
        myHealth = GetComponent<OWBoss_Health>();
        target = GameObject.Find("Player").transform;
        // set the starting state for the machine
        currentState = initialState;

        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this, myHealth.health, myHealth.maxHealth);
    }

    public void SwitchState(OWBoss_BaseState state)
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

    public void SpreadShot()
    {
        Debug.Log("shooting at the player with a spread shot");
        foreach (GunScript gun in guns)
        {
            gun.centerLocation = this.transform.position;
            gun.Shoot();
        }
    }

    public void AlternateSpreadShot()
    {
        Debug.Log("shooting at the player with the alternate spread shot");
        foreach (GunScript gun in alternateGuns)
        {
            gun.centerLocation = this.transform.position;
            gun.Shoot();
        }
    }
}
