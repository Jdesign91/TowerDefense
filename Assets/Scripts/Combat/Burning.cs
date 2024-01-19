using UnityEngine;

public struct Burning : Buff
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
            unitToAffect.health *= 0.9f;
        }

        duration -= Time.deltaTime;
    }

    public void removeEffect(Unit unitToAffect)
    {
        duration = maxDuation;
        hasBeenApplied = false;
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
        return new Burning();
    }
}

