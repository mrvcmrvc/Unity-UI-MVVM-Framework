using System;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("UI/Extensions/UIButton - Draggable")]
public class UnityUIDraggableButton : UnityUIButton, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Events
    public Action<PointerEventData> OnButtonDragBegin;
    public Action<PointerEventData> OnButtonDrag;
    public Action<PointerEventData> OnButtonDragEnd;

    void FireOnButtonDragBegin(PointerEventData eventData)
    {
        if (OnButtonDragBegin != null)
            OnButtonDragBegin(eventData);
    }

    void FireOnButtonDrag(PointerEventData eventData)
    {
        if (OnButtonDrag != null)
            OnButtonDrag(eventData);
    }

    void FireOnButtonDragEnd(PointerEventData eventData)
    {
        if (OnButtonDragEnd != null)
            OnButtonDragEnd(eventData);
    }
    #endregion

    #region Interface Implementation
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        ResetIfDelayedPress(true);

        if (IsListening)
            FireOnButtonDragBegin(eventData);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        ResetIfDelayedPress(true);

        if (IsListening)
            FireOnButtonDrag(eventData);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        ResetIfDelayedPress(true);

        if (IsListening)
            FireOnButtonDragEnd(eventData);
    }
    #endregion
}
