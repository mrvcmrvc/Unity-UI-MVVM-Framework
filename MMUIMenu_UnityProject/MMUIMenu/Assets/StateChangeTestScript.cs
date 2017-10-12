using System;
using MMUISystem.UIButton;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StateChangeTestScript : MonoBehaviour
{
    public Slider SensivitySlider;
    public TextMeshProUGUI Text, ValueText;
    public UnityUIButton UIButton;

    private void Awake()
    {
        UIButton.OnButtonPressDown += OnPressDown;
        UIButton.OnButtonPressUp += OnButtonPressUp;
        UIButton.OnButtonPress += OnButtonPress;
        UIButton.OnButtonDoubleTap += OnButtonDoubleTap;
        UIButton.OnButtonTap += OnButtonTap;
        UIButton.OnTapAndPress += OnTapAndPress;
    }

    void Update()
    {
        ValueText.text = SensivitySlider.value.ToString();

        UIButtonUtilities.SensivityInMilliseconds = (int)SensivitySlider.value;
    }

    private void OnTapAndPress(PointerEventData obj)
    {
        Text.text = "TapAndPress";
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
}
