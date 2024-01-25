using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI lvlText;
    public Slider hpBar;
    public Slider mpBar;

    public void SetHUD(Unit unit) 
    { 
        nameText.text = unit.unitName;
        lvlText.text = "Lvl: " + unit.unitLevel;
        hpBar.maxValue = unit.maxHp;
        hpBar.value = unit.currentHp;
        mpBar.maxValue = unit.maxMp;
        mpBar.value = unit.currentMp;
    }

    public void SetHp(float hp)
    {
        hpBar.value = hp;
    }

    public void SetMp(float mp)
    {
        mpBar.value = mp;
    }

   
}
