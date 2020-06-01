using MVVM;
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

    [SerializeField] private ViewAnimController _animController;
    [SerializeField] private UnityUIButton _testButton;

    protected override void AwakeCustomActions()
    {
        _animController.OnAnimStateChanged += OnAnimStateChanged;
    }

    protected override void OnDestroyCustomActions()
    {
        _animController.OnAnimStateChanged -= OnAnimStateChanged;
    }

    private void OnAnimStateChanged(ViewAnimController.EViewAnimState state)
    {
        switch(state)
        {
            case ViewAnimController.EViewAnimState.PostIntro:
                _testButton.StartListening();
                break;
            case ViewAnimController.EViewAnimState.PreOutro:
                _testButton.StopListening();
                break;
        }
    }

    protected override void ParsePLD(InteractableTestPLD pld)
    {
        Debug.Log("Updating Interactable View");
    }

    protected override void RegisterEventsCustomActions()
    {
        _testButton.OnButtonPressedUp += OnButtonPressed;
        _testButton.OnButtonTapped += OnButtonPressed;
    }

    protected override void UnregisterEventsCustomActions()
    {
        _testButton.OnButtonPressedUp -= OnButtonPressed;
        _testButton.OnButtonTapped -= OnButtonPressed;
    }

    private void OnButtonPressed(PointerEventData eventData)
    {
        UpdateViewModel(_viewModelMethodNameColl[0]);
    }
}
