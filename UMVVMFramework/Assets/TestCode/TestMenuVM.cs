using MVVM;
using System.Collections;
using UnityEngine;

public class TestMenuVM : VMBase<TestMenuVM>
{
    [ViewModelToView] private NonInteractableTestPLD _nonInteractableTestPLD { get; set; }
    [ViewModelToView] private InteractableTestPLD _interactableTestPLD { get; set; }

    private void Start()
    {
        ActivateUI();
    }

    protected override void RegisterActivationEvents()
    {
    }

    protected override void UnregisterActivationEvents()
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
        Debug.Log("OnInteractableTestViewPressed");
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
