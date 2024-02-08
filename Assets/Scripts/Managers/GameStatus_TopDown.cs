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
    [SerializeField]
    protected int deathCounter = 0;
    [SerializeField]
    protected float money = 0f;
    [SerializeField]
    protected float ammoCount;
    [SerializeField]
    protected float maxAmmo;

    [SerializeField]
    protected string previousRoom;

    [SerializeField]
    protected bool gamePaused;

    [Header("Player state bools")]
    [SerializeField]
    protected bool hasDash;
    [SerializeField]
    protected bool hasInvincibleDash;
    [SerializeField]
    protected bool hasMelee;
    [SerializeField]
    protected bool hasRanged;

    #endregion

    #region Enemy/boss gates, doors, chests, breakable walls, &c. tracking
    // format for each gate is "roomName_enemyGateOpen" or "roomName_bossGateOpen", "roomName_lockedDoor", "roomName_Chest"
        // adding open to the end of the names for clarity because the bools are simply indicating if the associated game object has been opened or not
    // I think i will need to manually add in each enemy gate for each room, and then the generic script will affect if they are set or not

    [Header("Gates/doors/chests tracking")]
    [SerializeField]
    protected bool room1_enemyGateOpen;
    [SerializeField]
    protected bool room2_chestOpen;
    [SerializeField]
    protected bool room2_HealthIncreaseTaken;
    [SerializeField]
    protected bool room2_AmmoIncreaseTaken;
    [SerializeField]
    protected bool room2_wallOpen;
    #endregion

    // making a class which can keep track of certain game states (i.e. what abilities the player has, how many times they have died, if a gate has been opened at some point?)

    // keep track of player state (maybe using an enum? take a look at that video after sorting this out)

    // track individual room values (i.e. is a room's chest has been opened or if the enemy gate has been opened)
    private void Awake()
    {
        previousRoom = "spawn";
    }


    private void Start()
    {
        #region Don't destroy on load/check for other instances
        if (instance != null)    // check if someone else is already this game object
        {
            Destroy(gameObject);
            return;
        }

        health = maxHealth;
        ammoCount = maxAmmo;
        instance = this;
        GameObject.DontDestroyOnLoad(gameObject);   // become immortal

        // keep this variables for now, but can change them later on using external scripts
        SetHasDash(false);
        SetHasMelee(false);
        SetHasRanged(false);
        SetHasInvincibleDash(false);

        //hasDash = false;

        #endregion
    }

    private void Update()
    {
        string currentRoom = SceneManager.GetActiveScene().name;
        GetGateState(currentRoom);
    }

    private void OnDestroy()
    {
        Debug.Log("game status destroyed");
    }

    #region Pause Menu
    public void SetGamePaused(bool pausedStatus)
    {
        gamePaused = pausedStatus;
    }

    public bool GetGamePaused()
    {
        return gamePaused;
    }
    #endregion

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

    public void AddMaxHealth(float healthIncrease)
    {
        maxHealth += healthIncrease;
    }

    public void LoseHealth(float damageAmount)
    {
        health -= damageAmount;
    }

    public void AddDeath()
    {
        deathCounter += 1;
    }
    #endregion

    #region Ammo counter
    public float GetAmmo()
    {
        return ammoCount;
    }

    public float GetMaxAmmo()
    {
        return maxAmmo;
    }

    public void AddAmmo(float bulletsRefilled)
    {
        ammoCount += bulletsRefilled;
        ammoCount = Mathf.Clamp(ammoCount, 0, maxAmmo);
    }


    public void ShootBullets()
    {
        ammoCount -= 1;
    }

    public void AddMaxAmmo(float ammoIncrease)
    {
        maxAmmo += ammoIncrease;
    }
    #endregion

    #region Money
    public void AddMoney(float moneyEarned)
    {
        money += moneyEarned;
    }

    public void LoseMoney(float cost)
    {
        money -= cost;
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

    public void SetHasInvincibleDash(bool iDash)
    {
        hasInvincibleDash = iDash;
    }

    public bool HasInvincibleDash()
    {
        return hasInvincibleDash;
    }

    public void SetHasMelee(bool melee)
    {
        hasMelee = melee;
    }

    public bool HasMelee()
    {
        return hasMelee;
    }

    public void SetHasRanged(bool ranged)
    {
        hasRanged = ranged;
    }

    public bool HasRanged()
    {
        return hasRanged;
    }
    #endregion

    #region Room Management
    public string GetCurrentRoom()
    {
       return SceneManager.GetActiveScene().name;
    }

    // these two strings return the same room; however, one will be used to store the name of the previous room that we were in to determine which door to spawn at on a transition
    public void SetPreviousRoom(string roomBeforeTransition)
    {
        previousRoom = roomBeforeTransition;
    }

    public string GetPreviousRoom()
    {
        return previousRoom;
    }
    #endregion

    #region Check and change the state of a door/chest
    // take in the name of the thing to be checked, then return if it is true of false
    #region Gates
    public bool GetGateState(string roomName)
    {
        if (roomName == "Room1")
        {
            Debug.Log(room1_enemyGateOpen);
            return room1_enemyGateOpen;
        }
        else
        {
            Debug.Log("cannot find the current room, returning true");
            return true;
        }
    }

    // do the same thing for set, but take in the name of the thing set it to true
    public void SetGateState(string roomName)
    {
        if(roomName == "Room1")
        {
            Debug.Log("Opening the gate in room 1");
            room1_enemyGateOpen = true;
        }
    }
    #endregion

    #region Chests
    public bool GetChestState(string roomName)
    {
        if (roomName == "Room2")
        {
            Debug.Log("from game status" + room2_chestOpen);
            return room2_chestOpen;
        }
        else
        {
            Debug.Log("cannot find the current room, returning true");
            return true;
        }
    }

    public void SetChestState(string roomName)
    {
        if (roomName == "Room2")
        {
            room2_chestOpen = true;
        }
    }
    #endregion

    #region Breakable Walls
    public bool GetWallState(string roomName)
    {
        if (roomName == "Room2")
        {
            Debug.Log("from game status" + room2_wallOpen);
            return room2_wallOpen;
        }
        else
        {
            Debug.Log("cannot find the current room, returning true");
            return true;
        }
    }

    public void SetWallState(string roomName)
    {
        if (roomName == "Room2")
        {
            room2_wallOpen = true;
        }
    }
    #endregion

    //#region Doors
    //public bool GetDoorsState(string roomName)
    //{
    //    //if (roomName == "Room2")
    //    //{
    //    //    return false;
    //    //}
    //    //else
    //    //{
    //    //    return false
    //    //}
    //    Debug.Log("there are currently no doors, calling this was a probably mistake");
    //    return false;
    //}

    //public void SetDoorsState(string roomName)
    //{
    //    //if (roomName == "Room2")
    //    //{
    //    //    room2_chestOpen = true;
    //    //}
    //    Debug.Log("there are currently no doors, calling this was a probably mistake");
    //}
    //#endregion

    // this structure can be used for any of the permanent pick ups/upgrades including the ability to dash and attack
    #region Permanent Upgrades
    public bool GetUpgradeState(string roomName, string pickup)
    {
        if (roomName == "Room2")
        {
            if(pickup == "Health")
            {
                Debug.Log("room2_HealthIncrease" + room2_HealthIncreaseTaken);
                return room2_HealthIncreaseTaken;
            }
            else if (pickup == "Ammo")
            {
                Debug.Log("room2_AmmoIncrease" + room2_AmmoIncreaseTaken);
                return room2_AmmoIncreaseTaken;
            }
        }

        if(pickup == "dash")
        {
            Debug.Log("player has dash? ");
            return hasDash;
        }
        else if (pickup == "iDash")
        {
            Debug.Log("player has iDash? ");
            return hasInvincibleDash;
        }
        else if (pickup == "melee")
        {
            Debug.Log("player has melee attack? ");
            return hasMelee;
        }
        else if (pickup == "ranged")
        {
            Debug.Log("player has ranged attack? ");
            return hasRanged;
        }
        else
        {
            return false;
        }
    }

    // do the same thing for set, but take in the name of the thing set it to true
    public void SetUpgradeState(string roomName, string pickup)
    {

        if (roomName == "Room2")
        {
            if (pickup == "Health")
            {
                Debug.Log("room2_HealthIncrease has been picked up");
                room2_HealthIncreaseTaken = true;
            }
            else if (pickup == "Ammo")
            {
                Debug.Log("room2_AmmoIncrease has been picked up");
                room2_AmmoIncreaseTaken = true;
            }
        }

        if(pickup == "dash")
        {
            Debug.Log("giving the player dash");
            SetHasDash(true);
            //hasDash = true;
        }
        else if(pickup == "iDash")
        {
            Debug.Log("giving the player invincible dash");
            SetHasInvincibleDash(true);
            //hasDash = true;
        }
        else if (pickup == "melee")
        {
            Debug.Log("giving the player ranged attack");
            SetHasMelee(true);
            //hasDash = true;
        }
        else if (pickup == "ranged")
        {
            Debug.Log("giving the player ranged attack");
            SetHasRanged(true);
            //hasDash = true;
        }
    }
    #endregion
    #endregion

    public static GameStatus GetInstance()
    {
        return instance;
    }
}
