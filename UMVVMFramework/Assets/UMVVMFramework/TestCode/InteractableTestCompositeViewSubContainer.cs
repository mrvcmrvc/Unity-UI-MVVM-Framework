using MVVM;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableTestCompositeViewSubContainer : MonoBehaviour
{
    #region Events
    public Action OnButtonPressed;
    #endregion

    [SerializeField] private UnityUIButton _button;

    private void Awake()
    {
        _button.OnButtonPressedUp += OnButtonPress;
        _button.OnButtonTapped += OnButtonPress;
    }

    private void OnDestroy()
    {
        _button.OnButtonPressedUp -= OnButtonPress;
        _button.OnButtonTapped -= OnButtonPress;
    }

    private void OnButtonPress(PointerEventData eventData)
    {
        OnButtonPressed?.Invoke();
    }

    public void StartListening()
    {
        _button.StartListening();
    }

    public void StopListening()
    {
        _button.StopListening();
    }
}
