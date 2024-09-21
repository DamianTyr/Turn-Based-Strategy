using System;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    public void Start()
    {
        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnOnAnyDestroyed;
    }

    private void DestructibleCrate_OnOnAnyDestroyed(object sender, EventArgs e)
    {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
    }
}
