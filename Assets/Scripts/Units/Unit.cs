using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;
    public float maxHp;
    public float currentHp;
    public float maxMp;
    public float currentMp;
    public float damageDealt;
    public float amountHealed;
    public float armorClass;

    //will be moved down to combat unit 
    public virtual bool Attack (Unit target)
    {
        if (target != null )
        {
            target.ChangeHealth(-damageDealt);
            return true;
        } else
        {
            return false;
        }
    }

    public void Heal (Unit target)
    {
        if (target != null)
        {
            target.ChangeHealth(amountHealed);
        }
    }

    public void ChangeHealth(float amount)
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0, maxHp);
    }
}