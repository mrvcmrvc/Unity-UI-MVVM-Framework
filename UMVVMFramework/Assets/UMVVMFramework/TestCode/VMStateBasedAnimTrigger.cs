using MVVM;
using UnityEngine;

public class VMStateBasedAnimTrigger : UIAnimTriggerBase
{
    [SerializeField] private VMBase _targetViewModel;

    protected override void RegisterToVMEvents()
    {
        _targetViewModel.OnVMStateChanged += OnVMStateChanged;

    }

    protected override void UnregisterFromVMEvents()
    {
        _targetViewModel.OnVMStateChanged -= OnVMStateChanged;
    }

    private void OnVMStateChanged(EVMState state)
    {
        if (state.Equals(EVMState.Active))
            OnIntroTriggered?.Invoke();
        else
            OnOutroTriggered?.Invoke();
    }
}
