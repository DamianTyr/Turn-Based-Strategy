using System;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    public void Start()
    {
        Destructible.OnAnyDestroyed += DestructibleCrate_OnOnAnyDestroyed;
        Mineable.OnAnyMined += OnAnyMined;
        Mineable.OnAnyMineableSpawned += OnAnyMineableSpawned;
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
