using System;
using UnityEngine;

public interface IColonyActionTarget
{
    public Vector3 transformPosition { get; set; }

    public void ProgressTask(int progressAmount, Action onTaskCompleted);
}
