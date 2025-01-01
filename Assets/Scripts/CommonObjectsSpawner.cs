using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonObjectsSpawner 
{
    [RuntimeInitializeOnLoadMethod]
    private static void InitializePersistentObjects()
    {
        var commonObjects = Resources.LoadAll("CommonObjects");
        foreach (GameObject commonObject in commonObjects) 
        {
            var obj = GameObject.Instantiate(commonObject);
            GameObject.DontDestroyOnLoad(obj); 
        }
    }
}
