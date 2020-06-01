using UITweening;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CarouselTransitionEnum
{
    Instant,
    Float
}

public class UIHorizontalCarouselController : UIBehaviourControllerBase<UIHorizontalCarouselBehaviour>
{
    public float Duration;
    [Range(0f, 1f)]
    public float MinSize;
    public int DefaultOpenedPage;
    public RectTransform CarouselContainer;
    public UITweeningEaseEnum Ease;
    public CarouselTransitionEnum Transition;

    protected override void Awake()
    {
        base.Awake();

        if (DefaultOpenedPage >= BehaviourList.Count)
            DefaultOpenedPage = 0;

        BehaviourList.ForEach(b => b.ResetUI(false, MinSize));
        BehaviourList[DefaultOpenedPage].ResetUI(true, 1 - MinSize);
    }

    public override void OnSubContainerDragBegin(UIHorizontalCarouselBehaviour interactedContainer, PointerEventData eventData)
    {
        interactedContainer.transform.SetAsLastSibling();

        UIHorizontalCarouselBehaviour nextBehavContainer = GetNextBehavContainer(eventData, BehaviourList.IndexOf(interactedContainer));
        nextBehavContainer.transform.SetAsLastSibling();

        if (Transition != CarouselTransitionEnum.Instant)
        {
            interactedContainer.Deactivate(Transition, Duration, Ease, MinSize, GetPivotForContainer(eventData, false));

            float expandedWidth = 1 - MinSize;

            nextBehavContainer.Activate(Transition, Duration, Ease, expandedWidth, GetPivotForContainer(eventData, true));
        }
        else
        {
            interactedContainer.Deactivate(Transition, MinSize);

            nextBehavContainer.Activate(Transition, 1 - MinSize);
        }

        base.OnSubContainerDragBegin(interactedContainer, eventData);
    }

    private UIHorizontalCarouselBehaviour GetNextBehavContainer(PointerEventData eventData, int closingContainerIndex)
    {
        int indexChange = -1;
        if (eventData.delta.x < 0)
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
        Vector2 newPivot = new Vector2(0f, 0.5f);

        if (eventData.delta.x < 0)
        {
            if(!isAppear)
                return new Vector2(0f, 0.5f);
            else
                return new Vector2(1f, 0.5f);
        }
        else
        {
            if (isAppear)
                return new Vector2(0f, 0.5f);
            else
                return new Vector2(1f, 0.5f);
        }
    }
}
