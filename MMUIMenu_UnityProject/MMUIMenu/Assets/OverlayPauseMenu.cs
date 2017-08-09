using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayPauseMenu : UIMenu<OverlayPauseMenu>
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