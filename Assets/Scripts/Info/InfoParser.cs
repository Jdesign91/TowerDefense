using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class InfoParser
{
    private const string DEFAULT_CREEP_CONFIG_LOCATION = "Data/creep";
    private const string DEFAULT_AMMO_CONFIG_LOCATION = "Data/ammo";
    private const string DEFAULT_TOWER_CONFIG_LOCATION = "Data/tower";
    private const string DEFAULT_WAVE_CONFIG_LOCATION = "Data/waves";

    TextAsset textToLoad;
    string[] txtLines;
    string[] args;

    public void setupCreeps(string path  = DEFAULT_CREEP_CONFIG_LOCATION)
    {
        InfoCache.allCreepInfo.Clear();
        Debug.LogError("Setup creeps");
        textToLoad = Loader.loadObjectOfType<TextAsset>(path);
        txtLines = textToLoad.text.Split('\n');

        CreepInfo reusableInfo = null;

        for (int i = 0; i < txtLines.Length; i++)
        {
            txtLines[i] = txtLines[i].ToLower();
            txtLines[i] = txtLines[i].Replace(" ", string.Empty);
            txtLines[i] = txtLines[i].Trim();

            if (txtLines[i] == "creep")
            {
                if (reusableInfo != null && reusableInfo.name != null)
                { 
                    InfoCache.allCreepInfo.Add(reusableInfo.name, reusableInfo); 
                }
                reusableInfo = new CreepInfo();
            }
            else
            {
                args = txtLines[i].Split(':');

                switch(args[0])
                {
                    case "name":
                        reusableInfo.name = args[1];
                        break;
                    case "speed":
                        reusableInfo.speed = int.Parse(args[1]);
                        break;
                    case "armor":
                        reusableInfo.armor = int.Parse(args[1]);
                        break;
                    case "health":
                        reusableInfo.health = float.Parse(args[1]);
                        break;
                    case "asset":
                        reusableInfo.resourcePath = (args[1]);
                        break;
                    case "value":
                        reusableInfo.value = int.Parse(args[1]);
                        break;

                }

            }
        }


        if (reusableInfo != null && reusableInfo.name != null)
        {
            InfoCache.allCreepInfo.Add(reusableInfo.name, reusableInfo);
        }
    }

    public void setupTowers(string path = DEFAULT_TOWER_CONFIG_LOCATION)
    {
        InfoCache.allTowerInfo.Clear();
        textToLoad = Loader.loadObjectOfType<TextAsset>(path);
        txtLines = textToLoad.text.Split('\n');

        TowerInfo reusableInfo = null;

        for (int i = 0; i < txtLines.Length; i++)
        {
            txtLines[i] = txtLines[i].ToLower();
            txtLines[i] = txtLines[i].Replace(" ", string.Empty);
            txtLines[i] = txtLines[i].Trim();

            if (txtLines[i] == "tower") 
            {
                if (reusableInfo != null && reusableInfo.name != null)
                {
                    InfoCache.allTowerInfo.Add(reusableInfo); ;
                }
                reusableInfo = new TowerInfo();
            }
            else
            {
                args = txtLines[i].Split(':');
                switch (args[0])
                {
                    case "name":
                        reusableInfo.name = args[1];
                        break;
                    case "speed":
                        reusableInfo.attackSpeed = int.Parse(args[1]);
                        break;
                    case "projectile":
                        reusableInfo.projectileType = (args[1]);
                        break;
                    case "asset":
                        reusableInfo.resourcePath = (args[1]);
                        break;
                    case "cost":
                        reusableInfo.cost = int.Parse(args[1]);
                        break;
                    case "range":
                        reusableInfo.range = int.Parse(args[1]);
                        break;
                }

            }
        }

        if (reusableInfo != null && reusableInfo.name != null)
        {
            InfoCache.allTowerInfo.Add(reusableInfo);
        }

    }

    public void setupAmmo(string path = DEFAULT_AMMO_CONFIG_LOCATION)
    {
        InfoCache.allProjectileInfo.Clear();
        string[] potentialModifiers;

        textToLoad = Loader.loadObjectOfType<TextAsset>(path);

        txtLines = textToLoad.text.Split('\n');

        ProjectileInfo reusableInfo = null;

        for (int i = 0; i < txtLines.Length; i++)
        {
            txtLines[i] = txtLines[i].ToLower();
            txtLines[i] = txtLines[i].Replace(" ", string.Empty);
            txtLines[i] = txtLines[i].Trim();

            if (txtLines[i] == "ammo")
            {
                if (reusableInfo != null && reusableInfo.name != null)
                {
                    InfoCache.allProjectileInfo.Add(reusableInfo.name, reusableInfo); ;
                }
                reusableInfo = new ProjectileInfo();
            }
            else
            {
                args = txtLines[i].Split(':');
                switch (args[0])
                {
                    case "name":
                        reusableInfo.name = args[1];
                        break;
                    case "damage":
                        reusableInfo.damage = int.Parse(args[1]);
                        break;
                    case "asset":
                        reusableInfo.asset = (args[1]);
                        break;
                    case "duration":
                        reusableInfo.duration = int.Parse(args[1]);
                        break;
                    case "modifier":
                        potentialModifiers = args[1].Split(',');

                        for (int j = 0; j < potentialModifiers.Length; j++)
                        {
                            reusableInfo.modifiers.Add(potentialModifiers[j]);
                        }
                        break;
                }

            }
        }

        if (reusableInfo != null && reusableInfo.name != null)
        {
            InfoCache.allProjectileInfo.Add(reusableInfo.name, reusableInfo);
        }
        
    }

    public void setupWaves(string path = DEFAULT_WAVE_CONFIG_LOCATION)
    {
        InfoCache.allWaveInfo.Clear();
        string[] creepTypes;

        textToLoad = Loader.loadObjectOfType<TextAsset>(path);

        txtLines = textToLoad.text.Split('\n');

        WaveInfo reusableInfo = null;

        for (int i = 0; i < txtLines.Length; i++)
        {
            txtLines[i] = txtLines[i].ToLower();
            txtLines[i] = txtLines[i].Replace(" ", string.Empty);
            txtLines[i] = txtLines[i].Trim();

            // Ok store creep info...
            if (txtLines[i] == "wave")
            {
                if (reusableInfo != null && reusableInfo.unitCount  > 0)
                {
                    InfoCache.allWaveInfo.Add(reusableInfo);
                }
                reusableInfo = new WaveInfo();
            }
            else
            {
                args = txtLines[i].Split(':');
                switch (args[0])
                {
                    case "count":
                        reusableInfo.unitCount = int.Parse(args[1]);
                        break;

                    case "spawnspeed":
                        reusableInfo.spawnSpeed = int.Parse(args[1]);
                        break;

                    case "creep":
                        creepTypes = args[1].Split(',');

                        for (int j = 0; j < creepTypes.Length; j++)
                        {
                            reusableInfo.unitOrder.Add(creepTypes[j]);
                        }
                        break;

                }

            }
        }

        if (reusableInfo != null && reusableInfo.unitCount > 0)
        {
            InfoCache.allWaveInfo.Add(reusableInfo);
        }

    }

}

