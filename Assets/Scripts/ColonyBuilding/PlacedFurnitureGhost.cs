using System;
using System.Collections;
using System.Collections.Generic;
using Colony;
using UnityEngine;

public class PlacedFurnitureGhost : MonoBehaviour
{
    public static Action<PlacedFurnitureGhost> OnAnySpawned;
    private GridPosition GridPosition;
    private int health = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        OnAnySpawned?.Invoke(this);

        GridPosition = ColonyGrid.Instance.GetGridPosition(transform.position);
        ColonyGrid.Instance.SetFurnitureGhostAtGridPosition(GridPosition, this);
        ColonyTasksManager.Instance.RegisterTask(GridPosition, ColonyActionType.Building);
        Debug.Log("Placed Furniture Ghost Spawned");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public void ProgressTask(int progressAmount, Action onTaskCompleted)
    {
        health -= progressAmount;
        Debug.Log("Progressing Task");
        if (health <= 0)
        {
            Debug.Log("Finished Building Task");
            onTaskCompleted();
        }
    }
    
    //HaveMethod Where it gets constructed
    
    //Once Constructed, spawn Furniture and self destruct
}
