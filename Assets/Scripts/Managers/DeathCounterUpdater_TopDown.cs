using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathCounterUpdater_TopDown : MonoBehaviour
{
    private void Update()
    {
        GetComponent<Text>().text = "Deaths: " + GameStatus.GetInstance().GetDeaths();
    }
}
