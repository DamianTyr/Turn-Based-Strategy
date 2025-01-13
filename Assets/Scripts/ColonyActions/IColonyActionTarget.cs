using System;
using UnityEngine;

namespace ColonyActions
{
    public interface IColonyActionTarget
    {
        public Vector3 transformPosition { get; set; }

        public void ProgressTask(int progressAmount, Action onTaskCompleted);
    }
}
