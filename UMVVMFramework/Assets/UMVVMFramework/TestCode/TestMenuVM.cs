using MVVM;
using System;
using UnityEngine;

public class TestMenuVM : VMBase<TestMenuVM>
{
    #region Events
    public Action OnInteractableTestViewPressSuccess;
    #endregion

    [ViewModelToView] private NonInteractableTestPLD _nonInteractableTestPLD { get; set; }
    [ViewModelToView] private InteractableTestPLD _interactableTestPLD { get; set; }

    protected override void RegisterActivationEvents()
    {
    }

    protected override void UnregisterActivationEvents()
    {
    }

    protected override void OnActivateCustomActions()
    {
        _nonInteractableTestPLD = new NonInteractableTestPLD("Non Interactable Val");
        _interactableTestPLD = new InteractableTestPLD(true);

        NotifyPropertyChanged();
    }

    [ViewToViewModel]
    private void OnInteractableTestViewPressed()
    {
        Debug.Log("OnInteractableTestViewPressed");

        OnInteractableTestViewPressSuccess?.Invoke();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
            ActivateUI();

        if (Input.GetKeyUp(KeyCode.LeftAlt))
            DeactivateUI();

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
