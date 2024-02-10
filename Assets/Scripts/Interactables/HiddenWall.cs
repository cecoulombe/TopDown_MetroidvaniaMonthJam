using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenWall : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_TopDown player = collision.GetComponent<PlayerController_TopDown>();
        if (player != null)
        {
            GameStatus.GetInstance().SetHiddenState(GameStatus.GetInstance().GetCurrentRoom());
        }
    }
}
