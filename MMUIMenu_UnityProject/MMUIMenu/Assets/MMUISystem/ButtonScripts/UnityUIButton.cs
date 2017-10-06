using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("UI/Extensions/UIButton")]
[RequireComponent(typeof(CanvasRenderer), typeof(Image), typeof(Button)), DisallowMultipleComponent]
public class UnityUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{ 
    public bool StartListeningOnEnable;

    public bool IsDelayedButton;
    public float DelayDurationBeforeFire;
    
    protected bool IsListening;

    private IEnumerator _onPointerDownRoutine;
    
    #region Events
    public Action<PointerEventData> OnButtonPressDown;
    public Action<PointerEventData> OnDelayedButtonPressDown;
    public Action<PointerEventData> OnButtonPressUp;
    public Action<PointerEventData> OnButtonDoubleTap;
    public Action<PointerEventData> OnButtonPressCancel;

    public static Action<PointerEventData> OnButtonPressDelayStarted;
    public static Action<float> OnButtonPressDelayUpdate;
    public static Action<PointerEventData> OnButtonPressDelayFinished;
    public static Action OnButtonPressDelayCanceled;

    void FireOnButtonPressDelayCanceled()
    {
        if (OnButtonPressDelayCanceled != null)
            OnButtonPressDelayCanceled();
    }

    void FireOnButtonPressDelayUpdate(float progress)
    {
        if (OnButtonPressDelayUpdate != null)
            OnButtonPressDelayUpdate(progress);
    }

    void FireOnDelayStarted(PointerEventData eventData)
    {
        if (OnButtonPressDelayStarted != null)
            OnButtonPressDelayStarted(eventData);
    }

    void FireOnDelayFinished(PointerEventData eventData)
    {
        if (OnButtonPressDelayFinished != null)
            OnButtonPressDelayFinished(eventData);
    }

    void FireOnDelayedButtonPressDown(PointerEventData eventData)
    {
        if (OnDelayedButtonPressDown != null)
            OnDelayedButtonPressDown(eventData);
    }

    void FireOnButtonPressDown(PointerEventData eventData)
    {
        if (OnButtonPressDown != null)
            OnButtonPressDown(eventData);
    }

    void FireOnButtonPressUp(PointerEventData eventData)
    {
        if (OnButtonPressUp != null)
            OnButtonPressUp(eventData);
    }

    void FireOnButtonDoubleTap(PointerEventData eventData)
    {
        if (OnButtonDoubleTap != null)
            OnButtonDoubleTap(eventData);
    }

    void FireOnButtonPressCancel(PointerEventData eventData)
    {
        if (OnButtonPressCancel != null)
            OnButtonPressCancel(eventData);
    }
    #endregion

    private void Awake()
    {
        _onPointerDownRoutine = null;
    }

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
        IsListening = true;
    }

    public virtual void StopListening()
    {
        IsListening = false;
    }

    #region Interface Implementation
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (IsListening)
        {
            if (eventData.dragging)
                return;

            if(ResetIfDelayedPress(false))
            {
                _onPointerDownRoutine = DelayedOnPointerDown(eventData);
                StartCoroutine(_onPointerDownRoutine);
            }

            FireOnButtonPressDown(eventData);
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (IsListening)
        {
            ResetIfDelayedPress(true);

            if (eventData.dragging)
            {
                FireOnButtonPressCancel(eventData);

                return;
            }

            if (eventData.clickCount == 2)
                FireOnButtonDoubleTap(eventData);

            FireOnButtonPressUp(eventData);
        }
    }
    #endregion

    protected bool ResetIfDelayedPress(bool fireCancelEvent)
    {
        if (_onPointerDownRoutine != null)
            StopCoroutine(_onPointerDownRoutine);

        _onPointerDownRoutine = null;

        if (IsDelayedButton && fireCancelEvent)
            FireOnButtonPressDelayCanceled();

        return IsDelayedButton;
    }

    private IEnumerator DelayedOnPointerDown(PointerEventData eventData)
    {
        FireOnDelayStarted(eventData);

        float progress = 0f;
        float passedTime = 0f;
        while(progress < 1f)
        {
            yield return new WaitForFixedUpdate();

            passedTime += Time.fixedDeltaTime;

            progress = passedTime / DelayDurationBeforeFire;
            if (progress > 1f)
                progress = 1f;

            FireOnButtonPressDelayUpdate(progress);
        }

        FireOnDelayFinished(eventData);

        FireOnDelayedButtonPressDown(eventData);

        _onPointerDownRoutine = null;
    }
}
