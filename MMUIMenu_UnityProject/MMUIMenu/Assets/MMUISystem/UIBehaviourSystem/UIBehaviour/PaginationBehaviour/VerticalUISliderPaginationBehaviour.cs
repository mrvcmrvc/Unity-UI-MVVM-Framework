public class VerticalUISliderPaginationBehaviour : UIBehaviourBase<VerticalUISliderPaginationBehaviour>
{
    public override void Activate(params object[] parameters)
    {
        ContentCanvasGroup.alpha = 1f;
    }

    public override void Deactivate(params object[] parameters)
    {
        ContentCanvasGroup.alpha = 0f;
    }

    public override void ResetUI(bool isActivate, params object[] parameters)
    {
        if (isActivate)
            Activate(parameters);
        else
            Deactivate(parameters);
    }

    protected override void HideContent()
    {
    }

    protected override void ShowContent()
    {
    }

    protected override void StartTransition(bool isActivate, params object[] parameters)
    {
    }
}
