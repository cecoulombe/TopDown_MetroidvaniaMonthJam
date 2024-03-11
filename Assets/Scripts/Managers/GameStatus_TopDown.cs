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

    protected bool openMeleeInstructions;
    protected bool openRangedInstructions;

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
    [SerializeField]
    protected bool Anger6_1_enemyGateOpen;
    [SerializeField]
    protected bool Words2_enemyGateOpen;
    [SerializeField]
    protected bool Words3_enemyGateOpen;
    [SerializeField]
    protected bool Words5_enemyGateOpen;
    [SerializeField]
    protected bool Overwhelm2_enemyGateOpen;
    [SerializeField]
    protected bool Overwhelm3_enemyGateOpen;

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
    [SerializeField]
    protected bool Anger6_1_AmmoIncreaseTaken;
    [SerializeField]
    protected bool Words2_HealthIncreaseTaken;
    [SerializeField]
    protected bool Words5_HealthIncreaseTaken;
    [SerializeField]
    protected bool Overwhelm2_AmmoIncreaseTaken;
    [SerializeField]
    protected bool Overwhelm4_HealthIncreaseTaken;
    [SerializeField]
    protected bool Overwhelm4_AmmoIncreaseTaken;

    [Header("Breakable Walls")]
    [SerializeField]
    protected bool GameStart_wallOpen;
    [SerializeField]
    protected bool Anger3_wallOpen;
    [SerializeField]
    protected bool Anger3_1_wallOpen;
    [SerializeField]
    protected bool Words1_wallOpen;
    [SerializeField]
    protected bool Words2_wallOpen;
    [SerializeField]
    protected bool Words5_wallOpen;
    [SerializeField]
    protected bool Overwhelm2_wallOpen;

    [Header("Hidden Rooms")]
    [SerializeField]
    protected bool GameStart_Overwhelm1_HiddenOpen;
    [SerializeField]
    protected bool Anger2_and6_HiddenOpen;
    [SerializeField]
    protected bool Anger4_HiddenOpen;
    [SerializeField]
    protected bool Words2_HiddenOpen;
    [SerializeField]
    protected bool Overwhelm3_4_HiddenOpen;
    #endregion


    protected float totalPickUps;
    public float pickedUpPercent;

    public float completionPercent;


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
        totalPickUps = 25;

        #endregion
    }

    private void Update()
    {
        string currentRoom = SceneManager.GetActiveScene().name;
        GetGateState(currentRoom);
        //SetCompletionPercent();
        completionPercent = Mathf.RoundToInt(pickedUpPercent / totalPickUps * 100);
        Debug.Log(pickedUpPercent + " divided by" + totalPickUps + " * 100 = " + completionPercent);


        if (Input.GetKey(KeyCode.Escape))
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
        openMeleeInstructions = shouldOpen;
    }
    public bool OpenMeleeInstructions()
    {
        return openMeleeInstructions;
    }

    public void SetOpenRangedInstructions(bool shouldOpen)
    {
        openRangedInstructions = shouldOpen;
    }
    public bool OpenRangedInstructions()
    {
        return openRangedInstructions;
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
        pickedUpPercent += 1;
    }
    public bool HasDash()
    {
        return hasDash;
    }

    public void SetHasInvincibleDash(bool iDash)
    {
        hasInvincibleDash = iDash;
        pickedUpPercent += 1;
    }

    public bool HasInvincibleDash()
    {
        return hasInvincibleDash;
    }

    public void SetHasMelee(bool melee)
    {
        hasMelee = melee;
        pickedUpPercent += 1;
    }

    public bool HasMelee()
    {
        return hasMelee;
    }

    public void SetHasRanged(bool ranged)
    {
        hasRanged = ranged;
        pickedUpPercent += 1;
    }

    public bool HasRanged()
    {
        return hasRanged;
    }

    public void SetHasHealing(bool healing)
    {
        hasHealing = healing;
        pickedUpPercent += 1;
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
            return Anger1_enemyGateOpen;
        }
        else if (roomName == "Anger4")
        {
            return Anger4_enemyGateOpen;
        }
        else if (roomName == "Anger5")
        {
            return Anger5_enemyGateOpen;
        }
        else if (roomName == "Anger6")
        {
            return Anger6_enemyGateOpen;
        }
        else if (roomName == "Anger6_1")
        {
            return Anger6_1_enemyGateOpen;
        }
        else if (roomName == "Words2")
        {
            return Words2_enemyGateOpen;
        }
        else if (roomName == "Words3")
        {
            return Words3_enemyGateOpen;
        }
        else if (roomName == "Words5")
        {
            return Words5_enemyGateOpen;
        }
        else if (roomName == "Overwhelm2")
        {
            return Overwhelm2_enemyGateOpen;
        }
        else if (roomName == "Overwhelm3")
        {
            return Overwhelm3_enemyGateOpen;
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
            Anger1_enemyGateOpen = true;
        }
        else if (roomName == "Anger4")
        {
            Anger4_enemyGateOpen = true;
        }
        else if (roomName == "Anger5")
        {
            Anger5_enemyGateOpen = true;
        }
        else if (roomName == "Anger6")
        {
            Anger6_enemyGateOpen = true;
        }
        else if (roomName == "Anger6_1")
        {
            Anger6_1_enemyGateOpen = true;
        }
        else if (roomName == "Words2")
        {
            Words2_enemyGateOpen = true;
        }
        else if (roomName == "Words3")
        {
            Words3_enemyGateOpen = true;
        }
        else if (roomName == "Words5")
        {
            Words5_enemyGateOpen = true;
        }
        else if (roomName == "Overwhelm2")
        {
            Overwhelm2_enemyGateOpen = true;
        }
        else if (roomName == "Overwhelm3")
        {
            Overwhelm3_enemyGateOpen = true;
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
            return GameStart_wallOpen;
        }
        else if (roomName == "Anger3")
        {
            return Anger3_wallOpen;
        }
        else if (roomName == "Anger3_1")
        {
            return Anger3_1_wallOpen;
        }
        else if (roomName == "Words1")
        {
            return Words1_wallOpen;
        }
        else if (roomName == "Words2")
        {
            return Words2_wallOpen;
        }
        else if (roomName == "Words5")
        {
            return Words5_wallOpen;
        }
        else if (roomName == "Overwhelm2")
        {
            return Overwhelm2_wallOpen;
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
            pickedUpPercent += 1;
        }
        else if (roomName == "Anger3")
        {
            Anger3_wallOpen = true;
            pickedUpPercent += 1;
        }
        else if (roomName == "Anger3_1")
        {
            Anger3_1_wallOpen = true;
            pickedUpPercent += 1;
        }
        else if (roomName == "Words1")
        {
            Words1_wallOpen = true;
            pickedUpPercent += 1;
        }
        else if (roomName == "Words2")
        {
            Words2_wallOpen = true;
            pickedUpPercent += 1;
        }
        else if (roomName == "Words5")
        {
            Words5_wallOpen = true;
            pickedUpPercent += 1;
        }
        else if (roomName == "Overwhelm2")
        {
            Overwhelm2_wallOpen = true;
            pickedUpPercent += 1;
        }
    }
    #endregion

    #region Hidden Rooms
    public bool GetHiddenState(string roomName)
    {
        if (roomName == "GameStart" || roomName == "Overwhelm1")
        {
            return GameStart_Overwhelm1_HiddenOpen;
        }
        else if (roomName == "Anger2" || roomName == "Anger6")
        {
            return Anger2_and6_HiddenOpen;
        }
        else if (roomName == "Anger4")
        {
            return Anger4_HiddenOpen;
        }
        else if (roomName == "Words2")
        {
            return Words2_HiddenOpen;
        }
        else if (roomName == "Overwhelm3" || roomName == "Overwhelm4")
        {
            return Overwhelm3_4_HiddenOpen;
        }
        else
        {
            Debug.Log("cannot find the current room, returning true");
            return true;
        }
    }

    public void SetHiddenState(string roomName)
    {
        if (roomName == "GameStart" || roomName == "Overwhelm1")
        {
            GameStart_Overwhelm1_HiddenOpen = true;
            pickedUpPercent += 1;
        }
        if (roomName == "Anger2" || roomName == "Anger6")
        {
            Anger2_and6_HiddenOpen = true;
            pickedUpPercent += 1f;
        }
        if (roomName == "Anger4")
        {
            Anger4_HiddenOpen = true;
            pickedUpPercent += 1f;
        }
        if (roomName == "Words2")
        {
            Words2_HiddenOpen = true;
            pickedUpPercent += 1;
        }
        if (roomName == "Overwhelm3" || roomName == "Overwhelm4")
        {
            Overwhelm3_4_HiddenOpen = true;
            pickedUpPercent += 1;
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
                return Anger3_1_HealthIncreaseTaken;
            }
            else if (pickup == "Ammo")
            {
                return Anger3_1_AmmoIncreaseTaken;
            }
        }
        else if (roomName == "Anger4")
        {
            if (pickup == "Health")
            {
                return Anger4_HealthIncreaseTaken;
            }
        }
        else if (roomName == "Anger5")
        {
            if (pickup == "Ammo")
            {
                return Anger5_AmmoIncreaseTaken;
            }
        }
        else if (roomName == "Anger6_1")
        {
            if (pickup == "Ammo")
            {
                return Anger6_1_AmmoIncreaseTaken;
            }
        }
        else if (roomName == "Anger6")
        {
            if (pickup == "Melee")
            {
                return hasMelee;
            }
        }
        else if (roomName == "Words2")
        {
            if (pickup == "Health")
            {
                return Words2_HealthIncreaseTaken;
            }
        }
        else if (roomName == "Words5")
        {
            if (pickup == "Health")
            {
                return Words5_HealthIncreaseTaken;
            }
        }
        else if (roomName == "Overwhelm2")
        {
            if (pickup == "Ammo")
            {
                return Overwhelm2_AmmoIncreaseTaken;
            }
        }
        else if (roomName == "Overwhelm3")
        {
            if (pickup == "Ranged")
            {
                return hasRanged;
            }
        }
        else if (roomName == "Overwhelm4")
        {
            if (pickup == "Health")
            {
                return Overwhelm4_HealthIncreaseTaken;
            }
            else if (pickup == "Ammo")
            {
                return Overwhelm4_AmmoIncreaseTaken;
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
                Anger3_1_HealthIncreaseTaken = true;
                pickedUpPercent += 1;
            }
            else if (pickup == "Ammo")
            {
                Anger3_1_AmmoIncreaseTaken = true;
                pickedUpPercent += 1;
            }
        }
        else if (roomName == "Anger4")
        {
            if (pickup == "Health")
            {
                Anger4_HealthIncreaseTaken = true;
                pickedUpPercent += 1;
            }
        }
        else if (roomName == "Anger5")
        {
            if (pickup == "Ammo")
            {
                Anger5_AmmoIncreaseTaken = true;
                pickedUpPercent += 1;
            }
        }
        else if (roomName == "Anger6_1")
        {
            if (pickup == "Ammo")
            {
                Anger6_1_AmmoIncreaseTaken = true;
                pickedUpPercent += 1;
            }
        }
        else if (roomName == "Words2")
        {
            if (pickup == "Health")
            {
                Words2_HealthIncreaseTaken = true;
                pickedUpPercent += 1;
            }
        }
        else if (roomName == "Words5")
        {
            if (pickup == "Health")
            {
                Words5_HealthIncreaseTaken = true;
                pickedUpPercent += 1;
            }
        }
        else if (roomName == "Overwhelm2")
        {
            if (pickup == "Ammo")
            {
                Overwhelm2_AmmoIncreaseTaken = true;
                pickedUpPercent += 1;
            }
        }
        else if (roomName == "Overwhelm4")
        {
            if (pickup == "Health")
            {
                Overwhelm4_HealthIncreaseTaken = true;
                pickedUpPercent += 1;
            }
            else if (pickup == "Ammo")
            {
                Overwhelm4_AmmoIncreaseTaken = true;
                pickedUpPercent += 1;
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
        pickedUpPercent = PlayerPrefs.GetFloat("pickedUpPercent");

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
        Words1_wallOpen = PlayerPrefs.GetInt("Words1_wallOpen") == 1;
        Words2_wallOpen = PlayerPrefs.GetInt("Words2_wallOpen") == 1;
        Words5_wallOpen = PlayerPrefs.GetInt("Words5_wallOpen") == 1;
        Overwhelm2_wallOpen = PlayerPrefs.GetInt("Overwhelm2_wallOpen") == 1;


        // Fake Walls
        GameStart_Overwhelm1_HiddenOpen = PlayerPrefs.GetInt("GameStart_Overwhelm1_HiddenOpen") == 1;
        Anger2_and6_HiddenOpen = PlayerPrefs.GetInt("Anger2_and6_HiddenOpen") == 1;
        Anger4_HiddenOpen = PlayerPrefs.GetInt("Anger4_HiddenOpen") == 1;
        Words2_HiddenOpen = PlayerPrefs.GetInt("Words2_HiddenOpen") == 1;
        Overwhelm3_4_HiddenOpen = PlayerPrefs.GetInt("Overwhelm3_4_HiddenOpen") == 1;

        // Enemy Gates
        Anger1_enemyGateOpen = PlayerPrefs.GetInt("Anger1_enemyGateOpen") == 1;
        Anger4_enemyGateOpen = PlayerPrefs.GetInt("Anger4_enemyGateOpen") == 1;
        Anger5_enemyGateOpen = PlayerPrefs.GetInt("Anger5_enemyGateOpen") == 1;
        Anger6_enemyGateOpen = PlayerPrefs.GetInt("Anger6_enemyGateOpen") == 1;
        Anger6_1_enemyGateOpen = PlayerPrefs.GetInt("Anger6_1_enemyGateOpen") == 1;
        Words2_enemyGateOpen = PlayerPrefs.GetInt("Words2_enemyGateOpen") == 1;
        Words3_enemyGateOpen = PlayerPrefs.GetInt("Words3_enemyGateOpen") == 1;
        Words5_enemyGateOpen = PlayerPrefs.GetInt("Words5_enemyGateOpen") == 1;
        Overwhelm2_enemyGateOpen = PlayerPrefs.GetInt("Overwhelm2_enemyGateOpen") == 1;
        Overwhelm3_enemyGateOpen = PlayerPrefs.GetInt("Overwhelm3_enemyGateOpen") == 1;

        // permanent upgrades
        Anger3_1_HealthIncreaseTaken = PlayerPrefs.GetInt("Anger3_1_HealthIncreaseTaken") == 1;
        Anger3_1_AmmoIncreaseTaken = PlayerPrefs.GetInt("Anger3_1_AmmoIncreaseTaken") == 1;
        Anger4_HealthIncreaseTaken = PlayerPrefs.GetInt("Anger4_HealthIncreaseTaken") == 1;
        Anger5_AmmoIncreaseTaken = PlayerPrefs.GetInt("Anger5_AmmoIncreaseTaken") == 1;
        Anger6_1_AmmoIncreaseTaken = PlayerPrefs.GetInt("Anger6_1_AmmoIncreaseTaken") == 1;
        Words2_HealthIncreaseTaken = PlayerPrefs.GetInt("Words2_HealthIncreaseTaken") == 1;
        Words5_HealthIncreaseTaken = PlayerPrefs.GetInt("Words5_HealthIncreaseTaken") == 1;
        Overwhelm2_AmmoIncreaseTaken = PlayerPrefs.GetInt("Overwhelm2_AmmoIncreaseTaken") == 1;
        Overwhelm4_HealthIncreaseTaken = PlayerPrefs.GetInt("Overwhelm4_HealthIncreaseTaken") == 1;
        Overwhelm4_AmmoIncreaseTaken = PlayerPrefs.GetInt("Overwhelm4_AmmoIncreaseTaken") == 1;

    }

    public void SetPlayerPrefs()
    {
        //PlayerPrefs.SetFloat("maxHealth" + saveFile, maxHealth);
        PlayerPrefs.SetFloat("maxHealth", GetMaxHealth());
        PlayerPrefs.SetFloat("maxAmmo", GetMaxAmmo());
        PlayerPrefs.SetInt("deathCounter", deathCounter);
        PlayerPrefs.SetFloat("pickedUpPercent", pickedUpPercent);


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
        PlayerPrefs.SetInt("Words1_wallOpen", Words1_wallOpen ? 1 : 0);
        PlayerPrefs.SetInt("Words2_wallOpen", Words2_wallOpen ? 1 : 0);
        PlayerPrefs.SetInt("Words5_wallOpen", Words5_wallOpen ? 1 : 0);
        PlayerPrefs.SetInt("Overwhelm2_wallOpen", Overwhelm2_wallOpen ? 1 : 0);

        // Fake Walls
        PlayerPrefs.SetInt("GameStart_Overwhelm1_HiddenOpen", GameStart_Overwhelm1_HiddenOpen ? 1 : 0);
        PlayerPrefs.SetInt("Anger2_and6_HiddenOpen", Anger2_and6_HiddenOpen ? 1 : 0);
        PlayerPrefs.SetInt("Anger4_HiddenOpen", Anger4_HiddenOpen ? 1 : 0);
        PlayerPrefs.SetInt("Words2_HiddenOpen", Words2_HiddenOpen ? 1 : 0);
        PlayerPrefs.SetInt("Overwhelm3_4_HiddenOpen", Overwhelm3_4_HiddenOpen ? 1 : 0);


        // Enemy Gates
        PlayerPrefs.SetInt("Anger1_enemyGateOpen", Anger1_enemyGateOpen ? 1 : 0);
        PlayerPrefs.SetInt("Anger4_enemyGateOpen", Anger4_enemyGateOpen ? 1 : 0);
        PlayerPrefs.SetInt("Anger5_enemyGateOpen", Anger5_enemyGateOpen ? 1 : 0);
        PlayerPrefs.SetInt("Anger6_enemyGateOpen", Anger6_enemyGateOpen ? 1 : 0);
        PlayerPrefs.SetInt("Anger6_1_enemyGateOpen", Anger6_1_enemyGateOpen ? 1 : 0);
        PlayerPrefs.SetInt("Words2_enemyGateOpen", Words2_enemyGateOpen ? 1 : 0);
        PlayerPrefs.SetInt("Words3_enemyGateOpen", Words3_enemyGateOpen ? 1 : 0);
        PlayerPrefs.SetInt("Words5_enemyGateOpen", Words5_enemyGateOpen ? 1 : 0);
        PlayerPrefs.SetInt("Overwhelm2_enemyGateOpen", Overwhelm2_enemyGateOpen ? 1 : 0);
        PlayerPrefs.SetInt("Overwhelm3_enemyGateOpen", Overwhelm3_enemyGateOpen ? 1 : 0);

        // permanent upgrades
        PlayerPrefs.SetInt("Anger3_1_HealthIncreaseTaken", Anger3_1_HealthIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("Anger3_1_AmmoIncreaseTaken", Anger3_1_AmmoIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("Anger4_HealthIncreaseTaken", Anger4_HealthIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("Anger5_AmmoIncreaseTaken", Anger5_AmmoIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("Anger6_1_AmmoIncreaseTaken", Anger6_1_AmmoIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("Words2_HealthIncreaseTaken", Words2_HealthIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("Words5_HealthIncreaseTaken", Words5_HealthIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("Overwhelm2_AmmoIncreaseTaken", Overwhelm2_AmmoIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("Overwhelm4_HealthIncreaseTaken", Overwhelm4_HealthIncreaseTaken ? 1 : 0);
        PlayerPrefs.SetInt("Overwhelm4_AmmoIncreaseTaken", Overwhelm4_AmmoIncreaseTaken ? 1 : 0);


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
        PlayerPrefs.SetFloat("pickedUpPercent", 0);


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
        PlayerPrefs.SetInt("Words1_wallOpen", 0);
        PlayerPrefs.SetInt("Words2_wallOpen", 0);
        PlayerPrefs.SetInt("Words5_wallOpen", 0);
        PlayerPrefs.SetInt("Overwhelm2_wallOpen", 0);

        // Fake Walls
        PlayerPrefs.SetInt("GameStart_Overwhelm1_HiddenOpen", 0);
        PlayerPrefs.SetInt("Anger2_and6_HiddenOpen", 0);
        PlayerPrefs.SetInt("Anger4_HiddenOpen", 0);
        PlayerPrefs.SetInt("Words2_HiddenOpen", 0);
        PlayerPrefs.SetInt("Overwhelm3_4_HiddenOpen", 0);

        PlayerPrefs.SetInt("Anger1_enemyGateOpen", 0);
        // Enemy Gates
        PlayerPrefs.SetInt("Anger4_enemyGateOpen", 0);
        PlayerPrefs.SetInt("Anger5_enemyGateOpen", 0);
        PlayerPrefs.SetInt("Anger6_enemyGateOpen", 0);
        PlayerPrefs.SetInt("Anger6_1_enemyGateOpen", 0);
        PlayerPrefs.SetInt("Words2_enemyGateOpen", 0);
        PlayerPrefs.SetInt("Words3_enemyGateOpen", 0);
        PlayerPrefs.SetInt("Words5_enemyGateOpen", 0);
        PlayerPrefs.SetInt("Overwhelm2_enemyGateOpen", 0);
        PlayerPrefs.SetInt("Overwhelm3_enemyGateOpen", 0);

        // permanent upgrades
        PlayerPrefs.SetInt("Anger3_1_HealthIncreaseTaken", 0);
        PlayerPrefs.SetInt("Anger3_1_AmmoIncreaseTaken", 0);
        PlayerPrefs.SetInt("Anger4_HealthIncreaseTaken", 0);
        PlayerPrefs.SetInt("Anger5_AmmoIncreaseTaken", 0);
        PlayerPrefs.SetInt("Anger6_1_AmmoIncreaseTaken", 0);
        PlayerPrefs.SetInt("Words2_HealthIncreaseTaken", 0);
        PlayerPrefs.SetInt("Words5_HealthIncreaseTaken", 0);
        PlayerPrefs.SetInt("Overwhelm2_AmmoIncreaseTaken", 0);
        PlayerPrefs.SetInt("Overwhelm4_HealthIncreaseTaken", 0);
        PlayerPrefs.SetInt("Overwhelm4_AmmoIncreaseTaken", 0);

        PlayerPrefs.Save();
    }
    #endregion

    #region Completion Percent calculator
    private void SetCompletionPercent()
    {
        totalPickUps = 37;
        completionPercent = Mathf.RoundToInt((pickedUpPercent / totalPickUps) * 100);
    }

    public float GetCompletionPercent()
    {
        return completionPercent;
    }

    #endregion

    public static GameStatus GetInstance()
    {
        return instance;
    }
}
