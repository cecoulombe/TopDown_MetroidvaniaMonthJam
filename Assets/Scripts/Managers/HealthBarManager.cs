using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Image healthBar;

    private void Update()
    {
        HealthBar();
    }

    public void HealthBar()
    {
        healthBar.fillAmount = GameStatus.GetInstance().GetHealth() / GameStatus.GetInstance().GetMaxHealth();
    }

}
