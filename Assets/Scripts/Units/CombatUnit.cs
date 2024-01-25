using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnit : Unit
{
    public int minDamage;
    public int maxDamage;
    public int damageModifier;
    public int hitModifier;
    public List<Skill> skills;
    

    public override bool Attack(Unit target)
    {
        if (target != null)
        {
            int diceRoll = Random.Range(1, 21);
            if (diceRoll + hitModifier >= target.armorClass)
            {
                damageDealt = Random.Range(minDamage, maxDamage) + damageModifier;
                target.ChangeHealth(-damageDealt);
                return true;
            } else {
                return false;
            }
        } else { return false; }
    }

    public virtual void UseSkill()
    {
        Debug.Log("UseSkill called");
    }
}
