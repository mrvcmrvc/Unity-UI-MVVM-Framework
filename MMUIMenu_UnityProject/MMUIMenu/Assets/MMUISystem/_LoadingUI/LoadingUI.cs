using TMPro;
using UnityEngine;

public class LoadingUI : MonoBehaviour
{
    public TextMeshProUGUI ErrorMessage, LoadingMessage;

    public void InitAndActivate()
    {
        gameObject.SetActive(true);

        ErrorMessage.text = "";
        ErrorMessage.gameObject.SetActive(false);

        LoadingMessage.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void ShowError(string errorMessage)
    {
        ErrorMessage.text = errorMessage;

        ErrorMessage.gameObject.SetActive(true);

        LoadingMessage.gameObject.SetActive(false);
    }
}
