using System;
using UnityEngine;

struct Slow : Buff
{
    public bool hasBeenApplied;
    public float duration;
    public bool expired;
    private float maxDuation;

    public void updateEffect(Unit unitToAffect)
    {
        expired = duration == 0;
        hasBeenApplied = duration != maxDuation;

        if (!hasBeenApplied)
        {
            unitToAffect.speed = unitToAffect.maxSpeed - 5; ;
        }

        duration -= Time.deltaTime;
    }

    public void removeEffect(Unit unitToAffect)
    {
        duration = maxDuation;
        hasBeenApplied = false;
        unitToAffect.speed += 5;
    }

    public bool isExpired()
    {
        return expired;
    }

    public void setDuration(float newDuration)
    {
        maxDuation = newDuration;
        duration = newDuration;
    }

    public static Buff getBuffConstructor()
    {
        return new Slow();
    }
}