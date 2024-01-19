using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TowerInfo
{
    private const string DEFAULT_TOWER_PATH = "Prefabs/Towers/Default";

    public List<Buff> modifiers;
    public string projectileType;
    public string name;
    public float attackSpeed;
    public int range;
    public int cost;
    
    public string typeName;

    private string _resoucePath = "";
    public string resourcePath
    {
        get
        {
            if (string.IsNullOrEmpty(_resoucePath))
            {
                return DEFAULT_TOWER_PATH;
            }
            else
            {
                return _resoucePath;
            }
        }
        set
        {
            _resoucePath = value;
        }
    }
}

