using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType { HEAL, DAMAGE, PROJECTILE, AREAEFFECT }

public class Skill : Ability
{
    public int baseValue;
    public SkillType type;
    public int mpCost;

}
