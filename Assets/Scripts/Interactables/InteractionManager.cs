using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    // going to use this to manage any sort of interactions such as doors and chests (maybe will add in nps after)
    // the idea is to use a "weapon" idea similar to the melee attack but instead of attacking, if it overlaps, it will allow for interaction with the object, and the tag of the object will determine what the interaction is
    #region Variables
    [SerializeField]
    private bool isInteracting;

    private bool isChest;
    private bool isDoor;

    [SerializeField]
    private InteractableObject interactableObject;
    #endregion

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Chest"))
        {
            isInteracting = true;
            isChest = true;
            isDoor = false;
        }
        else if(collider.CompareTag("Door"))
        {
            isInteracting = true;
            isChest = false;
            isDoor = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        isInteracting = false;
        isChest = false;
        isDoor = false;
    }

    public void IsInteracting()
    {
        // is the itneractor currently hitting anything
        if(isInteracting)
        {
            Debug.Log("is interacting");
            if(isChest)
            {
                Debug.Log("chest");
                interactableObject.CheckChest();
            }
            else if(isDoor)
            {
                Debug.Log("door");
            }
        }
    }
}
