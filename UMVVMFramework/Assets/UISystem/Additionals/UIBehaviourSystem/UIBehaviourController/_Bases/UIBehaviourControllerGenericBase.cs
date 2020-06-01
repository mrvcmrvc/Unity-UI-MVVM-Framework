using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIBehaviourControllerBase<T> : UIBehaviourControllerBase where T : UIBehaviourBase<T>
{
    public List<T> BehaviourList { get; private set; }

    public Action<bool> OnBehaviourTweenFinished;
    protected void FireOnBehaviourTweenFinished(bool isActive)
    {
        if (OnBehaviourTweenFinished != null)
            OnBehaviourTweenFinished(isActive);
    }

    protected virtual void Awake()
    {
        BehaviourList = new List<T>(GetComponentsInChildren<T>());

        foreach(var behaviour in BehaviourList)
        {
            behaviour.OnButtonDoubleTapCallback += TriggerSubContainerDoubleTap;
            behaviour.OnButtonDragBeginCallback += TriggerSubContainerDragBegin;
            behaviour.OnButtonDragCallback += TriggerSubContainerDrag;
            behaviour.OnButtonDragEndCallback += TriggerSubContainerDragEnd;
            behaviour.OnButtonPressCancelCallback += TriggerSubContainerPressCancel;
            behaviour.OnButtonPressDownCallback += TriggerSubContainerPressDown;
            behaviour.OnButtonPressUpCallback += TriggerSubContainerPressUp;
            behaviour.OnButtonDelayedPressDownCallback += TriggerSubContainerDelayedPressDown;

            behaviour.OnTweenFinished += FireOnBehaviourTweenFinished;

            behaviour.Init(((RectTransform)transform).sizeDelta);
        }
    }

    protected virtual void OnDestroy()
    {
        foreach (var behaviour in BehaviourList)
        {
            behaviour.OnButtonDoubleTapCallback -= TriggerSubContainerDoubleTap;
            behaviour.OnButtonDragBeginCallback -= TriggerSubContainerDragBegin;
            behaviour.OnButtonDragCallback -= TriggerSubContainerDrag;
            behaviour.OnButtonDragEndCallback -= TriggerSubContainerDragEnd;
            behaviour.OnButtonPressCancelCallback -= TriggerSubContainerPressCancel;
            behaviour.OnButtonPressDownCallback -= TriggerSubContainerPressDown;
            behaviour.OnButtonPressUpCallback -= TriggerSubContainerPressUp;
            behaviour.OnButtonDelayedPressDownCallback -= TriggerSubContainerDelayedPressDown;

            behaviour.OnTweenFinished -= FireOnBehaviourTweenFinished;
        }

        BehaviourList = null;
    }

    #region EventImplementation
    private void TriggerSubContainerDoubleTap(UIBehaviourBase interactedContainer, PointerEventData eventData)
    {
        if (!Interactable)
            return;

        OnSubContainerDoubleTap((T)interactedContainer, eventData);
    }

    private void TriggerSubContainerDragBegin(UIBehaviourBase interactedContainer, PointerEventData eventData)
    {
        if (!Interactable)
            return;

        OnSubContainerDragBegin((T)interactedContainer, eventData);
    }

    private void TriggerSubContainerDrag(UIBehaviourBase interactedContainer, PointerEventData eventData)
    {
        if (!Interactable)
            return;

        OnSubContainerDrag((T)interactedContainer, eventData);
    }

    private void TriggerSubContainerDragEnd(UIBehaviourBase interactedContainer, PointerEventData eventData)
    {
        if (!Interactable)
            return;

        OnSubContainerDragEnd((T)interactedContainer, eventData);
    }

    private void TriggerSubContainerPressCancel(UIBehaviourBase interactedContainer, PointerEventData eventData)
    {
        if (!Interactable)
            return;

        OnSubContainerPressCancel((T)interactedContainer, eventData);
    }

    private void TriggerSubContainerPressDown(UIBehaviourBase interactedContainer, PointerEventData eventData)
    {
        if (!Interactable)
            return;

        OnSubContainerPressDown((T)interactedContainer, eventData);
    }

    private void TriggerSubContainerPressUp(UIBehaviourBase interactedContainer, PointerEventData eventData)
    {
        if (!Interactable)
            return;

        OnSubContainerPressUp((T)interactedContainer, eventData);
    }

    private void TriggerSubContainerDelayedPressDown(UIBehaviourBase interactedContainer, PointerEventData eventData)
    {
        if (!Interactable)
            return;

        OnSubContainerDelayedPressDown((T)interactedContainer, eventData);
    }

    public virtual void OnSubContainerDoubleTap(T interactedContainer, PointerEventData eventData)
    {
    }

    public virtual void OnSubContainerDragBegin(T interactedContainer, PointerEventData eventData)
    {
    }

    public virtual void OnSubContainerDrag(T interactedContainer, PointerEventData eventData)
    {
    }

    public virtual void OnSubContainerDragEnd(T interactedContainer, PointerEventData eventData)
    {
    }

    public virtual void OnSubContainerPressCancel(T interactedContainer, PointerEventData eventData)
    {
    }

    public virtual void OnSubContainerPressDown(T interactedContainer, PointerEventData eventData)
    {
    }

    public virtual void OnSubContainerPressUp(T interactedContainer, PointerEventData eventData)
    {
    }

    public virtual void OnSubContainerDelayedPressDown(T interactedContainer, PointerEventData eventData)
    {
    }
    #endregion
}
