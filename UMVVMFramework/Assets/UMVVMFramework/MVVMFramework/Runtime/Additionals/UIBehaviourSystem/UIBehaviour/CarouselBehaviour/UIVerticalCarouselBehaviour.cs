using UITweening;
using UnityEngine;

public class UIVerticalCarouselBehaviour : UIBehaviourBase<UIVerticalCarouselBehaviour>
{
    private bool _isActive;
    private UIVirtualValueTweener _activeTween;

    public override void Activate(params object[] parameters)
    {
        StartTransition(true, parameters);
    }

    public override void Deactivate(params object[] parameters)
    {
        StartTransition(false, parameters);
    }

    //parameters: float to
    public override void ResetUI(bool isActivate, params object[] parameters)
    {
        if (_activeTween != null)
        {
            UIVirtualValueTweenController.Instance.StopTweener(_activeTween);

            _activeTween = null;
        }

        _isActive = isActivate;

        if (!_isActive)
            HideContent();
        else
            ShowContent();

        RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetNewVertSize((float)parameters[0]));
    }

    protected override void ShowContent()
    {
        ContentCanvasGroup.alpha = 1f;
    }

    protected override void HideContent()
    {
        ContentCanvasGroup.alpha = 0f;
    }

    public float GetNewVertSize(float sizeCoef)
    {
        return InitialSize.y * sizeCoef;
    }

    //(For instant trans.)parameters: CarouselTransitionEnum transition, float to
    //(For float trans.)parameters: CarouselTransitionEnum transition, float duration, MMTweeningEaseEnum ease, float to, Vector2 pivot
    protected override void StartTransition(bool isActivate, object[] parameters)
    {
        _isActive = isActivate;

        if (!_isActive)
            HideContent();

        CarouselTransitionEnum transition = (CarouselTransitionEnum)parameters[0];

        if (transition == CarouselTransitionEnum.Instant)
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetNewVertSize((float)parameters[1]));

            if (_isActive)
                ShowContent();
        }
        else if (transition == CarouselTransitionEnum.Float)
        {
            float duration = (float)parameters[1];
            UITweeningEaseEnum ease = (UITweeningEaseEnum)parameters[2];
            float to = (float)parameters[3];
            Vector2 pivot = (Vector2)parameters[4];

            StartFloatTransition(duration, ease, to, pivot);
        }
    }

    #region Float Transition
    private void StartFloatTransition(float duration, UITweeningEaseEnum ease, float to, Vector2 pivot)
    {
        if (_activeTween != null)
        {
            UIVirtualValueTweenController.Instance.StopTweener(_activeTween);

            _activeTween = null;
        }

        RectTransform.SetPivotWithCounterAdjustPosition(pivot, InitialSize);

        UIVirtualValueTweenerInfo newTweenInfo = new UIVirtualValueTweenerInfo()
        {
            Callback = OnFloatTweenUpdated,
            Duration = duration,
            Ease = ease,
            IgnoreTimeScale = true,
            From = RectTransform.sizeDelta.y,
            To = GetNewVertSize(to)
        };

        _activeTween = new UIVirtualValueTweener(newTweenInfo);
        UIVirtualValueTweenController.Instance.StartTweener(_activeTween);
    }

    private void OnFloatTweenUpdated(float value, bool isFinished)
    {
        RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);

        if (isFinished && _isActive)
            ShowContent();

        if (isFinished)
            FireOnTweenFinished(_isActive);
    } 
    #endregion
}