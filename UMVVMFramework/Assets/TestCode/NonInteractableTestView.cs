using MVVM;
using TMPro;
using UnityEngine;

public class NonInteractableTestPLD : IPLDBase
{
    public string Val { get; private set; }

    public NonInteractableTestPLD(string val)
    {
        Val = val;
    }
}

public class NonInteractableTestView : NonInteractableViewBase<NonInteractableTestPLD>
{
    [SerializeField] private TextMeshProUGUI _text;

    protected override void ParsePLD(NonInteractableTestPLD pld)
    {
        Debug.Log("Updating NonInteractable View");

        _text.SetText(pld.Val);
    }
}
