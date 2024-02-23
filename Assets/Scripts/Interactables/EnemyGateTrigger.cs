using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class EnemyGateTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject Words3_EnemyGate;

    private bool triggered;

    private void Update()
    {
        // make the sprites invisible and turn off the colliders
        if (!triggered)
        {
            Words3_EnemyGate.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_TopDown player = collision.GetComponent<PlayerController_TopDown>();

        if(player != null)
        {
            triggered = true;
            Words3_EnemyGate.SetActive(true);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
