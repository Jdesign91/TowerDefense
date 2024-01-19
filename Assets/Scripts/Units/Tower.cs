using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tower behaviour
public class Tower : Unit
{
    // Tower stats/resource locations
    public TowerInfo info;

    // How many tiles it takes up
    public Vector2 size = new Vector2(4, 4);

    float turnSpeed = 1f;
    float range = 0;
    float attackSpeed = 1;

    float timeSinceAttacked = 0;

    Unit currentTarget;
 
    // Projectile loaded on fly from config. Any tower can shoot any projectile!
    Projectile projectile;

    // Looks through the creep list for closest creep. Could be optimized by using an oct tree or having nodes track which creeps are on them.
    public override void acquireTarget(List<Unit> creepsToPickFrom)
    {
        // Search for closest creep.
        if (currentTarget == null || currentTarget.health <= 0 || !currentTarget.gameObject.activeSelf)
        {
            int closestCreep = -1;
            float bestDistance = float.MaxValue;
            float currDistance = 0;
            for (int i = 0; i < creepsToPickFrom.Count; i++)
            {
                currDistance = Vector3.Distance(creepsToPickFrom[i].transform.localPosition, transform.localPosition);

                if (currDistance < bestDistance)
                {
                    bestDistance = currDistance;
                    closestCreep = i;
                }
            }

            if (closestCreep >= 0 && creepsToPickFrom[closestCreep].health > 0)
            {
                currentTarget = creepsToPickFrom[closestCreep];
            }
        }
        else
        {
            // Determine which direction to rotate towards
            doPathing();
        }
    }

    public override void attack(Unit attackedUnit)
    {
        if (timeSinceAttacked < attackSpeed)
        {
            return;
        }

        if (currentTarget.health <= 0 || Vector3.Distance(currentTarget.transform.localPosition, transform.localPosition) > range)
        {
            currentTarget = null;
            return;
        }

        // We can have multiple projectiles in flight so lets load up another
        Projectile loadedProjectile = GameObject.Instantiate<Projectile>(projectile);
        ProjectileInfo ammoToUse = InfoCache.allProjectileInfo[info.projectileType];

        // Setup this projectile...
        loadedProjectile.onHitTarget = onProjectileHit;
        loadedProjectile.target = currentTarget.gameObject.transform.position;
        loadedProjectile.targetObject = currentTarget.transform;
        loadedProjectile.transform.position = transform.position;
        loadedProjectile.damageInfo = projectile.damageInfo;

        for (int i = 0; i < ammoToUse.modifiers.Count; i++)
        {
            loadedProjectile.damageInfo.modifiers.Add(InfoCache.staticBuffConstructors[ammoToUse.modifiers[i]]());
            loadedProjectile.damageInfo.modifiers[i].setDuration(ammoToUse.duration);
        }

        timeSinceAttacked = 0;
    }

    private void onProjectileHit(Projectile ammoHit)
    {
        // Actually construct damage item and apply debuffs/damage/etc
        if (currentTarget != null)
        {
            currentTarget.onTakeDamage(ammoHit.damageInfo);
        }
    }

    public override void doDeathSequence()
    {
        if (onUnitDeath != null)
        {
            onUnitDeath(this);
            onUnitDeath = null;
        }

        GameObject.Destroy(this.gameObject);
    }

    // Technically towers do not path, but they do pivot to attack things.
    public override void doPathing(GameBoard map = null)
    {
        Vector3 targetDirection = currentTarget.transform.localPosition - transform.localPosition;

        float dot = Vector3.Dot(transform.forward, targetDirection.normalized);

        if (dot > 0.99)
        {
            // We're on target, go ahead and attack
            attack(currentTarget);
            return;
        }

        // The step size is equal to speed times frame time.
        float singleStep = turnSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red);

        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public override Vector2 getBoardLocation(GameBoard map)
    {
        Vector3 myWorldLoc = gameObject.transform.localPosition;

        // 4.5 because a plane is -4.5 to 4.5 giving it a 9x9 size.
        myWorldLoc.x += 4.5f;
        myWorldLoc.z += 4.5f;

        float boardLocX = (myWorldLoc.x / 9) * GameBoard.SIZE_OF_BOARD;
        float boardLocY = (myWorldLoc.z / 9) * GameBoard.SIZE_OF_BOARD;

        return new Vector2((int)boardLocX, (int)boardLocY);
    }

    public override Vector2 getWorldLocation()
    {
        // just current X/Y. may not be useful
        return Vector2.zero;
    }

    public override void onTakeDamage(Damage damageInfo)
    {
        // Do a little animation
        damageInfo.apply(this);

        if (health <= 0)
        {
            doDeathSequence();
        }
    }

    // Start is called before the first frame updates
    void Start()
    {
        // If we get attacked we die.
        health = 0;

        attackSpeed = info.attackSpeed;
        range = info.range;

        // Load and cache projectile. Setup basic info;
        ProjectileInfo ammoToUse = InfoCache.allProjectileInfo[info.projectileType];
        projectile = Loader.loadObjectOfType<Projectile>(ammoToUse.asset, true);

        projectile.damageInfo = new Damage();
        projectile.damageInfo.damageAmount = ammoToUse.damage;
        projectile.damageInfo.modifiers = new List<Buff>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceAttacked += Time.deltaTime;
    }

    public override void applyBuff(Buff buff)
    {
        // Maybe towers get buffed?
    }
}
