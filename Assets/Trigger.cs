using Mirror;
using UnityEngine;

public abstract class Trigger : NetworkBehaviour
{
    public delegate void TriggerAction();
    public event TriggerAction OnTriggered;

    [ClientRpc]
    public virtual void RpcFireTrigger()
    {
        OnTriggered();
        if (AudioManager.Instance != null) AudioManager.Instance.Play("Lever");
    }
}