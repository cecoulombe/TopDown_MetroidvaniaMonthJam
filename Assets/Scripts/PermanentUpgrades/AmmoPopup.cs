using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoPopUp : MonoBehaviour
{
    private float timer;

    [SerializeField]
    private Text popup;

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        // make it fade out after 2 seconds
        timer += Time.deltaTime;
        if (timer >= 2)
        {
            popup.enabled = false;
        }
    }
}
