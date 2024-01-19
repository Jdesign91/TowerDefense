using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// We could do different damage types that define apply differently, but in reality 
// each buff should probably do that. So for example... armor piercing is a buff that gets applied once, causes some damage, and is removed.
public struct Damage
{
    public int damageAmount;
    public List<Buff> modifiers;

    public void apply(Unit unitToDamage)
    {
        // 0 armor may as well be 1 armor. Anyway, lets avoid divide by 0 errors due to something like a bad config.
        unitToDamage.health -= Mathf.Clamp(damageAmount, 1, damageAmount) / Mathf.Clamp(unitToDamage.armorValue, 1, unitToDamage.armorValue);

        if (modifiers != null)
        { 
            for (int i = 0; i < modifiers.Count; i++)
            {
                unitToDamage.applyBuff(modifiers[i]);
            }
        }
    }
}
