using MVVM;
using System;
using System.Collections;
using UnityEngine;

public class TestMenuVM : VMBase<TestMenuVM>
{
    [ViewModelToView(typeof(NonInteractableTestPLD))]
    private NonInteractableTestPLD _nonInteractableTestPLD { get; set; }

    [ViewModelToView(typeof(InteractableTestPLD))]
    private InteractableTestPLD _interactableTestPLD { get; set; }

    private Action _callback;

    private void Start()
    {
        _callback?.Invoke();
    }

    protected override void RegisterActivationEvents(Action callback)
    {
        _callback = callback;
    }

    protected override void UnregisterActivationEvents(Action callback)
    {
    }

    protected override IEnumerator PreActivateAdditional()
    {
        _nonInteractableTestPLD = new NonInteractableTestPLD("Non Interactable Val");
        _interactableTestPLD = new InteractableTestPLD(true);

        NotifyPropertyChanged();

        return base.PreActivateAdditional();
    }

    [ViewToViewModel]
    private void OnInteractableTestViewPressed(bool val)
    {
        UnityEngine.Debug.Log("OnInteractableTestViewPressed");
    }














    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            _nonInteractableTestPLD = new NonInteractableTestPLD("Non Interactable Val 2");

            NotifyPropertyChanged();
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            _nonInteractableTestPLD = new NonInteractableTestPLD("Non Interactable Val 3");

            NotifyPropertyChanged();
        }
    }
}
