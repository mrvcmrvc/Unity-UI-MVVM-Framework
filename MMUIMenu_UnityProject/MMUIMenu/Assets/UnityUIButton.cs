using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("UI/Extensions/UIButton")]
[RequireComponent(typeof(CanvasRenderer), typeof(Image), typeof(Button)), DisallowMultipleComponent]
public abstract class UnityUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool StartListeningOnEnable;

    bool _isListening;

    #region Events
    public Action OnButtonPressDown;
    void FireOnButtonPressDown()
    {
        if (OnButtonPressDown != null)
            OnButtonPressDown();
    }

    public Action OnButtonPressUp;
    void FireOnButtonPressUp()
    {
        if (OnButtonPressUp != null)
            OnButtonPressUp();
    }

    public Action OnButtonDoubleTap;
    void FireOnButtonDoubleTap()
    {
        if (OnButtonDoubleTap != null)
            OnButtonDoubleTap();
    }
    #endregion

    protected virtual void OnEnable()
    {
        if (StartListeningOnEnable)
            StartListening();
    }

    protected virtual void OnDisable()
    {
        if (StartListeningOnEnable)
            StopListening();
    }

    public virtual void StartListening()
    {
        _isListening = true;
    }

    public virtual void StopListening()
    {
        _isListening = false;
    }

    protected virtual void OnPressUp() { }
    protected virtual void OnPressDown() { }
    protected virtual void OnDoubleTap() { }

    /// <summary>
    /// DO NOT use this directly! Instead use OnPressDown()
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if(_isListening)
        {
            if (eventData.dragging)
                return;

            FireOnButtonPressDown();

            OnPressDown();
        }
    }

    /// <summary>
    /// DO NOT use this directly! Instead use OnPressUp()
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isListening)
        {
            if (eventData.dragging)
                return;

            if (eventData.clickCount == 2)
            {
                FireOnButtonDoubleTap();

                OnDoubleTap();
            }

            FireOnButtonPressUp();

            OnPressUp();
        }
    }
}
