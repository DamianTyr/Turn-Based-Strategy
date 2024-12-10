using System;

public interface IColonyActionTarget
{
    public void ProgressTask(int progressAmount, Action onTaskCompleted);
}
