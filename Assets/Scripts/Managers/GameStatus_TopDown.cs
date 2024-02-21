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

    protected bool gamePaused;

    protected bool openInstructions;

    [Header("Player state bools")]
    [SerializeField]
    protected bool hasDash;
    [SerializeField]
    protected bool hasInvincibleDash;
    [SerializeField]
    protected bool hasMelee;
    [SerializeField]
    protected bool hasRanged;
    [SerializeField]
    protected bool hasHealing;

    #endregion

    #region Enemy/boss gates, doors, chests, breakable walls, &c. tracking
    // format for each gate is "roomName_enemyGateOpen" or "roomName_bossGateOpen", "roomName_lockedDoor", "roomName_Chest"
        // adding open to the end of the names for clarity because the bools are simply indicating if the associated game object has been opened or not
    // I think i will need to manually add in each enemy gate for each room, and then the generic script will affect if they are set or not

    [Header("Gates/doors/chests tracking")]
    [Header("Enemy Gates")]
    [SerializeField]
    protected bool Anger1_enemyGateOpen;
    [SerializeField]
    protected bool Anger4_enemyGateOpen;
    [SerializeField]
    protected bool Anger5_enemyGateOpen;
    [SerializeField]
    protected bool Anger6_enemyGateOpen;

    [Header("Chests")]
    [SerializeField]
    protected bool room2_chestOpen;

    [Header("Permanent Increases")]
    [SerializeField]
    protected bool Anger3_1_HealthIncreaseTaken;
    [SerializeField]
    protected bool Anger3_1_AmmoIncreaseTaken;
    [SerializeField]
    protected bool Anger4_HealthIncreaseTaken;
    [SerializeField]
    protected bool Anger5_AmmoIncreaseTaken;

    [Header("Breakable Walls")]
    [SerializeField]
    protected bool GameStart_wallOpen;
    [SerializeField]
    protected bool Anger3_wallOpen;
    [SerializeField]
    protected bool Anger3_1_wallOpen;

    [Header("Hidden Rooms")]
    [SerializeField]
    protected bool GameStart_HiddenOpen;
    [SerializeField]
    protected bool Anger2_and6_HiddenOpen;
    [SerializeField]
    protected bool Anger4_HiddenOpen;
    #endregion

    #region Player Prefs Variables
    // I think I need to use player prefs to save information between launches of the game? so I might have the prefs save between rooms for the time being and then eventually save when benching and load when launching the game?

    //private int saveFile;   // this is the number of the save file, which will allow for there to be multiple files. For now, just have 1 so slot == 1

    // things to player pref: max health, max ammo, death counter, last save room, dash, idash, range, melee, any doors/gates/chests/&c.

    [Header("Player health stats")]
    private bool resetPrefs = false;

    //protected string saveRoom;
    #endregion

    // making a class which can keep track of certain game states (i.e. what abilities the player has, how many times they have died, if a gate has been opened at some point?)

    // keep track of player state (maybe using an enum? take a look at that video after sorting this out)

    // track individual room values (i.e. is a room's chest has been opened or if the enemy gate has been opened)
    private void Awake()
    {
        previousRoom = "spawn";
        LoadSettings();
        Debug.Log("the previous room is: " + previousRoom);
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

        //if(Input.GetKey(KeyCode.Y))
        //{
        //    Debug.Log("giving movement abilities");
        //    GiveAbilitiesPlayerPrefs();
        //    resetPrefs = true;
        //}

        //if (Input.GetKey(KeyCode.U))
        //{
        //    Debug.Log("giving everything");
        //    GiveAllPlayerPrefs();
        //    resetPrefs = true;
        //}

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
        Debug.Log("game Paused" + gamePaused);
    }

    public bool GetGamePaused()
    {
        return gamePaused;
    }

    public void SetOpenMeleeInstructions(bool shouldOpen)
    {
        openInstructions = shouldOpen;
    }
    public bool OpenMeleeInstructions()
    {
        return openInstructions;
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


    public void ShootBullets(float bullets)
    {
        ammoCount -= bullets;
    }

    public void AddMaxAmmo(float ammoIncrease)
    {
        maxAmmo += ammoIncrease;
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

    public void SetHasHealing(bool healing)
    {
        hasHealing = healing;
    }

    public bool HasHealing()
    {
        return hasHealing;
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
        if (roomName == "Anger1")
        {
            Debug.Log(Anger1_enemyGateOpen);
            return Anger1_enemyGateOpen;
        }
        else if (roomName == "Anger4")
        {
            Debug.Log(Anger4_enemyGateOpen);
            return Anger4_enemyGateOpen;
        }
        else if (roomName == "Anger5")
        {
            Debug.Log(Anger5_enemyGateOpen);
            return Anger5_enemyGateOpen;
        }
        else if (roomName == "Anger6")
        {
            Debug.Log(Anger6_enemyGateOpen);
            return Anger6_enemyGateOpen;
        }
        else
        {
            Debug.Log("cannot find the current room, returning true");
            return true;
        }
    }

     //do the same thing for set, but take in the name of the thing set it to true
    public void SetGateState(string roomName)
    {
        if (roomName == "Anger1")
        {
            Debug.Log("Opening the gate in anger 1");
            Anger1_enemyGateOpen = true;
        }
        else if (roomName == "Anger4")
        {
            Debug.Log("Opening the gate in anger 4");
            Anger4_enemyGateOpen = true;
        }
        else if (roomName == "Anger5")
        {
            Debug.Log("Opening the gate in anger 5");
            Anger5_enemyGateOpen = true;
        }
        else if (roomName == "Anger6")
        {
            Debug.Log("Opening the gate in anger 6");
            Anger6_enemyGateOpen = true;
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
        if (roomName == "GameStart")
        {
            Debug.Log("from game status" + GameStart_wallOpen);
            return GameStart_wallOpen;
        }
        else if (roomName == "Anger3")
        {
            Debug.Log("from game status" + Anger3_wallOpen);
            return Anger3_wallOpen;
        }
        else if (roomName == "Anger3_1")
        {
            Debug.Log("from game status" + Anger3_1_wallOpen);
            return Anger3_1_wallOpen;
        }
        else
        {
            Debug.Log("cannot find the current room, returning true");
            return true;
        }
    }

    public void SetWallState(string roomName)
    {
        if (roomName == "GameStart")
        {
            GameStart_wallOpen = true;
        }
        else if (roomName == "Anger3")
        {
            Anger3_wallOpen = true;
        }
        else if (roomName == "Anger3_1")
        {
            Anger3_1_wallOpen = true;
        }
    }
    #endregion

    #region Hidden Rooms
    public bool GetHiddenState(string roomName)
    {
        if (roomName == "GameStart")
        {
            Debug.Log("from game status" + GameStart_HiddenOpen);
            return GameStart_HiddenOpen;
        }
        else if (roomName == "Anger2" || roomName == "Anger6")
        {
            Debug.Log("from game status" + Anger2_and6_HiddenOpen);
            return Anger2_and6_HiddenOpen;
        }
        else if (roomName == "Anger4")
        {
            Debug.Log("from game status" + Anger4_HiddenOpen);
            return Anger4_HiddenOpen;
        }
        else
        {
            Debug.Log("cannot find the current room, returning true");
            return true;
        }
    }

    public void SetHiddenState(string roomName)
    {
        if (roomName == "GameStart")
        {
            GameStart_HiddenOpen = true;
        }
        if (roomName == "Anger2" || roomName == "Anger6")
        {
            Anger2_and6_HiddenOpen = true;
        }
        if (roomName == "Anger4")
        {
            Anger4_HiddenOpen = true;
        }
    }
    #endregion

    // this structure can be used for any of the permanent pick ups/upgrades including the ability to dash and attack
    #region Permanent Upgrades
    public bool GetUpgradeState(string roomName, string pickup)
    {
        if (roomName == "Anger3_1")
        {
            if (pickup == "Health")
            {
                Debug.Log("Anger3_1_HealthIncrease" + Anger3_1_HealthIncreaseTaken);
                return Anger3_1_HealthIncreaseTaken;
            }
            else if (pickup == "Ammo")
            {
                Debug.Log("Anger3_1_AmmoIncreaseTaken" + Anger3_1_AmmoIncreaseTaken);
                return Anger3_1_AmmoIncreaseTaken;
            }
        }
        else if (roomName == "Anger4")
        {
            if (pickup == "Health")
            {
                Debug.Log("Anger4_HealthIncrease" + Anger4_HealthIncreaseTaken);
                return Anger4_HealthIncreaseTaken;
            }
        }
        else if (roomName == "Anger5")
        {
            if (pickup == "Ammo")
            {
                Debug.Log("Anger5_AmmoIncreaseTaken" + Anger5_AmmoIncreaseTaken);
                return Anger5_AmmoIncreaseTaken;
            }
        }
        else if (roomName == "Anger6")
        {
            if (pickup == "Melee")
            {
                Debug.Log("player has melee attack? " + hasMelee);
                return hasMelee;
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
        else if (pickup == "healing")
        {
            Debug.Log("player has healing ");
            return hasHealing;
        }
        else
        {
            return false;
        }
    }

     //do the same thing for set, but take in the name of the thing set it to true
    public void SetUpgradeState(string roomName, string pickup)
    {
        if (roomName == "Anger3_1")
        {
            if (pickup == "Health")
            {
                Debug.Log("Anger3_1_HealthIncrease has been picked up");
                Anger3_1_HealthIncreaseTaken = true;
            }
            else if (pickup == "Ammo")
            {
                Debug.Log("Anger3_1_AmmoIncrease has been picked up");
                Anger3_1_AmmoIncreaseTaken = true;
            }
        }
        else if (roomName == "Anger4")
        {
            if (pickup == "Health")
            {
                Debug.Log("Anger4_HealthIncrease has been picked up");
                Anger4_HealthIncreaseTaken = true;
            }
        }
        else if (roomName == "Anger5")
        {
            if (pickup == "Ammo")
            {
                Debug.Log("Anger5_AmmoIncrease has been picked up");
                Anger5_AmmoIncreaseTaken = true;
            }
        }
        if (pickup == "dash")
        {
            Debug.Log("giving the player dash");
            SetHasDash(true);
        }
        else if (pickup == "iDash")
        {
            Debug.Log("giving the player invincible dash");
            SetHasInvincibleDash(true);
        }
        else if (pickup == "melee")
        {
            Debug.Log("giving the player melee attack");
            SetHasMelee(true);
        }
        else if (pickup == "ranged")
        {
            Debug.Log("giving the player ranged attack");
            SetHasRanged(true);
        }
        else if (pickup == "healing")
        {
            Debug.Log("giving the player healing");
            SetHasHealing(true);
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
        previousRoom = PlayerPrefs.GetString("saveRoom");

        // permanent abilities
        hasDash = PlayerPrefs.GetInt("hasDash") == 1;
        hasInvincibleDash = PlayerPrefs.GetInt("hasInvincibleDash") == 1;
        hasMelee = PlayerPrefs.GetInt("hasMelee") == 1;
        hasRanged = PlayerPrefs.GetInt("hasRanged") == 1;
        hasHealing = PlayerPrefs.GetInt("hasHealing") == 1;

        // Breakable Walls
        GameStart_wallOpen = PlayerPrefs.GetInt("GameStart_wallOpen") == 1;
        Anger3_wallOpen = PlayerPrefs.GetInt("Anger3_wallOpen") == 1;
        Anger3_1_wallOpen = PlayerPrefs.GetInt("Anger3_1_wallOpen") == 1;

        // Fake Walls
        GameStart_HiddenOpen = PlayerPrefs.GetInt("GameStart_HiddenOpen") == 1;
        Anger2_and6_HiddenOpen = PlayerPrefs.GetInt("Anger2_and6_HiddenOpen") == 1;
        Anger4_HiddenOpen = PlayerPrefs.GetInt("Anger4_HiddenOpen") == 1;

        // Enemy Gates
        Anger1_enemyGateOpen = PlayerPrefs.GetInt("Anger1_enemyGateOpen") == 1;
        Anger4_enemyGateOpen = PlayerPrefs.GetInt("Anger4_enemyGateOpen") == 1;
        Anger5_enemyGateOpen = PlayerPrefs.GetInt("Anger5_enemyGateOpen") == 1;
        Anger6_enemyGateOpen = PlayerPrefs.GetInt("Anger6_enemyGateOpen") == 1;
        

        // permanent upgrades
        Anger3_1_HealthIncreaseTaken = PlayerPrefs.GetInt("Anger3_1_HealthIncreaseTaken") == 1;
        Anger3_1_AmmoIncreaseTaken = PlayerPrefs.GetInt("Anger3_1_AmmoIncreaseTaken") == 1;
        Anger4_HealthIncreaseTaken = PlayerPrefs.GetInt("Anger4_HealthIncreaseTaken") == 1;
        Anger5_AmmoIncreaseTaken = PlayerPrefs.GetInt("Anger5_AmmoIncreaseTaken") == 1;
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
        PlayerPrefs.SetInt("hasHealing", HasHealing() ? 1 : 0);


        // Breakable Walls
        PlayerPrefs.SetInt("GameStart_wallOpen", GameStart_wallOpen ? 1 : 0);
        PlayerPrefs.SetInt("Anger3_wallOpen", Anger3_wallOpen ? 1 : 0);
        PlayerPrefs.SetInt("Anger3_1_wallOpen", Anger3_1_wallOpen ? 1 : 0);

        // Fake Walls
        PlayerPrefs.SetInt("GameStart_HiddenOpen", GameStart_HiddenOpen ? 1 : 0);
        PlayerPrefs.SetInt("Anger2_and6_HiddenOpen", Anger2_and6_HiddenOpen ? 1 : 0);
        PlayerPrefs.SetInt("Anger4_HiddenOpen", Anger4_HiddenOpen ? 1 : 0);

        // Enemy Gates
        PlayerPrefs.SetInt("Anger1_enemyGateOpen", Anger1_enemyGateOpen ? 1 : 0);
        PlayerPrefs.SetInt("Anger4_enemyGateOpen", Anger4_enemyGateOpen ? 1 : 0);
        PlayerPrefs.SetInt("Anger5_enemyGateOpen", Anger5_enemyGateOpen ? 1 : 0);
        PlayerPrefs.SetInt("Anger6_enemyGateOpen", Anger6_enemyGateOpen ? 1 : 0);
        
        // permanent upgrades
        PlayerPrefs.SetInt("Anger3_1_HealthIncreaseTaken", Anger3_1_HealthIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("Anger3_1_AmmoIncreaseTaken", Anger3_1_AmmoIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("Anger4_HealthIncreaseTaken", Anger4_HealthIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("Anger5_AmmoIncreaseTaken", Anger5_AmmoIncreaseTaken ? 1 : 0);

        //PlayerPrefs.Save();
    }

    public void SetSaveRoom()
    {
        if (GetCurrentRoom() != "MainMenu")
        {
            PlayerPrefs.SetString("saveRoom", GetCurrentRoom());
        }
        PlayerPrefs.Save();

        Debug.Log("save room is " + PlayerPrefs.GetString("saveRoom"));
    }

    public string GetSaveRoom()
    {
        return PlayerPrefs.GetString("saveRoom");
    }

    public void ResetPlayerPrefs()  // use this to reset the player prefs when testing stuff but remove the hot key in the final version - a version of this will be used to make multiple save files
    {
        //PlayerPrefs.SetFloat("maxHealth" + saveFile, maxHealth);
        PlayerPrefs.SetFloat("maxHealth", 10);
        PlayerPrefs.SetFloat("maxAmmo", 10);
        PlayerPrefs.SetInt("deathCounter", 0);
        PlayerPrefs.SetString("saveRoom", "MainMenu");

        // player states
        PlayerPrefs.SetInt("hasDash", 0);
        PlayerPrefs.SetInt("hasInvincibleDash", 0);
        PlayerPrefs.SetInt("hasMelee", 0);
        PlayerPrefs.SetInt("hasRanged", 0);
        PlayerPrefs.SetInt("hasHealing", 0);

        // Breakable Walls
        PlayerPrefs.SetInt("GameStart_wallOpen", 0);
        PlayerPrefs.SetInt("Anger3_wallOpen", 0);
        PlayerPrefs.SetInt("Anger3_1_wallOpen", 0);

        // Fake Walls
        PlayerPrefs.SetInt("GameStart_HiddenOpen", 0);
        PlayerPrefs.SetInt("Anger2_and6_HiddenOpen", 0);
        PlayerPrefs.SetInt("Anger4_HiddenOpen", 0);

        // Enemy Gates
        PlayerPrefs.SetInt("Anger1_enemyGateOpen", 0);
        PlayerPrefs.SetInt("Anger4_enemyGateOpen", 0);
        PlayerPrefs.SetInt("Anger5_enemyGateOpen", 0);
        PlayerPrefs.SetInt("Anger6_enemyGateOpen", 0);
        

        // permanent upgrades
        PlayerPrefs.SetInt("Anger3_1_HealthIncreaseTaken", 0);
        PlayerPrefs.SetInt("Anger3_1_AmmoIncreaseTaken", 0);
        PlayerPrefs.SetInt("Anger4_HealthIncreaseTaken", 0);
        PlayerPrefs.SetInt("Anger5_AmmoIncreaseTaken", 0);

        PlayerPrefs.Save();
    }

    //public void GiveAllPlayerPrefs()  // use this to reset the player prefs when testing stuff but remove the hot key in the final version - a version of this will be used to make multiple save files
    //{
    //    //PlayerPrefs.SetFloat("maxHealth" + saveFile, maxHealth);
    //    PlayerPrefs.SetFloat("maxHealth", 30);
    //    PlayerPrefs.SetFloat("maxAmmo", 30);
    //    PlayerPrefs.SetInt("deathCounter", 0);

    //    // player states
    //    PlayerPrefs.SetInt("hasDash", 1);
    //    PlayerPrefs.SetInt("hasInvincibleDash", 1);
    //    PlayerPrefs.SetInt("hasMelee", 1);
    //    PlayerPrefs.SetInt("hasRanged", 1);
    //    PlayerPrefs.SetInt("hasHealing", 1);


    //    // doors and everything else
    //    PlayerPrefs.SetInt("room2_wallOpen", 1);
    //    PlayerPrefs.SetInt("room1_HiddenOpen", 1);

    //    // permanent upgrades
    //    //PlayerPrefs.SetInt("room1_HealthIncreaseTaken", 1);

    //    //PlayerPrefs.Save();
    //}

    //private void GiveAbilitiesPlayerPrefs()
    //{
    //    // player states
    //    PlayerPrefs.SetInt("hasDash", 1);
    //    PlayerPrefs.SetInt("hasInvincibleDash", 1);
    //    PlayerPrefs.SetInt("hasMelee", 1);
    //    PlayerPrefs.SetInt("hasRanged", 1);
    //    PlayerPrefs.SetInt("hasHealing", 1);
    //}
    #endregion

    public static GameStatus GetInstance()
    {
        return instance;
    }
}
