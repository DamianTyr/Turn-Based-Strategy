using System;
using System.Collections.Generic;
using ColonyBuilding;
using Grid;
using Mission;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    public void Start()
    {
        Destructible.OnAnyDestroyed += DestructibleCrate_OnOnAnyDestroyed;
        Mineable.OnAnyMined += OnAnyMined;
        Mineable.OnAnySpawned += OnAnyMineableSpawned;
        Furniture.OnAnySpawned += OnAnyFurnitureSpawned;
    }

    private void OnAnyFurnitureSpawned(Furniture obj, List<GridPosition> occupiedGridPositionList)
    {
        foreach (GridPosition gridPosition in occupiedGridPositionList)
        {
            Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
        }
    }

    private void OnAnyMineableSpawned(GridPosition gridPosition, Mineable mineable)
    {
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }

    private void OnAnyMined(GridPosition gridPosition)
    {
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }

    private void DestructibleCrate_OnOnAnyDestroyed(object sender, EventArgs e)
    {
        Destructible destructible = sender as Destructible;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructible.GetGridPosition(), true);
    }
}
