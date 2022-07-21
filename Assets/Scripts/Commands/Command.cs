using UnityEngine;
public abstract class Command
{
    public string name;
    public abstract void Execute();
    public abstract bool IsCompleted();
    public abstract void SetComplete();
}