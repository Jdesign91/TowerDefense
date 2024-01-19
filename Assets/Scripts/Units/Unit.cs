using System.Collections.Generic;
using UnityEngine;
using System;

// Base class/member variables for units. The idea is to enforce a structure that allows units to interact easily, while
// providing anything you'd need to create a robust unit.
public abstract class Unit : MonoBehaviour
{
    public delegate void onDeath(Unit unit);
    public onDeath onUnitDeath;

    public float speed;
    public float maxSpeed;
    public float health;
    public int attackValue;
    public int armorValue;
    public int value;

    public List<Buff> buffs;
    public abstract void applyBuff(Buff buff);
    public abstract Vector2 getBoardLocation(GameBoard map);
    public abstract Vector2 getWorldLocation();
    public abstract void doDeathSequence();
    public abstract void attack(Unit target);
    public abstract void onTakeDamage(Damage damageInfo);
    public abstract void doPathing(GameBoard map);
    public abstract void acquireTarget(List<Unit> potentialTargets);
}

