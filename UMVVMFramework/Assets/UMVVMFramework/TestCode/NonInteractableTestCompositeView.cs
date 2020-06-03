using MVVM;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NonInteractableTestCompositePLD : IPLDBase
{
    public string Val { get; private set; }

    public NonInteractableTestCompositePLD(string val)
    {
        Val = val;
    }
}

public class NonInteractableTestCompositeView : NonInteractableCompositeViewBase<NonInteractableTestCompositePLD>
{
    protected override void ParsePLD(List<NonInteractableTestCompositePLD> pldColl, List<RectTransform> subContainerColl)
    {
        Debug.Log("Updating NonInteractableTest CompositeView");

        for (int i = 0; i < pldColl.Count; i++)
            subContainerColl[i].GetComponent<TextMeshProUGUI>().SetText(pldColl[i].Val);
    }
}
