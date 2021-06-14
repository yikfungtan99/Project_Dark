using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    public delegate void TriggerAction();
    public event TriggerAction OnTriggered;

    public virtual void FireTrigger()
    {
        OnTriggered();
    }
}