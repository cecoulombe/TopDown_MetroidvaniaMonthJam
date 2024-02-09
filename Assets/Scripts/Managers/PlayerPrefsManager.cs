using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    // I think I need to use player prefs to save information between launches of the game? so I might have the prefs save between rooms for the time being and then eventually save when benching and load when launching the game?

    //private int saveFile;   // this is the number of the save file, which will allow for there to be multiple files. For now, just have 1 so slot == 1

    // things to player pref: max health, max ammo, death counter, last save room, dash, idash, range, melee, any doors/gates/chests/&c.

    [Header("Player health stats")]
    [SerializeField]
    protected float maxHealth_prefs;
    [SerializeField]
    protected int deathCounter_prefs;
    [SerializeField]
    protected float maxAmmo_prefs;

    [SerializeField]
    protected string saveRoom_prefs;

    [Header("Player state bools")]
    [SerializeField]
    protected bool hasDash_prefs;
    [SerializeField]
    protected bool hasInvincibleDash_prefs;
    [SerializeField]
    protected bool hasMelee_prefs;
    [SerializeField]
    protected bool hasRanged_prefs;

    [Header("Gates/doors/chests tracking")]
    [SerializeField]
    protected bool room1_enemyGateOpen_prefs;
    [SerializeField]
    protected bool room2_chestOpen_prefs;
    [SerializeField]
    protected bool room2_wallOpen_prefs;

    [Header("Permanent upgrades")]
    [SerializeField]
    protected bool room2_HealthIncreaseTaken_prefs;
    [SerializeField]
    protected bool room2_AmmoIncreaseTaken_prefs;

    // start by loading all relevant player prefs and then adding them into the game status
    void Start()
    {
        LoadSettings();
        SendToGameStatus();
    }

    public void LoadSettings()
    {
        // health, ammo, and save room
        maxHealth_prefs = PlayerPrefs.GetFloat("maxHealth");
        maxAmmo_prefs = PlayerPrefs.GetFloat("maxAmmo");
        //deathCounter = PlayerPrefs.GetInt("deathCounter");
        saveRoom_prefs = PlayerPrefs.GetString("saveRoom");

        // permanent abilities
        hasDash_prefs = PlayerPrefs.GetInt("hasDash") == 1;
        hasInvincibleDash_prefs = PlayerPrefs.GetInt("hasInvincibleDash") == 1;
        hasMelee_prefs = PlayerPrefs.GetInt("hasMelee") == 1;
        hasRanged_prefs = PlayerPrefs.GetInt("hasRanged") == 1;

        // gates/chests/upgrades/secret doors
        room1_enemyGateOpen_prefs = PlayerPrefs.GetInt("room1_enemyGateOpen") == 1;
        room2_chestOpen_prefs = PlayerPrefs.GetInt("room2_chestOpen") == 1;
        room2_wallOpen_prefs = PlayerPrefs.GetInt("room2_wallOpen") == 1;

        // permanent upgrades
        room2_HealthIncreaseTaken_prefs = PlayerPrefs.GetInt("room2_HealthIncreaseTaken") == 1;
        room2_AmmoIncreaseTaken_prefs = PlayerPrefs.GetInt("room2_AmmoIncreaseTaken") == 1;
    }

    public void SendToGameStatus()
    {
        GameStatus.GetInstance().SetMaxHealth(maxHealth_prefs);
        GameStatus.GetInstance().SetMaxAmmo(maxAmmo_prefs);

        // player states
        GameStatus.GetInstance().SetHasDash(hasDash_prefs);
        GameStatus.GetInstance().SetHasInvincibleDash(hasInvincibleDash_prefs);
        GameStatus.GetInstance().SetHasMelee(hasMelee_prefs);
        GameStatus.GetInstance().SetHasRanged(hasRanged_prefs);

        // gates, chests, walls, &c.
        if (room1_enemyGateOpen_prefs)
        {
            GameStatus.GetInstance().SetGateState("room1");
        }

        if (room2_chestOpen_prefs)
        {
            GameStatus.GetInstance().SetChestState("room2");
        }

        if (room2_wallOpen_prefs)
        {
            GameStatus.GetInstance().SetWallState("room2");
        }

        //if (room1_enemyGateOpen == true)
        //{
        //    // gate is open
        //}

        // permanent upgrades
        if(room2_HealthIncreaseTaken_prefs)
        {
            GameStatus.GetInstance().SetUpgradeState("room2", "Health");
        }

        if (room2_AmmoIncreaseTaken_prefs)
        {
            GameStatus.GetInstance().SetUpgradeState("room2", "Ammo");
        }

        //if (room2_AmmoIncreaseTaken)
        //{
        //    // upgrade was taken
        //}

    }

    public void SetPlayerPrefs()
    {
        //PlayerPrefs.SetFloat("maxHealth" + saveFile, maxHealth);
        PlayerPrefs.SetFloat("maxHealth", maxHealth_prefs);
        PlayerPrefs.SetFloat("maxAmmo", maxAmmo_prefs);

        // player states
        PlayerPrefs.GetInt("hasDash", GameStatus.GetInstance().HasDash() ? 1 : 0) ;
        PlayerPrefs.GetInt("hasInvincibleDash", GameStatus.GetInstance().HasInvincibleDash() ? 1 : 0);
        PlayerPrefs.GetInt("hasMelee", GameStatus.GetInstance().HasMelee() ? 1 : 0);
        PlayerPrefs.GetInt("hasRanged", GameStatus.GetInstance().HasRanged() ? 1 : 0);

        // doors and everything else
        PlayerPrefs.GetInt("room1_enemyGateOpen", GameStatus.GetInstance().GetGateState("room1") ? 1 : 0);
        PlayerPrefs.GetInt("room2_chestOpen", GameStatus.GetInstance().GetChestState("room2") ? 1 : 0);
        PlayerPrefs.GetInt("room2_wallOpen", GameStatus.GetInstance().GetWallState("room2") ? 1 : 0);

        // permanent upgrades
        PlayerPrefs.GetInt("room2_HealthIncreaseTaken", GameStatus.GetInstance().GetUpgradeState("room2", "Health") ? 1 : 0);
        PlayerPrefs.GetInt("room2_AmmoIncreaseTaken", GameStatus.GetInstance().GetUpgradeState("room2", "Ammo") ? 1 : 0);

        PlayerPrefs.Save();

    }
}
