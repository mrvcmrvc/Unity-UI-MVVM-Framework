using UnityEngine;
using UnityEngine.EventSystems;

public class UIVerticalAccordionController : UIBehaviourControllerBase<UIVerticalAccordionBehaviour>
{
    [Range(0f, 1f)]
    public float MinSize;
    public float Duration;
    public RectTransform AccordionContainer;
    public MMTweeningEaseEnum Ease;

    protected override void Awake()
    {
        base.Awake();

        BehaviourList.ForEach(b => b.ResetUI(false, MinSize));
        BehaviourList[0].ResetUI(true, 1 - (MinSize * (BehaviourList.Count - 1)));
    }

    public override void OnSubContainerPressDown(UIVerticalAccordionBehaviour interactedContainer, PointerEventData eventData)
    {
        float expendedHight = 1 - (MinSize * (BehaviourList.Count - 1));

        interactedContainer.Activate(expendedHight, Duration, Ease);

        BehaviourList.FindAll(b => b != interactedContainer).ForEach(b => b.Deactivate(MinSize, Duration, Ease));

        base.OnSubContainerPressDown(interactedContainer, eventData);
    }
}
