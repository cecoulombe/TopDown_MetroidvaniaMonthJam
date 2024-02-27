using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Variables

    public PlayerController_TopDown player;

    private Vector3 lastPlayerPosition;

    private float distanceToMove;

    public bool canMoveHorizontal;

    public bool canMoveVertical;

    private bool hasHappened;

    #endregion

    void Start()
    {
        Debug.Log("camera awake");
        player = FindObjectOfType<PlayerController_TopDown>();
        lastPlayerPosition = player.transform.position;
        hasHappened = false;
    }

    void Update()
    {
        if(!hasHappened)
        {
            MoveCameraToPlayer();
            hasHappened = true;
        }
        if (canMoveHorizontal)
        {
            Debug.Log("moving the camera horizontally");

            distanceToMove = player.transform.position.x - lastPlayerPosition.x;

            transform.position = new Vector3(transform.position.x + distanceToMove, transform.position.y, transform.position.z);

        }
        if (canMoveVertical)
        {
            Debug.Log("moving the camera vertically");

            distanceToMove = player.transform.position.y - lastPlayerPosition.y;

            transform.position = new Vector3(transform.position.x, transform.position.y+ distanceToMove, transform.position.z);
        }

        lastPlayerPosition = player.transform.position;

    }

    private void MoveCameraToPlayer()
    {
        Debug.Log("the players current position is: " + player.transform.position + " and the camera's current position is: " + transform.position);
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}