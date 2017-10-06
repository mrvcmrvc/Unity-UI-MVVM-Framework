using MMUISystem.UIButton;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StateChangeTestScript : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public UnityUIButton UIButton;

    private void Awake()
    {
        UIButton.OnButtonPressDown += OnPressDown;
        UIButton.OnButtonPressUp += OnButtonPressUp;
        UIButton.OnButtonPressCancel += OnButtonPressCancel;
        UIButton.OnButtonPress += OnButtonPress;
        UIButton.OnButtonDoubleTap += OnButtonDoubleTap;
        UIButton.OnTapAndPress += OnTapAndPress;
        UIButton.OnDelayedButtonPressDown += OnDelayedButtonPressDown;
    }

    private void OnDelayedButtonPressDown(PointerEventData obj)
    {
        Text.text = "OnDelayedButtonPressDown";
    }

    private void OnTapAndPress(PointerEventData obj)
    {
        Text.text = "OnTapAndPress";
    }

    private void OnButtonDoubleTap(PointerEventData obj)
    {
        Text.text = "OnButtonDoubleTap";
    }

    private void OnButtonPress(PointerEventData obj)
    {
        Text.text = "OnButtonPress";
    }

    private void OnButtonPressCancel(PointerEventData obj)
    {
        Text.text = "OnButtonPressCancel";
    }

    private void OnButtonPressUp(PointerEventData obj)
    {
        Text.text = "OnButtonPressUp";
    }

    private void OnPressDown(PointerEventData obj)
    {
        Text.text = "OnPressDown";
    }
}
