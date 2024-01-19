using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CreepInfo
{
    private const string DEFAULT_CREEP_PATH = "Prefabs/Towers/Default";

    public List<Buff> modifiers;
    public string name;
    public float health;
    public float speed;
    public int armor;
    public int value;

    private string _resoucePath = "";
    public string resourcePath
    {
        get
        {
            if (string.IsNullOrEmpty(_resoucePath))
            {
                return DEFAULT_CREEP_PATH;
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

