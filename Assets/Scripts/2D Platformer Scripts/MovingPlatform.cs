using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Transform pointA;

    [SerializeField]
    private Transform pointB;

    [SerializeField]
    private float speed = 3.0f;

    private bool switching = false;
    private Transform target;
    #endregion

    private void FixedUpdate()
    {
        if (!switching)
        {
            target = pointB;
        }
        else if (switching)
        {
            target = pointA;
        }

        if (transform.position == pointB.position)
        {
            switching = true;
        }
        else if (transform.position == pointA.position)
        {
            switching = false;
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.transform.parent = this.transform;
            //Debug.Log("player has entered the platform");
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.transform.parent = null;
            //Debug.Log("player has exited the platform");

        }
    }
}