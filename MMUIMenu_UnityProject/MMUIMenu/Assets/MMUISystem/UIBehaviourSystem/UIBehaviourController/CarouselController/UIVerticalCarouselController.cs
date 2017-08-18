using UnityEngine;
using UnityEngine.EventSystems;

public class UIVerticalCarouselController : UIBehaviourControllerBase<UIVerticalCarouselBehaviour>
{
    public float Duration;
    [Range(0f, 1f)]
    public float MinSize;
    public RectTransform CarouselContainer;
    public MMTweeningEaseEnum Ease;
    public CarouselTransitionEnum Transition;

    protected override void Awake()
    {
        base.Awake();

        BehaviourList.ForEach(b => b.ResetUI(false, MinSize));
        BehaviourList[0].ResetUI(true, 1 - MinSize);
    }

    public override void OnSubContainerDragBegin(UIVerticalCarouselBehaviour interactedContainer, PointerEventData eventData)
    {
        interactedContainer.transform.SetAsLastSibling();

        UIVerticalCarouselBehaviour nextBehavContainer = GetNextBehavContainer(eventData, BehaviourList.IndexOf(interactedContainer));
        nextBehavContainer.transform.SetAsLastSibling();

        if (Transition != CarouselTransitionEnum.Instant)
        {
            interactedContainer.Deactivate(Transition, Duration, Ease, MinSize, GetPivotForContainer(eventData, false));

            float expandedHeight = 1 - MinSize;
            
            nextBehavContainer.Activate(Transition, Duration, Ease, expandedHeight, GetPivotForContainer(eventData, true));
        }
        else
        {
            interactedContainer.Deactivate(Transition, MinSize);
            
            nextBehavContainer.Activate(Transition, 1 - MinSize);
        }

        base.OnSubContainerDragBegin(interactedContainer, eventData);
    }

    private UIVerticalCarouselBehaviour GetNextBehavContainer(PointerEventData eventData, int closingContainerIndex)
    {
        int indexChange = -1;
        if (eventData.delta.y < 0)
            indexChange = 1;

        int nextBehavIndex = closingContainerIndex + indexChange;

        if (nextBehavIndex >= BehaviourList.Count)
            nextBehavIndex = 0;
        if (nextBehavIndex < 0)
            nextBehavIndex = BehaviourList.Count - 1;

        return BehaviourList[nextBehavIndex];
    }

    private Vector2 GetPivotForContainer(PointerEventData eventData, bool isAppear)
    {
        Vector2 newPivot = new Vector2(0.5f, 0f);

        if (eventData.delta.y < 0)
        {
            if (!isAppear)
                return new Vector2(0.5f, 0f);
            else
                return new Vector2(0.5f, 1f);
        }
        else
        {
            if (isAppear)
                return new Vector2(0.5f, 0f);
            else
                return new Vector2(0.5f, 1f);
        }
    }
}
