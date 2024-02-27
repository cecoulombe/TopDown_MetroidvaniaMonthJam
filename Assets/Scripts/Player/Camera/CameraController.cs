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

    #endregion

    void Start()
    {
        player = FindObjectOfType<PlayerController_TopDown>();
        lastPlayerPosition = player.transform.position;
    }

    void Update()
    {
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
}