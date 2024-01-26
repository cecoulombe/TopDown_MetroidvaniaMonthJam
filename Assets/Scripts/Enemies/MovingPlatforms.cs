using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Transform pointA;

    [SerializeField]
    private Transform pointB;

    [SerializeField]
    private float speed = 3.0f;

    private bool switching = false;
    private Transform walkPath;
    #endregion

    private void FixedUpdate()
    {
        if (!switching)
        {
            walkPath = pointB;
        }
        else if (switching)
        {
            walkPath = pointA;
        }

        if (transform.position == pointB.position)
        {
            switching = true;
        }
        else if (transform.position == pointA.position)
        {
            switching = false;
        }
        transform.position = Vector3.MoveTowards(transform.position, walkPath.position, speed * Time.deltaTime);
    }
}