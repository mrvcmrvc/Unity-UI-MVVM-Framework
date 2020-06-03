public class UIHorizontalInfiniteScrollViewBehaviour : UIBehaviourBase<UIHorizontalInfiniteScrollViewBehaviour>
{
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
    }

    protected override void HideContent()
    {
    }

    protected override void ShowContent()
    {
    }

    protected override void StartTransition(bool isActivate, params object[] parameters)
    {
        IsActive = isActivate;
    }
}
