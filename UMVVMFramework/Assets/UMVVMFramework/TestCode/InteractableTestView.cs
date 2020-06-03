using MVVM;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableTestPLD : IPLDBase
{
    public bool CanAfford;
    public bool IsButtonActive { get; private set; }

    public InteractableTestPLD(bool isButtonActive)
    {
        IsButtonActive = isButtonActive;
    }
}

public class InteractableTestView : InteractableViewBase<InteractableTestPLD>
{
    protected override List<string> _viewModelMethodNameColl { get { return new List<string>() { "OnInteractableTestViewPressed" }; } }

    [SerializeField] private UIAnimController _uiAnimController;
    [SerializeField] private UnityUIButton _testButton;

    protected override void ParsePLD(InteractableTestPLD pld)
    {
        Debug.Log("Updating Interactable View");
    }

    protected override void RegisterEventsCustomActions()
    {
        _testButton.OnButtonPressedUp += OnButtonPressed;
        _testButton.OnButtonTapped += OnButtonPressed;

        _uiAnimController.OnPostIntro += OnPostIntro;
        _uiAnimController.OnPreOutro += OnPreOutro;
    }

    protected override void UnregisterEventsCustomActions()
    {
        _testButton.OnButtonPressedUp -= OnButtonPressed;
        _testButton.OnButtonTapped -= OnButtonPressed;

        _uiAnimController.OnPostIntro -= OnPostIntro;
        _uiAnimController.OnPreOutro -= OnPreOutro;
    }

    private void OnPostIntro()
    {
        Debug.Log("OnPostIntro");
        _testButton.StartListening();
    }

    private void OnPreOutro()
    {
        Debug.Log("OnPreOutro");
        _testButton.StopListening();
    }

    private void OnButtonPressed(PointerEventData eventData)
    {
        UpdateViewModel(_viewModelMethodNameColl[0]);

        //_uiAnimController.TriggerOutroManually();
    }
}
