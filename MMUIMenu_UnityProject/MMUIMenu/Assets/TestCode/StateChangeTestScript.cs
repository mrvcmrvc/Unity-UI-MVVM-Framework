using MVVM;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StateChangeTestScript : MonoBehaviour
{
    public Slider SensivitySlider;
    public TextMeshProUGUI Text, ValueText;
    public UnityUIDraggableButton UIButton;

    private void Awake()
    {
        UIButton.OnButtonDoubleTapped += OnButtonDoubleTap;
        UIButton.OnButtonTapped += OnButtonTap;
        UIButton.OnButtonPressed += OnButtonPress;
        UIButton.OnButtonPressedDown += OnPressDown;
        UIButton.OnButtonPressedUp += OnButtonPressUp;
        UIButton.OnTappedAndHeld += OnTapAndHold;
        UIButton.OnButtonDraggedBegin += OnDragBegin;
        UIButton.OnButtonDragged += OnDrag;
        UIButton.OnButtonDraggedEnd += OnDragEnd;
    }

    void Update()
    {
        ValueText.text = SensivitySlider.value.ToString();

        UIButtonUtilities.SensivityInMilliseconds = (int)SensivitySlider.value;
    }

    private void OnTapAndHold(PointerEventData obj)
    {
        Text.text = "OnTapAndHold";
    }

    private void OnButtonTap(PointerEventData obj)
    {
        Text.text = "Tap";
    }

    private void OnButtonDoubleTap(PointerEventData obj)
    {
        Text.text = "DoubleTap";
    }

    private void OnButtonPress(PointerEventData obj)
    {
        Text.text = "Press";
    }

    private void OnButtonPressUp(PointerEventData obj)
    {
        Text.text = "PressUp";
    }

    private void OnPressDown(PointerEventData obj)
    {
        Text.text = "PressDown";
    }

    private void OnDragBegin(PointerEventData obj)
    {
        Text.text = "Drag Begin";
    }

    private void OnDrag(PointerEventData obj)
    {
        Text.text = "Drag";
    }

    private void OnDragEnd(PointerEventData obj)
    {
        Text.text = "Drag End";
    }
}
