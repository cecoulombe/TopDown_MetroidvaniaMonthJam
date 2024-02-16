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
    protected int deathCounter;
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
    [Header("Enemy Gates")]
    [SerializeField]
    protected bool room1_enemyGateOpen;

    [Header("Chests")]
    [SerializeField]
    protected bool room2_chestOpen;

    [Header("Permanent Increases")]
    [SerializeField]
    protected bool room1_HealthIncreaseTaken;
    [SerializeField]
    protected bool room1_AmmoIncreaseTaken;
    [SerializeField]
    protected bool room2_HealthIncreaseTaken;
    [SerializeField]
    protected bool room2_AmmoIncreaseTaken;

    [Header("Breakable Walls")]
    [SerializeField]
    protected bool room2_wallOpen;

    [Header("Hidden Rooms")]
    [SerializeField]
    protected bool room1_HiddenOpen;
    #endregion

    #region Player Prefs Variables
    // I think I need to use player prefs to save information between launches of the game? so I might have the prefs save between rooms for the time being and then eventually save when benching and load when launching the game?

    //private int saveFile;   // this is the number of the save file, which will allow for there to be multiple files. For now, just have 1 so slot == 1

    // things to player pref: max health, max ammo, death counter, last save room, dash, idash, range, melee, any doors/gates/chests/&c.

    [Header("Player health stats")]
    private bool resetPrefs = false;

    protected string saveRoom;
    #endregion

    // making a class which can keep track of certain game states (i.e. what abilities the player has, how many times they have died, if a gate has been opened at some point?)

    // keep track of player state (maybe using an enum? take a look at that video after sorting this out)

    // track individual room values (i.e. is a room's chest has been opened or if the enemy gate has been opened)
    private void Awake()
    {
        previousRoom = "spawn";
        LoadSettings();
    }


    private void Start()
    {
        #region Don't destroy on load/check for other instances
        if (instance != null)    // check if someone else is already this game object
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        GameObject.DontDestroyOnLoad(gameObject);   // become immortal

        health = maxHealth;
        ammoCount = maxAmmo;

        #endregion
    }

    private void Update()
    {
        string currentRoom = SceneManager.GetActiveScene().name;
        GetGateState(currentRoom);

        if(Input.GetKey(KeyCode.T))
        {
            Debug.Log("reseting the player prefs");
            ResetPlayerPrefs();
            resetPrefs = true;
        }

        if(Input.GetKey(KeyCode.Y))
        {
            Debug.Log("giving movement abilities");
            GiveAbilitiesPlayerPrefs();
            resetPrefs = true;
        }

        if (Input.GetKey(KeyCode.U))
        {
            Debug.Log("giving everything");
            GiveAllPlayerPrefs();
            resetPrefs = true;
        }

        if (!resetPrefs)
        {
            SetPlayerPrefs();
        }
    }

    private void OnDestroy()
    {
        SetSaveRoom();
        PlayerPrefs.Save();
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

    public string GetSaveRoom()
    {
        return saveRoom;
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

    public void SetMaxHealth(float healthValue)
    {
        maxHealth = healthValue;
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

    public void SetMaxAmmo(float ammoValue)
    {
        maxAmmo = ammoValue;
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

    //#region Money
    //public void AddMoney(float moneyEarned)
    //{
    //    money += moneyEarned;
    //}

    //public void LoseMoney(float cost)
    //{
    //    money -= cost;
    //}
    //#endregion

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

    #region Hidden Rooms
    public bool GetHiddenState(string roomName)
    {
        if (roomName == "Room1")
        {
            Debug.Log("from game status" + room1_HiddenOpen);
            return room1_HiddenOpen;
        }
        else
        {
            Debug.Log("cannot find the current room, returning true");
            return true;
        }
    }

    public void SetHiddenState(string roomName)
    {
        if (roomName == "Room1")
        {
            room1_HiddenOpen = true;
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
        if (roomName == "Room1")
        {
            if(pickup == "Health")
            {
                Debug.Log("room1_HealthIncrease" + room1_HealthIncreaseTaken);
                return room2_HealthIncreaseTaken;
            }
            else if (pickup == "Ammo")
            {
                Debug.Log("room1_AmmoIncrease" + room1_AmmoIncreaseTaken);
                return room2_AmmoIncreaseTaken;
            }
        }
        if (roomName == "Room2")
        {
            if (pickup == "Health")
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

        if (pickup == "dash")
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
        if (roomName == "Room1")
        {
            if (pickup == "Health")
            {
                Debug.Log("room1_HealthIncrease has been picked up");
                room1_HealthIncreaseTaken = true;
            }
            else if (pickup == "Ammo")
            {
                Debug.Log("room1_AmmoIncrease has been picked up");
                room1_AmmoIncreaseTaken = true;
            }
        }
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

    #region Player Prefs
    public void LoadSettings()
    {
        // health, ammo, and save room
        maxHealth = PlayerPrefs.GetFloat("maxHealth");
        maxAmmo = PlayerPrefs.GetFloat("maxAmmo");
        deathCounter = PlayerPrefs.GetInt("deathCounter");
        saveRoom = PlayerPrefs.GetString("saveRoom");

        // permanent abilities
        hasDash = PlayerPrefs.GetInt("hasDash") == 1;
        hasInvincibleDash = PlayerPrefs.GetInt("hasInvincibleDash") == 1;
        hasMelee = PlayerPrefs.GetInt("hasMelee") == 1;
        hasRanged = PlayerPrefs.GetInt("hasRanged") == 1;

        // gates/chests/upgrades/secret doors
        room1_enemyGateOpen = PlayerPrefs.GetInt("room1_enemyGateOpen") == 1;
        room2_chestOpen = PlayerPrefs.GetInt("room2_chestOpen") == 1;
        room2_wallOpen = PlayerPrefs.GetInt("room2_wallOpen") == 1;
        room1_HiddenOpen = PlayerPrefs.GetInt("room1_HiddenOpen") == 1;

        // permanent upgrades
        room1_HealthIncreaseTaken = PlayerPrefs.GetInt("room1_HealthIncreaseTaken") == 1;
        room1_AmmoIncreaseTaken = PlayerPrefs.GetInt("room1_AmmoIncreaseTaken") == 1;
        room2_HealthIncreaseTaken = PlayerPrefs.GetInt("room2_HealthIncreaseTaken") == 1;
        room2_AmmoIncreaseTaken = PlayerPrefs.GetInt("room2_AmmoIncreaseTaken") == 1;
    }

    public void SetPlayerPrefs()
    {
        //PlayerPrefs.SetFloat("maxHealth" + saveFile, maxHealth);
        PlayerPrefs.SetFloat("maxHealth", GetMaxHealth());
        PlayerPrefs.SetFloat("maxAmmo", GetMaxAmmo());
        PlayerPrefs.SetInt("deathCounter", deathCounter);

        // player states
        PlayerPrefs.SetInt("hasDash", HasDash() ? 1 : 0);
        PlayerPrefs.SetInt("hasInvincibleDash", HasInvincibleDash() ? 1 : 0);
        PlayerPrefs.SetInt("hasMelee", HasMelee() ? 1 : 0);
        PlayerPrefs.SetInt("hasRanged", HasRanged() ? 1 : 0);

        // doors and everything else
        PlayerPrefs.SetInt("room1_enemyGateOpen", room1_enemyGateOpen ? 1 : 0);
        PlayerPrefs.SetInt("room2_chestOpen", room2_chestOpen ? 1 : 0);
        PlayerPrefs.SetInt("room2_wallOpen", room2_wallOpen ? 1 : 0);
        PlayerPrefs.SetInt("room1_HiddenOpen", room1_HiddenOpen ? 1 : 0);

        // permanent upgrades
        PlayerPrefs.SetInt("room1_HealthIncreaseTaken", room1_HealthIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("room1_AmmoIncreaseTaken", room1_AmmoIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("room2_HealthIncreaseTaken", room2_HealthIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("room2_AmmoIncreaseTaken", room2_AmmoIncreaseTaken ? 1 : 0);

        //PlayerPrefs.Save();
    }

    public void SetSaveRoom()
    {
        PlayerPrefs.SetString("saveRoom", GetCurrentRoom());
    }

    public void ResetPlayerPrefs()  // use this to reset the player prefs when testing stuff but remove the hot key in the final version - a version of this will be used to make multiple save files
    {
        //PlayerPrefs.SetFloat("maxHealth" + saveFile, maxHealth);
        PlayerPrefs.SetFloat("maxHealth", 10);
        PlayerPrefs.SetFloat("maxAmmo", 10);
        PlayerPrefs.SetInt("deathCounter", 0);

        // player states
        PlayerPrefs.SetInt("hasDash", 0);
        PlayerPrefs.SetInt("hasInvincibleDash", 0);
        PlayerPrefs.SetInt("hasMelee", 0);
        PlayerPrefs.SetInt("hasRanged", 0);

        // doors and everything else
        PlayerPrefs.SetInt("room1_enemyGateOpen", 0);
        PlayerPrefs.SetInt("room2_chestOpen", 0);
        PlayerPrefs.SetInt("room2_wallOpen", 0);
        PlayerPrefs.SetInt("room1_HiddenOpen", 0);

        // permanent upgrades
        PlayerPrefs.SetInt("room1_HealthIncreaseTaken", 0);
        PlayerPrefs.SetInt("room1_AmmoIncreaseTaken", 0);
        PlayerPrefs.SetInt("room2_HealthIncreaseTaken", 0);
        PlayerPrefs.SetInt("room2_AmmoIncreaseTaken", 0);

        //PlayerPrefs.Save();
    }

    public void GiveAllPlayerPrefs()  // use this to reset the player prefs when testing stuff but remove the hot key in the final version - a version of this will be used to make multiple save files
    {
        //PlayerPrefs.SetFloat("maxHealth" + saveFile, maxHealth);
        PlayerPrefs.SetFloat("maxHealth", 30);
        PlayerPrefs.SetFloat("maxAmmo", 30);
        PlayerPrefs.SetInt("deathCounter", 0);

        // player states
        PlayerPrefs.SetInt("hasDash", 1);
        PlayerPrefs.SetInt("hasInvincibleDash", 1);
        PlayerPrefs.SetInt("hasMelee", 1);
        PlayerPrefs.SetInt("hasRanged", 1);

        // doors and everything else
        PlayerPrefs.SetInt("room1_enemyGateOpen", 1);
        PlayerPrefs.SetInt("room2_chestOpen", 1);
        PlayerPrefs.SetInt("room2_wallOpen", 1);
        PlayerPrefs.SetInt("room1_HiddenOpen", 1);

        // permanent upgrades
        PlayerPrefs.SetInt("room1_HealthIncreaseTaken", 1);
        PlayerPrefs.SetInt("room1_AmmoIncreaseTaken", 1);
        PlayerPrefs.SetInt("room2_HealthIncreaseTaken", 1);
        PlayerPrefs.SetInt("room2_AmmoIncreaseTaken", 1);

        //PlayerPrefs.Save();
    }

    private void GiveAbilitiesPlayerPrefs()
    {
        // player states
        PlayerPrefs.SetInt("hasDash", 1);
        PlayerPrefs.SetInt("hasInvincibleDash", 1);
        PlayerPrefs.SetInt("hasMelee", 1);
        PlayerPrefs.SetInt("hasRanged", 1);
    }
    #endregion

    public static GameStatus GetInstance()
    {
        return instance;
    }
}
