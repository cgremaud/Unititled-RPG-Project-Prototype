using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { FIRE, ICE }

public class CombatUnit : Unit
{
    
    public int minDamage;
    public int maxDamage;
    public int damageModifier;
    public int hitModifier;
    public List<Skill> skills;
    public DamageType attackDamageType;
    public List<DamageType> resistances;
    public List<DamageType> weaknesses;
    public float resistanceModifier;


    public bool Attack(CombatUnit target)
    {
        if (target != null)
        {
            int diceRoll = Random.Range(1, 21);
            if (diceRoll + hitModifier >= target.armorClass)
            {
                if (target.resistances.Contains(attackDamageType))
                {
                    Debug.Log("Damage reduced!");
                    damageDealt = Random.Range(minDamage, maxDamage) + damageModifier * resistanceModifier;
                    target.ChangeHealth(-damageDealt);
                    return true;
                } else if (target.weaknesses.Contains(attackDamageType))
                {
                    Debug.Log("Damage increased!");
                    damageDealt = Random.Range(minDamage, maxDamage) + damageModifier / resistanceModifier;
                    target.ChangeHealth(-damageDealt);
                    return true;
                } 
                
                else
                {
                    damageDealt = Random.Range(minDamage, maxDamage) + damageModifier;
                    target.ChangeHealth(-damageDealt);
                    return true;
                }
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
