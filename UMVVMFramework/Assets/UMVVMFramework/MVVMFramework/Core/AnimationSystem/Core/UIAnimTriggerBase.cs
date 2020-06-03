using System;
using UnityEngine;

public abstract class UIAnimTriggerBase : MonoBehaviour
{
    public Action OnIntroTriggered;
    public Action OnOutroTriggered;

    private void Awake()
    {
        RegisterToVMEvents();

        OnAwakeCustomActions();
    }

    private void OnDestroy()
    {
        UnregisterFromVMEvents();

        OnDestroyCustomActions();
    }

    protected virtual void OnAwakeCustomActions() { }
    protected virtual void OnDestroyCustomActions() { }

    protected abstract void RegisterToVMEvents();
    protected abstract void UnregisterFromVMEvents();
}
