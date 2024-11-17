using System;
using UnityEngine;

public class Mineable : MonoBehaviour
{
    public static Action OnAnyMineableSpawned;

    private void Start()
    {
        OnAnyMineableSpawned?.Invoke();
    }
}
