using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Creep behaviour
public class Creep : Unit
{
    // What node index we're heading towards
    int currentPathIndex = 0;

    // Just to control directional speed
    float speedX = 0;
    float speedY = 0;

    // How fast we attack. Not too relevant sicne we currently 1 shot towers and bases
    float attackSpeed = 1;
    float timeSinceAttacked = 0;

    // So we can easily blow up towers?
    Damage damage;

    Vector3 newLocation;
    void Start()
    {
        buffs = new List<Buff>();
        // Only should be used to kill towers realy. Maybe a mind control tower in the future breaks this?
    }

    public override void acquireTarget(List<Unit> potentialTargets = null)
    {
        if (potentialTargets.Count > 0 && timeSinceAttacked > attackSpeed)
        {
            attack(potentialTargets[0]);
            timeSinceAttacked = 0;
        }
    }

    public override void attack(Unit unitToAttack)
    {
        // if we are facing creep.. do attack
        unitToAttack.onTakeDamage(damage);
    }

    public override void doDeathSequence()
    {
        // play death animation
        //gameObject.SetActive(false);
      
        
        GameObject.Destroy(this.gameObject);
    }

    // We probably should create a pather class and just use data from it.
    public override void doPathing(GameBoard map)
    {
        if (Vector3.Distance(map.pathFinder.path[currentPathIndex].realLocation, transform.localPosition) < 0.01)
        {
            if (map.pathFinder.path[currentPathIndex].occupied || map.pathFinder.path[currentPathIndex].isGoal)
            {
                // Just in case you place a tower RIGHT in fron of this creep as it's going under it, it wont blow it up unless that tower blocked the path.
                if (map.pathFinder.path[currentPathIndex].isGoal || map.pathFinder.isBlocked)
                { 
                    Unit thingInTheWay = map.nodes[(int)map.pathFinder.path[currentPathIndex].boardLocation.x, (int)map.pathFinder.path[currentPathIndex].boardLocation.y].occupyingUnit;
                    acquireTarget(new List<Unit>() { thingInTheWay });

                    if (thingInTheWay != null && thingInTheWay.health > 0)
                    {
                        return;
                    }
                }
            }

            currentPathIndex++;

            if (currentPathIndex >= map.pathFinder.path.Count)
            {
                currentPathIndex--;
            }
        }

        Mathf.Clamp(speed, 1, maxSpeed);

        if (map.pathFinder.path[currentPathIndex].realLocation.x > transform.localPosition.x)
        {
            speedX = Time.deltaTime * speed;
        }
        else if (map.pathFinder.path[currentPathIndex].realLocation.x < transform.localPosition.x)
        {
            speedX = -Time.deltaTime * speed;
        }

        if (map.pathFinder.path[currentPathIndex].realLocation.z > transform.localPosition.z)
        {
            speedY = Time.deltaTime * speed;
        }
        else if (map.pathFinder.path[currentPathIndex].realLocation.z < transform.localPosition.z)
        {
            speedY = -Time.deltaTime * speed;
        }

        newLocation = transform.localPosition;
        newLocation.x += speedX;
        newLocation.z += speedY;

        transform.localPosition = newLocation;
    }

    public override Vector2 getBoardLocation(GameBoard map)
    {
        Vector3 myWorldLoc = gameObject.transform.localPosition;

        // ok now we're 0-9 since a plane is technically a 9x9 piece of geometry
        myWorldLoc.x += 4.5f;
        myWorldLoc.z += 4.5f;

        float boardLocX = (myWorldLoc.x/9) * GameBoard.SIZE_OF_BOARD;
        float boardLocY = (myWorldLoc.z/9) * GameBoard.SIZE_OF_BOARD;

        return new Vector2((int)boardLocX, (int)boardLocY);
    }

    public override Vector2 getWorldLocation()
    {
        // just current X/Y. may not be useful
        return Vector2.zero;
    }

    public override void onTakeDamage(Damage damageInfo)
    {
        // Do a little animation, also do anything damage needs to do
        damageInfo.apply(this);

        if (health <= 0)
        {
            if (onUnitDeath != null)
            {
                Debug.LogError("Callin on unit death");
                onUnitDeath(this);
                onUnitDeath = null;
            }
            else
            {
                Debug.LogError("On unit death was null");
            }
            doDeathSequence();
        }
    }

    void Update()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            buffs[i].updateEffect(this);
        }

        for (int i = buffs.Count - 1; i >= 0; i--)
        {
            if (buffs[i].isExpired())
            {
                buffs[i].removeEffect(this);
                buffs.RemoveAt(i);
            }
        }

        timeSinceAttacked += Time.deltaTime;
    }

    public override void applyBuff(Buff buff)
    {
        buffs.Add(buff);
    }
}
