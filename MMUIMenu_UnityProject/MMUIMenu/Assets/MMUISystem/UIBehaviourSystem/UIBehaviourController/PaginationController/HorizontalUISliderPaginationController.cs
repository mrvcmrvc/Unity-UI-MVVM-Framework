using MMUISystem.UIButton;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HorizontalUISliderPaginationController : UIBehaviourControllerBase<HorizontalUISliderPaginationBehaviour>
{
    public List<UnityUIButton> PageButtons;
    public ScrollRect PageScrollrect;
    public RectTransform HandleSlideArea;
    public RectTransform HandleIndicator;
    public HorizontalLayoutGroup PageButtonsHorLayoutGroup;
    public Slider HeaderSlider;
    public bool ActivateUIOcclusion;
    public float TransitionDuration;

    private float _headerWidth;
    private float _headerSlideAmountPerPage;
    private float _totalTransitionCountBTPages;
    private bool _isDirty;
    private int _activePage;
    private MMVirtualValueTweener _pageTweener;

    protected override void Awake()
    {
        base.Awake();

        Init();

        if (PageScrollrect.vertical)
            Debug.LogError(gameObject.name + " is set to vertical, please close vertical");

        ChangePage(Mathf.CeilToInt(PageButtons.Count / 2f));
    }

    private void Init()
    {
        _pageTweener = null;

        _isDirty = false;

        PageButtonsHorLayoutGroup.SetLayoutHorizontal();
        Canvas.ForceUpdateCanvases();

        _headerWidth = HeaderSlider.GetComponent<RectTransform>().rect.width;

        var buttonRect = PageButtons[0].GetComponent<RectTransform>().rect;
        HandleIndicator.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, buttonRect.height);
        HandleIndicator.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, buttonRect.width);

        _totalTransitionCountBTPages = PageButtons.Count - 1;
        _headerSlideAmountPerPage = 1f / _totalTransitionCountBTPages;

        PageScrollrect.horizontal = true;

        PageScrollrect.onValueChanged.AddListener(OnScrollViewDragged);

        if (ActivateUIOcclusion)
            BehaviourList.ForEach(b => b.ResetUI(false));

        PageButtons.ForEach(b => b.OnButtonPressDown += OnPageButtonPressed);

        float slideAreaWidth = _headerWidth - buttonRect.width;
        HandleSlideArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slideAreaWidth);
    }

    protected override void OnDestroy()
    {
        PageScrollrect.onValueChanged.RemoveListener(OnScrollViewDragged);

        PageButtons.ForEach(b => b.OnButtonPressDown -= OnPageButtonPressed);

        base.OnDestroy();
    }

    private void OnPageButtonPressed(PointerEventData eventData)
    {
        var pressedButton = PageButtons.Find(go => go.gameObject == eventData.selectedObject);
        int selectedPage = PageButtons.IndexOf(pressedButton) + 1;

        ChangePage(selectedPage);
    }

    private void ChangePage(int selectedPage)
    {
        _activePage = selectedPage;

        if (ActivateUIOcclusion)
            BehaviourList.ForEach(b => b.ResetUI(false));

        if (ActivateUIOcclusion)
            ActivatePageWithAdjacents();

        StopTweensIfActive();

        MMVirtualValueTweenerInfo pageTweenInfo = new MMVirtualValueTweenerInfo()
        {
            Callback = PageTweenUpdate,
            Duration = TransitionDuration,
            Ease = MMTweeningEaseEnum.InOutCubic,
            From = PageScrollrect.horizontalNormalizedPosition,
            To = _headerSlideAmountPerPage * (_activePage - 1),
            IgnoreTimeScale = true
        };
        
        _pageTweener = new MMVirtualValueTweener(pageTweenInfo);
        
        MMVirtualValueTweenController.Instance.StartTweener(_pageTweener);
    }

    private void ActivatePageWithAdjacents()
    {
        if(_activePage - 2 >= 0)
            BehaviourList[_activePage - 2].Activate();

        BehaviourList[_activePage - 1].Activate();

        if (_activePage < BehaviourList.Count)
            BehaviourList[_activePage].Activate();
    }

    private void PageTweenUpdate(float value, bool isFinished)
    {
        SetSliderValues(value);

        if (isFinished)
            _pageTweener = null;
    }

    void SetSliderValues(float value)
    {
        PageScrollrect.horizontalNormalizedPosition = value;
        HeaderSlider.normalizedValue = PageScrollrect.horizontalNormalizedPosition;
    }

    private void StopTweensIfActive()
    {
        if (_pageTweener != null)
        {
            MMVirtualValueTweenController.Instance.StopTweener(_pageTweener);
            _pageTweener = null;
        }
    }

    private void Update()
    {
        if(_isDirty & Input.GetMouseButtonUp(0))
        {
            _isDirty = false;

            var closestPageNo = GetPageNumber(HeaderSlider.normalizedValue);

            ChangePage(closestPageNo);
        }
    }

    public override void OnSubContainerPressDown(HorizontalUISliderPaginationBehaviour interactedContainer, PointerEventData eventData)
    {
        _isDirty = true;

        StopTweensIfActive();

        base.OnSubContainerPressDown(interactedContainer, eventData);
    }

    private void OnScrollViewDragged(Vector2 deltaDir)
    {
        SetSliderValues(PageScrollrect.horizontalNormalizedPosition);
    }

    private int GetPageNumber(float curHorValue)
    {
        float closestValue = 0f;
        for (float i = 0f; i <= 1f;)
        {
            if (Mathf.Abs(curHorValue - i) < Mathf.Abs(curHorValue - closestValue))
                closestValue = i;

            i += _headerSlideAmountPerPage;
        }

        return ((int)(closestValue / _headerSlideAmountPerPage)) + 1;
    }
}
