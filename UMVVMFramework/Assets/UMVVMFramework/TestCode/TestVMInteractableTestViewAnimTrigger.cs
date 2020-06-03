using MVVM;
using UnityEngine;

public class TestVMInteractableTestViewAnimTrigger : UIAnimTriggerBase
{
    [SerializeField] private TestMenuVM _targetViewModel;

    protected override void RegisterToVMEvents()
    {
        _targetViewModel.OnVMStateChanged += OnVMStateChanged;
        _targetViewModel.OnInteractableTestViewPressSuccess += OnInteractableTestViewPressSuccess;

    }

    protected override void UnregisterFromVMEvents()
    {
        _targetViewModel.OnVMStateChanged -= OnVMStateChanged;
        _targetViewModel.OnInteractableTestViewPressSuccess -= OnInteractableTestViewPressSuccess;
    }

    private void OnInteractableTestViewPressSuccess()
    {
        OnOutroTriggered?.Invoke();
    }

    private void OnVMStateChanged(EVMState state)
    {
        if (state.Equals(EVMState.Active))
            OnIntroTriggered?.Invoke();
        else
            OnOutroTriggered?.Invoke();
    }
}
