using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class InfoCache
{
    public static List<TowerInfo> allTowerInfo = new List<TowerInfo>();
    public static Dictionary<string, CreepInfo> allCreepInfo = new Dictionary<string, CreepInfo>();
    public static Dictionary<string, ProjectileInfo> allProjectileInfo = new Dictionary<string, ProjectileInfo>();
    public static List<WaveInfo> allWaveInfo = new List<WaveInfo>();

    public static Dictionary<string, UnityEngine.Object> cachedObjects = new Dictionary<string, UnityEngine.Object>() ;

    // These dictionaries are pre-populated so we can look stuff up easily. It would be up to devs to add things here..
    // A rules system may work better in the long term...
    public static Dictionary<string, Func<Buff>> staticBuffConstructors = new Dictionary<string, Func<Buff>>()
    {
        { "burning", Burning.getBuffConstructor},
        { "slow", Slow.getBuffConstructor}
    };
        
        // We probably dont need a function for this but it's nice to have it apparent that we need to do it.
    public static void clearCachedObjects()
    {
        cachedObjects.Clear();
    }
}

