using UnityEngine;

public class MainMenu : UIMenu<MainMenu>
{
    protected override void FinishListeningEvents()
    {

    }

    public override void OnBackPressed()
    {
        Application.Quit();
    }

    protected override void StartListeningEvents()
    {

    }
}
