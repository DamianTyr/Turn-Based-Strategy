using System;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public static Action<Furniture> OnAnySpawned;
    
    // Start is called before the first frame update
    void Start()
    {
        OnAnySpawned?.Invoke(this);
        Debug.Log("Furniture Spawned");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
