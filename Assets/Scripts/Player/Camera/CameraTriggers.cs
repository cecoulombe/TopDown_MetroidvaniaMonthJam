using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraTriggers : MonoBehaviour
{
    private CameraController mainCam;

    [SerializeField]
    private bool isHorizontalTrigger;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    PlayerController_TopDown player = collision.GetComponent<PlayerController_TopDown>();

    //    if (player != null)
    //    {
    //        if (isHorizontalTrigger && !mainCam.canMoveHorizontal)
    //        {
    //            Debug.Log("unlocking camera horizontal");
    //            mainCam.canMoveHorizontal = true;
    //        }
    //        else if (!isHorizontalTrigger && !mainCam.canMoveVertical)
    //        {
    //            Debug.Log("unlocking camera vertical");
    //            mainCam.canMoveVertical = true;
    //        }
    //        else if (isHorizontalTrigger && mainCam.canMoveHorizontal)
    //        {
    //            Debug.Log("locking camera horizontal");
    //            mainCam.canMoveHorizontal = false;
    //        }
    //        else if (!isHorizontalTrigger && mainCam.canMoveVertical)
    //        {
    //            Debug.Log("locking camera vertical");
    //            mainCam.canMoveVertical = false;
    //        }
    //    }
    //}

    private void Start()
    {
        mainCam = FindObjectOfType<CameraController>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController_TopDown player = collision.GetComponent<PlayerController_TopDown>();

        if (player != null)
        {
            if (isHorizontalTrigger && !mainCam.canMoveHorizontal)
            {
                Debug.Log("unlocking camera horizontal");
                mainCam.canMoveHorizontal = true;
            }
            else if (!isHorizontalTrigger && !mainCam.canMoveVertical)
            {
                Debug.Log("unlocking camera vertical");
                mainCam.canMoveVertical = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController_TopDown player = collision.GetComponent<PlayerController_TopDown>();

        if (player != null)
        {
            if (isHorizontalTrigger && mainCam.canMoveHorizontal)
            {
                Debug.Log("unlocking camera horizontal");
                mainCam.canMoveHorizontal = false;
            }
            else if (!isHorizontalTrigger && mainCam.canMoveVertical)
            {
                Debug.Log("unlocking camera vertical");
                mainCam.canMoveVertical = false;
            }
        }
    }
}
