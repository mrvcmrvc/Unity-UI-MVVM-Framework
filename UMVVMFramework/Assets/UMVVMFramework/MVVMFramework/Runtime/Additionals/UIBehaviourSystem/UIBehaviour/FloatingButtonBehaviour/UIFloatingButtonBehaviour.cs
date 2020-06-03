using UITweening;

public class UIFloatingButtonBehaviour : UIBehaviourBase<UIFloatingButtonBehaviour>
{
    public UITweenRotation RotTween;
    public UITweenPosition PosTween;

    public override void Activate(params object[] parameters)
    {
        StartTransition(true, parameters);
    }

    public override void Deactivate(params object[] parameters)
    {
        StartTransition(false, parameters);
    }

    public override void ResetUI(bool isActivate, params object[] parameters)
    {
        IsActive = isActivate;

        RotTween.KillTween();
        PosTween.KillTween();

        if(IsActive)
        {
            RotTween.InitValueToTO();
            PosTween.InitValueToTO();
        }
        else
        {
            RotTween.InitValueToFROM();
            PosTween.InitValueToFROM();
        }
    }

    protected override void HideContent()
    {
    }

    protected override void ShowContent()
    {
    }

    //parameters: float duration, MMTweeningEaseEnum ease
    protected override void StartTransition(bool isActivate, params object[] parameters)
    {
        IsActive = isActivate;

        if(IsActive)
        {
            RotTween.PlayForward();
            PosTween.PlayForward();
        }
        else
        {
            RotTween.PlayReverse();
            PosTween.PlayReverse();
        }
    }
}
