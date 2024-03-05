using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDoNotDestroy : MonoBehaviour
{
    // create a do not destroy so that the music persists between scenes without restarting the track for each load

    private void Awake()
    {
        GameObject[] musicObject = GameObject.FindGameObjectsWithTag("DepressionMusic");

        if(musicObject.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
