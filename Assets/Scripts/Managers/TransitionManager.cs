using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    // this class will go on transitions to determine what room to load and what position in that room to load, which will be read by the reciving script

    [SerializeField]
    private string targetRoom;

    private string previousRoom;

    [SerializeField]
    private SceneLoader sceneLoader;

    private void OnTriggerEnter2D(Collider2D Player)
    {
        if(Player.CompareTag("Player"))
        {
            previousRoom = GameStatus.GetInstance().GetCurrentRoom();
            sceneLoader.LoadScene(targetRoom);
        }
    }
}
