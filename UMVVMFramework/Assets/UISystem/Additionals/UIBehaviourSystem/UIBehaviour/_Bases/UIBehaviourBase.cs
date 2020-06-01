using MVVM;
using System;
using System.Collections.Generic;
using UITweening;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIBehaviourBase : MonoBehaviour
{
    public CanvasGroup ContentCanvasGroup;
    public UnityUIButton InteractionButton;
    public UnityUIDraggableButton DraggableInteractionButton;

    public bool IsActive { get; protected set; }
    public RectTransform RectTransform { get; protected set; }

    protected Vector2 InitialSize;
    protected List<UIVirtualValueTweener> TweenList;

    #region Events
    public Action<UIBehaviourBase, PointerEventData> OnButtonDoubleTapCallback;
    public void FireOnButtonDoubleTap(PointerEventData eventData)
    {
        if (OnButtonDoubleTapCallback != null)
            OnButtonDoubleTapCallback(this, eventData);
    }

    public Action<UIBehaviourBase, PointerEventData> OnButtonDragCallback;
    public void FireOnButtonDrag(PointerEventData eventData)
    {
        if (OnButtonDragCallback != null)
            OnButtonDragCallback(this, eventData);
    }

    public Action<UIBehaviourBase, PointerEventData> OnButtonDragBeginCallback;
    public void FireOnButtonDragBegin(PointerEventData eventData)
    {
        if (OnButtonDragBeginCallback != null)
            OnButtonDragBeginCallback(this, eventData);
    }

    public Action<UIBehaviourBase, PointerEventData> OnButtonDragEndCallback;
    public void FireOnButtonDragEnd(PointerEventData eventData)
    {
        if (OnButtonDragEndCallback != null)
            OnButtonDragEndCallback(this, eventData);
    }

    public Action<UIBehaviourBase, PointerEventData> OnButtonPressCancelCallback;
    public void FireOnButtonPressCancel(PointerEventData eventData)
    {
        if (OnButtonPressCancelCallback != null)
            OnButtonPressCancelCallback(this, eventData);
    }

    public Action<UIBehaviourBase, PointerEventData> OnButtonPressDownCallback;
    public void FireOnButtonPressDown(PointerEventData eventData)
    {
        if (OnButtonPressDownCallback != null)
            OnButtonPressDownCallback(this, eventData);
    }

    public Action<UIBehaviourBase, PointerEventData> OnButtonPressUpCallback;
    public void FireOnButtonPressUp(PointerEventData eventData)
    {
        if (OnButtonPressUpCallback != null)
            OnButtonPressUpCallback(this, eventData);
    }

    public Action<UIBehaviourBase, PointerEventData> OnButtonDelayedPressDownCallback;
    public void FireOnButtonDelayedPressDown(PointerEventData eventData)
    {
        if (OnButtonDelayedPressDownCallback != null)
            OnButtonDelayedPressDownCallback(this, eventData);
    }
    #endregion

    protected virtual void Awake()
    {
        if(DraggableInteractionButton != null)
        {
            DraggableInteractionButton.OnButtonDoubleTapped += OnButtonDoubleTap;
            DraggableInteractionButton.OnButtonDragged += OnButtonDrag;
            DraggableInteractionButton.OnButtonDraggedBegin += OnButtonDragBegin;
            DraggableInteractionButton.OnButtonDraggedEnd += OnButtonDragEnd;
            DraggableInteractionButton.OnButtonPressedDown += OnButtonPressDown;
            DraggableInteractionButton.OnButtonPressedUp += OnButtonPressUp;
        }

        if(InteractionButton != null)
        {
            InteractionButton.OnButtonDoubleTapped += OnButtonDoubleTap;
            InteractionButton.OnButtonPressedDown += OnButtonPressDown;
            InteractionButton.OnButtonPressedUp += OnButtonPressUp;
        }
    }

    protected virtual void OnDestroy()
    {
        if (DraggableInteractionButton != null)
        {
            DraggableInteractionButton.OnButtonDoubleTapped -= OnButtonDoubleTap;
            DraggableInteractionButton.OnButtonDragged -= OnButtonDrag;
            DraggableInteractionButton.OnButtonDraggedBegin -= OnButtonDragBegin;
            DraggableInteractionButton.OnButtonDraggedEnd -= OnButtonDragEnd;
            DraggableInteractionButton.OnButtonPressedDown -= OnButtonPressDown;
            DraggableInteractionButton.OnButtonPressedUp -= OnButtonPressUp;
        }

        if (InteractionButton != null)
        {
            InteractionButton.OnButtonDoubleTapped -= OnButtonDoubleTap;
            InteractionButton.OnButtonPressedDown -= OnButtonPressDown;
            InteractionButton.OnButtonPressedUp -= OnButtonPressUp;
        }
    }

    public virtual void Init(Vector2 controllerRectSize)
    {
        RectTransform = GetComponent<RectTransform>();

        InitialSize = controllerRectSize;
    }

    #region ButtonEvents
    protected virtual void OnButtonDoubleTap(PointerEventData eventData)
    {
        FireOnButtonDoubleTap(eventData);
    }

    protected virtual void OnButtonDrag(PointerEventData eventData)
    {
        FireOnButtonDrag(eventData);
    }

    protected virtual void OnButtonDragBegin(PointerEventData eventData)
    {
        FireOnButtonDragBegin(eventData);
    }

    protected virtual void OnButtonDragEnd(PointerEventData eventData)
    {
        FireOnButtonDragEnd(eventData);
    }

    protected virtual void OnButtonPressCancel(PointerEventData eventData)
    {
        FireOnButtonPressCancel(eventData);
    }

    protected virtual void OnButtonPressDown(PointerEventData eventData)
    {
        FireOnButtonPressDown(eventData);
    }

    protected virtual void OnButtonPressUp(PointerEventData eventData)
    {
        FireOnButtonPressUp(eventData);
    }

    protected virtual void OnButtonDelayedPressDown(PointerEventData eventData)
    {
        FireOnButtonDelayedPressDown(eventData);
    }
    #endregion

    public abstract void Activate(params object[] parameters);
    public abstract void Deactivate(params object[] parameters);
    public abstract void ResetUI(bool isActivate, params object[] parameters);

    protected abstract void StartTransition(bool isActivate, params object[] parameters);
    protected abstract void ShowContent();
    protected abstract void HideContent();
}
