using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBarManager : MonoBehaviour
{
    public Image ammoBar;

    private void Update()
    {
        AmmoBar();
    }

    public void AmmoBar()
    {
        ammoBar.fillAmount = GameStatus.GetInstance().GetAmmo() / GameStatus.GetInstance().GetMaxAmmo();
    }

}
