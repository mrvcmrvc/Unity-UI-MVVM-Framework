using System;

public class PauseMenu : UIMenu<PauseMenu>
{
    protected override void FinishListeningEvents()
    {

    }

    public override void OnBackPressed()
    {
        UIMenuManager.Instance.CloseUIMenu(Instance);
    }

    protected override void StartListeningEvents()
    {

    }
}
