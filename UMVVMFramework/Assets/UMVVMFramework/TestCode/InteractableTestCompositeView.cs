using MVVM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableTestCompositePLD : IPLDBase
{
    public bool CanAfford;
    public bool IsButtonActive { get; private set; }

    public InteractableTestCompositePLD(bool isButtonActive)
    {
        IsButtonActive = isButtonActive;
    }
}

public class InteractableTestCompositeView : InteractableCompositeViewBase<InteractableTestCompositePLD>
{
    [SerializeField] private UIAnimController _uiAnimController;

    protected override List<string> _viewModelMethodNameColl { get { return new List<string>() { "OnInteractableTestCompositeViewPressed" }; } }

    private List<InteractableTestCompositeViewSubContainer> _subContainerColl;

    protected override void ParsePLD(List<InteractableTestCompositePLD> pldColl, List<RectTransform> subContainerColl)
    {
        Debug.Log("Updating Interactable Composite View");

        _subContainerColl = new List<InteractableTestCompositeViewSubContainer>();

        foreach (RectTransform subContainer in subContainerColl)
        {
            InteractableTestCompositeViewSubContainer castedSubCont = subContainer.GetComponent<InteractableTestCompositeViewSubContainer>();
            _subContainerColl.Add(castedSubCont);

            castedSubCont.OnButtonPressed -= OnButtonPressed;
            castedSubCont.OnButtonPressed += OnButtonPressed;

            if (_uiAnimController.CurState.Equals(UIAnimController.EUIAnimState.PostIntro))
                castedSubCont.StartListening();
        }
    }

    protected override void RegisterEventsCustomActions()
    {
        _uiAnimController.OnPostIntro += OnPostIntro;
        _uiAnimController.OnPreOutro += OnPreOutro;
    }

    protected override void UnregisterEventsCustomActions()
    {
        _uiAnimController.OnPostIntro -= OnPostIntro;
        _uiAnimController.OnPreOutro -= OnPreOutro;
    }

    private void OnPostIntro(UIAnimController animController)
    {
        _subContainerColl.ForEach(sc => sc.StartListening());
    }

    private void OnPreOutro(UIAnimController animController)
    {
        _subContainerColl.ForEach(sc => sc.StopListening());
    }

    private void OnButtonPressed()
    {
        UpdateViewModel(_viewModelMethodNameColl[0]);

        //_uiAnimController.TriggerOutroManually();
    }
}
