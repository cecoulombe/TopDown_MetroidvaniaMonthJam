using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour
{
    // make sure this is the only instance of the game status, even though it isn't destroying on load
    static GameStatus instance;

    private HealthBarManager HealthManager;

    // store some player variables that will need to persist between loads
    #region Player variables
    [Header("Player health stats")]
    [SerializeField]
    protected float health;
    [SerializeField]
    protected float maxHealth;

    [Header("Player state bools")]
    [SerializeField]
    protected bool hasDash;
    [SerializeField]
    protected bool canMelee;
    [SerializeField]
    protected bool canRanged;

    #endregion

    #region Enemy/boss gates, doors, chests, &c. tracking
    // format for each gate is "roomName_enemyGate" or "roomName_bossGate", "roomName_lockedDoor", "roomName_Chest"
    // I think i will need to manually add in each enemy gate for each room, and then the generic script will affect if they are set or not
    [Header("Gates/Doors/Chests tracking")]
    [SerializeField]
    protected bool Room1_EnemyGate;
    #endregion

    // making a class which can keep track of certain game states (i.e. what abilities the player has, how many times they have died, if a gate has been opened at some point?)

    // keep track of player state (maybe using an enum? take a look at that video after sorting this out)

    // track individual room values (i.e. is a room's chest has been opened or if the enemy gate has been opened)

    private void Start()
    {
        #region Don't destroy on load/check for other instances
        if (instance != null)    // check if someone else is already this game object
        {
            Destroy(gameObject);
            return;
        }

        health = maxHealth;
        instance = this;
        GameObject.DontDestroyOnLoad(gameObject);   // become immortal

        // keep this variables for now, but can change them later on using external scripts
        SetHasDash(true);
        SetCanMelee(true);
        SetCanRanged(true);
        #endregion
    }

    private void OnDestroy()
    {
        Debug.Log("game status destroyed");
    }

    #region Health and Damage Functions
    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void AddHealth(float healthAmount)
    {
        health += healthAmount;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public void LoseHealth(float damageAmount)
    {
        health -= damageAmount;
    }
    #endregion

    #region Ability Checks
    public void SetHasDash(bool dash)
    {
        hasDash = dash;
    }
    public bool HasDash()
    {
        return hasDash;
    }

    public void SetCanMelee(bool melee)
    {
        canMelee = melee;
    }
    public bool CanMelee()
    {
        return canMelee;
    }

    public void SetCanRanged(bool ranged)
    {
        canRanged = ranged;
    }
    public bool CanRanged()
    {
        return canRanged;
    }
    #endregion

    #region Check and change the state of a door/chest
    // take in the name of the thing to be checked, then return if it is true of false

    public bool GetGateState(GameObject gateName)
    {
        Room1_EnemyGate = gateName.activeInHierarchy;
        return gateName.activeInHierarchy;
    }

    // do the same thing for set, but take in the name of the thing set it to true
    public void SetGateState(GameObject gateName, bool deactivate)
    {
        gateName.SetActive(deactivate);
        Room1_EnemyGate = false;
    }
    #endregion

    public static GameStatus GetInstance()
    {
        return instance;
    }
}
