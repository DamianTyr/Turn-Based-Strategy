using System;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    public void Start()
    {
        Destructible.OnAnyDestroyed += DestructibleCrate_OnOnAnyDestroyed;
    }

    private void DestructibleCrate_OnOnAnyDestroyed(object sender, EventArgs e)
    {
        Destructible destructible = sender as Destructible;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructible.GetGridPosition(), true);
    }
}
