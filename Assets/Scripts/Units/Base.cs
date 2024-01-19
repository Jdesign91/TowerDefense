using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base behviour (That is... behaviour of the base object)
public class Base : Unit
{
    public int money = 100;
    public bool destoryed = false;
    public int kills;
    private float moneyTimer = 0;
    public void getMoneyFromKill(Unit killedUnit)
    {
        kills++;
        money += killedUnit.value;
    }

    public void resetBase()
    {
        money = 0;
        kills = 0;
        moneyTimer = 0;
        destoryed = false;
    }
    public override void acquireTarget(List<Unit> potentialTargets)
    {
        // Bases dont have targets
    }

    public override void applyBuff(Buff buff)
    {
        // Maybe I can have buffs? Invul potion, etc?
    }

    public override void attack(Unit target)
    {
        // Bases dont attack
    }

    public override void doDeathSequence()
    {
       // Play some sort of linked animation
    }

    public override void doPathing(GameBoard map)
    {
        // No
    }

    // This could would should be passed from map when created
    public override Vector2 getBoardLocation(GameBoard map)
    {
        Vector3 myWorldLoc = gameObject.transform.localPosition;

        // ok now we're 0-9....
        myWorldLoc.x += 4.5f;
        myWorldLoc.z += 4.5f;

        float boardLocX = (myWorldLoc.x / 9) * GameBoard.SIZE_OF_BOARD;
        float boardLocY = (myWorldLoc.z / 9) * GameBoard.SIZE_OF_BOARD;


        return new Vector2((int)boardLocX, (int)boardLocY);
    }

    public override Vector2 getWorldLocation()
    {
        // this is just global pos... maybe dont bother
        return Vector2.zero;
    }

    public override void onTakeDamage(Damage damageInfo)
    {  
        damageInfo.apply(this);

        if (!destoryed)
        {
            Main.instance.loadDialog("Prefabs/UI/You Lose Screen");
        }

        destoryed = true;
    }

    private void Update()
    {
        moneyTimer += Time.deltaTime;
        if (moneyTimer > 1)
        {
            moneyTimer = 0;
            money++;
        }
    }
}
