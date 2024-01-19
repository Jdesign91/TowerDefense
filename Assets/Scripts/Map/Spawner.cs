using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// Class repsonsible for managing all of our creeps
public class Spawner : MonoBehaviour
{
    public List<Unit> activeSpawns = new List<Unit>();

    // The original idea was to have multiple spawn locations configurable but... maybe some day, ran out of time!
    public List<Vector3> spawnLocations;

    // What wave we're on
    public int wave;

    // What creep we should spawn next
    public int creepIndex = 0;

    // seconds
    float spawnRate = 1;
    float timeSinceLastSpawn = 0;
    bool canSpawn = true;

    // Creeps in the wave to instantiate
    private List<Unit> creepsToSpawn = new List<Unit>();
    private Unit reusableCreep;

    // Tracks how many of the creeps in the current wave were spawned
    int spawnCount = 0;

    private void Start()
    {
        spawnLocations.Add(transform.localPosition);

        spawnRate = InfoCache.allWaveInfo[wave].spawnSpeed;
        for (int i = 0; i < InfoCache.allWaveInfo[wave].unitOrder.Count; i++)
        {
            creepsToSpawn.Add(Loader.loadObjectOfType<Unit>(InfoCache.allCreepInfo[InfoCache.allWaveInfo[wave].unitOrder[i]].resourcePath));

            creepsToSpawn[i].armorValue = InfoCache.allCreepInfo[InfoCache.allWaveInfo[wave].unitOrder[i]].armor;
            creepsToSpawn[i].value = InfoCache.allCreepInfo[InfoCache.allWaveInfo[wave].unitOrder[i]].value;
            creepsToSpawn[i].speed = InfoCache.allCreepInfo[InfoCache.allWaveInfo[wave].unitOrder[i]].speed;
            creepsToSpawn[i].maxSpeed = InfoCache.allCreepInfo[InfoCache.allWaveInfo[wave].unitOrder[i]].speed;
            creepsToSpawn[i].health = InfoCache.allCreepInfo[InfoCache.allWaveInfo[wave].unitOrder[i]].health;
        }
    }

    public void updateAndSpawnUnits(GameBoard spawningBoard)
    {
        for (int i = activeSpawns.Count - 1; i >= 0; i--)
        {
            if (activeSpawns[i] == null)
            {
                activeSpawns.RemoveAt(i);
            }

            if (activeSpawns.Count <= 0 && wave < InfoCache.allWaveInfo.Count)
            {
                canSpawn = true;
                wave++;
            }
            
            if (wave >= InfoCache.allWaveInfo.Count)
            {
                canSpawn = false;
                Main.instance.loadDialog("Prefabs/UI/You Win Screen");
            }
            else
            {
                spawnRate = InfoCache.allWaveInfo[wave].spawnSpeed;
            }
        }

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn > spawnRate && canSpawn && creepsToSpawn.Count > 0)
        {
            timeSinceLastSpawn = 0;

            for (int i = 0; i < spawnLocations.Count; i++)
            {
                spawnCount++;
                reusableCreep = GameObject.Instantiate<Unit>(creepsToSpawn[creepIndex], spawningBoard.transform);
                reusableCreep.transform.localPosition = spawnLocations[i];
                activeSpawns.Add(reusableCreep);

                creepIndex++;
                if (creepIndex >= creepsToSpawn.Count)
                {
                    creepIndex = 0;
                }

                // Avoid double add
                if (spawningBoard.baseToDefend != null)
                {
                    reusableCreep.onUnitDeath -= spawningBoard.baseToDefend.getMoneyFromKill;
                    reusableCreep.onUnitDeath += spawningBoard.baseToDefend.getMoneyFromKill;
                }

                if (spawnCount >= InfoCache.allWaveInfo[wave].unitCount)
                {
                    spawnCount = 0;
                    canSpawn = false;
                }
            }
        }

        for (int i = 0; i < activeSpawns.Count; i++)
        {
            activeSpawns[i].doPathing(spawningBoard);
        }
    }

    public void clearBoard()
    {
        for (int i = activeSpawns.Count - 1; i >= 0; i--)
        {
            if (activeSpawns[i] != null)
            {
                GameObject.Destroy(activeSpawns[i].gameObject);
            }
        }
    }

}
