using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour
{
    #region Variables
    // make sure this is the only instance of the game status, even though it isn't destroying on load
    static GameStatus instance;

    // count the number of deaths for the file
    protected int deathCounter = 0;

    #endregion
    // making a class which can keep track of certain game states (i.e. what abilities the player has, how many times they have died, if a gate has been opened at some point?)

    // keep track of player state (maybe using an enum? take a look at that video after sorting this out)

    // track individual room values (i.e. is a room's chest has been opened or if the enemy gate has been opened)

    private void Start()
    {
        #region Don't destroy on load/check for other instances
        if (instance != null)    // check if someone else is already this game object
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);   // become immortal
        #endregion
    }

    private void OnDestroy()
    {
        Debug.Log("game status destroyed");
    }

    public void AddDeath()
    {
        deathCounter += 1;
    }

    public int GetDeaths()
    {
        return deathCounter;
    }

    public static GameStatus GetInstance()
    {
        return instance;
    }
}
