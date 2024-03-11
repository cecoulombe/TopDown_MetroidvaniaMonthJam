using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompletionPercent : MonoBehaviour
{
    private Text percentage;
    private float completionPercent;

    private void OnEnable()
    {
        percentage = GetComponent<Text>();
        if (percentage == null)
        {
            Debug.Log("didnt get the text");
        }
        completionPercent = GameStatus.GetInstance().GetCompletionPercent();
        UpdateText();
    }

    //private void Update()
    //{
    //    UpdateText()
    //}

    private void UpdateText()
    {
        percentage.text = completionPercent + "%";
    }
}
