using UnityEngine;

public class UIListener : MonoBehaviour
{
    private void Start()
    {
        UIMenuManager.Instance.OpenUIMenu(MainMenu.Instance);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            UIMenuManager.Instance.OnBackPressed();

        if (Input.GetKeyDown(KeyCode.A))
            UIMenuManager.Instance.OpenUIMenu(MainMenu.Instance);

        if (Input.GetKeyDown(KeyCode.S))
            UIMenuManager.Instance.OpenUIMenu(PauseMenu.Instance);

        if (Input.GetKeyDown(KeyCode.D))
            UIMenuManager.Instance.OpenUIMenu(OverlayPauseMenu.Instance);
    }
}
