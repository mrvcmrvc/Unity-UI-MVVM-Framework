using MVVM;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TestMenuVM : VMBase<TestMenuVM>
{
    #region Events
    public Action OnInteractableTestViewPressSuccess;
    #endregion

    [ViewModelToView] private NonInteractableTestPLD _nonInteractableTestPLD { get; set; }
    [ViewModelToView] private InteractableTestPLD _interactableTestPLD { get; set; }
    [ViewModelToView] private List<NonInteractableTestCompositePLD> _nonInteractableTestCompositePLD { get; set; }
    [ViewModelToView] private List<InteractableTestCompositePLD> _interactableTestCompositePLD { get; set; }

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

        _nonInteractableTestCompositePLD = new List<NonInteractableTestCompositePLD>()
        {
            new NonInteractableTestCompositePLD("1"),
            new NonInteractableTestCompositePLD("2"),
            new NonInteractableTestCompositePLD("3")
        };

        _interactableTestCompositePLD = new List<InteractableTestCompositePLD>()
        {
            new InteractableTestCompositePLD(true),
            new InteractableTestCompositePLD(true),
            new InteractableTestCompositePLD(true)
        };

        NotifyPropertyChanged();
    }

    [ViewToViewModel]
    private void OnInteractableTestViewPressed()
    {
        Debug.Log("OnInteractableTestViewPressed");

        OnInteractableTestViewPressSuccess?.Invoke();
    }

    [ViewToViewModel]
    private void OnInteractableTestCompositeViewPressed()
    {
        Debug.Log("OnInteractableTestCompositeViewPressed");
    }

    #region Test Code
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            ActivateUI();

        if (Input.GetKeyUp(KeyCode.LeftAlt))
            DeactivateUI();

        #region View Test Code
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
        #endregion

        #region CompositeView Test Code
        if (Input.GetKeyUp(KeyCode.C))
        {
            _nonInteractableTestCompositePLD = new List<NonInteractableTestCompositePLD>()
            {
                new NonInteractableTestCompositePLD("1"),
                new NonInteractableTestCompositePLD("2"),
                new NonInteractableTestCompositePLD("3"),
                new NonInteractableTestCompositePLD("4")
            };

            NotifyPropertyChanged();
        }

        if (Input.GetKeyUp(KeyCode.V))
        {
            _nonInteractableTestCompositePLD = new List<NonInteractableTestCompositePLD>()
            {
                new NonInteractableTestCompositePLD("1"),
                new NonInteractableTestCompositePLD("2"),
            };

            NotifyPropertyChanged();
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            _interactableTestCompositePLD = new List<InteractableTestCompositePLD>()
            {
                new InteractableTestCompositePLD(true),
                new InteractableTestCompositePLD(true),
                new InteractableTestCompositePLD(true),
                new InteractableTestCompositePLD(true)
            };

            NotifyPropertyChanged();
        }

        if (Input.GetKeyUp(KeyCode.N))
        {
            _interactableTestCompositePLD = new List<InteractableTestCompositePLD>()
            {
                new InteractableTestCompositePLD(true),
                new InteractableTestCompositePLD(true),
            };

            NotifyPropertyChanged();
        }
        #endregion
    } 
    #endregion
}
