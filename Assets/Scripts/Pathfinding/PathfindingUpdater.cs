using System;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    public void Start()
    {
        Destructible.OnAnyDestroyed += DestructibleCrate_OnOnAnyDestroyed;
        Mineable.OnAnyMined += OnAnyMined;
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
