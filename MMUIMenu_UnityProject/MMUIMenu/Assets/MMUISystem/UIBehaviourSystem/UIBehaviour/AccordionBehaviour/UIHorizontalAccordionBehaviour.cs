using UnityEngine;

public class UIHorizontalAccordionBehaviour : UIBehaviourBase<UIHorizontalAccordionBehaviour>
{
    private bool _isActive;
    private MMVirtualValueTweener _activeTween;

    public override void Activate(params object[] parameters)
    {
        StartTransition(true, parameters);
    }

    public override void Deactivate(params object[] parameters)
    {
        StartTransition(false, parameters);
    }

    //parameters: float width
    public override void ResetUI(bool isActivate, params object[] parameters)
    {
        if (_activeTween != null)
        {
            MMVirtualValueTweenController.Instance.StopTweener(_activeTween);

            _activeTween = null;
        }

        _isActive = isActivate;

        RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GetNewVertSize((float)parameters[0]));

        if (_isActive)
            ShowContent();
        else
            HideContent();
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
        return InitialSize.x * sizeCoef;
    }

    //parameters: float width, float duration, MMTweeningEaseEnum ease
    protected override void StartTransition(bool isActivate, object[] parameters)
    {
        if (_activeTween != null)
        {
            MMVirtualValueTweenController.Instance.StopTweener(_activeTween);

            _activeTween = null;
        }

        _isActive = isActivate;

        if (!_isActive)
            HideContent();

        float width = (float)parameters[0];
        float duration = (float)parameters[1];
        MMTweeningEaseEnum ease = (MMTweeningEaseEnum)parameters[2];

        MMVirtualValueTweenerInfo tweenInfo = new MMVirtualValueTweenerInfo()
        {
            Duration = duration,
            Ease = ease,
            From = RectTransform.sizeDelta.x,
            IgnoreTimeScale = true,
            To = GetNewVertSize(width),
            Callback = OnTweenUpdated
        };

        MMVirtualValueTweener newTween = new MMVirtualValueTweener(tweenInfo);
        MMVirtualValueTweenController.Instance.StartTweener(newTween);
    }

    public void OnTweenUpdated(float value, bool isFinished)
    {
        RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);

        if (isFinished && _isActive)
            ShowContent();
    }
}
