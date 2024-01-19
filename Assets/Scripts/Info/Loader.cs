using UnityEngine;

public static class Loader
{
    // Caches things as needed. An abstraction of the unity Resources.load function. You need to clear the cache manually if you ever want it cleared
    public static T loadObjectOfType<T>(string path, bool shouldCache = false) where T : UnityEngine.Object
    {
        T objectToLoad = null;
        if (InfoCache.cachedObjects.ContainsKey(path))
        {
            objectToLoad = (T)InfoCache.cachedObjects[path];
        }
        else
        {
            objectToLoad = Resources.Load<T>(path);
        }

        if (objectToLoad == null)
        {
            Debug.LogError("Could not load object at path " + path);
            return objectToLoad;
        }

        if (shouldCache)
        {
            InfoCache.cachedObjects[path] = objectToLoad;
        }
        return objectToLoad;
    }


}
