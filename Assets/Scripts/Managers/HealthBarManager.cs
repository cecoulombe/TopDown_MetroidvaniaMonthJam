using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    public Image healthBar;

    public void HealthBar()
    {
        healthBar.fillAmount = GameStatus.GetInstance().GetHealth() / GameStatus.GetInstance().GetMaxHealth();
    }

}
