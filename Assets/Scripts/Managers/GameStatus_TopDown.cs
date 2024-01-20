using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour
{
    // make sure this is the only instance of the game status, even though it isn't destroying on load
    static GameStatus instance;

    // store some player variables that will need to persist between loads
    #region Player variables    
    [SerializeField]
    protected float health;
    [SerializeField]
    protected float maxHealth;

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
        #endregion
    }

    private void OnDestroy()
    {
        Debug.Log("game status destroyed");
    }

    public float GetHealth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public static GameStatus GetInstance()
    {
        return instance;
    }
}
