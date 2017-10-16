using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MMUISystem.UIButton
{
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
            if (IsListening)
            {
                LastEventData = eventData;

                TriggerStateMachine(CommandEnum.DragBegin);
            }
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (IsListening)
            {
                LastEventData = eventData;

                TriggerStateMachine(CommandEnum.Drag);
            }
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (IsListening)
            {
                LastEventData = eventData;

                TriggerStateMachine(CommandEnum.DragEnd);
            }
        }

        protected override void OnStateHandled(InteractionStateEnum state)
        {
            switch (state)
            {
                case InteractionStateEnum.DragBegin:
                    FireOnButtonDragBegin(LastEventData);
                    break;
                case InteractionStateEnum.Drag:
                    FireOnButtonDrag(LastEventData);
                    break;
                case InteractionStateEnum.DragEnd:
                    FireOnButtonDragEnd(LastEventData);
                    break;
            }

            base.OnStateHandled(state);
        }
        #endregion
    }
}