using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MMUISystem.UIButton
{
    [AddComponentMenu("UI/Extensions/UIButton - Draggable")]
    public class UnityUIDraggableButton : UnityUIButton, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Events
        public static Action<PointerEventData, UnityUIDraggableButton> OnButtonDragBegin_Static;
        public static Action<PointerEventData, UnityUIDraggableButton> OnButtonDrag_Static;
        public static Action<PointerEventData, UnityUIDraggableButton> OnButtonDragEnd_Static;

        public Action<PointerEventData> OnButtonDragBegin;
        public Action<PointerEventData> OnButtonDrag;
        public Action<PointerEventData> OnButtonDragEnd;

        void FireOnButtonDragBegin(PointerEventData eventData)
        {
            if (OnButtonDragBegin != null)
                OnButtonDragBegin(eventData);

            if (OnButtonDragBegin_Static != null)
                OnButtonDragBegin_Static(eventData, this);
        }

        void FireOnButtonDrag(PointerEventData eventData)
        {
            if (OnButtonDrag != null)
                OnButtonDrag(eventData);

            if (OnButtonDrag_Static != null)
                OnButtonDrag_Static(eventData, this);
        }

        void FireOnButtonDragEnd(PointerEventData eventData)
        {
            if (OnButtonDragEnd != null)
                OnButtonDragEnd(eventData);

            if (OnButtonDragEnd_Static != null)
                OnButtonDragEnd_Static(eventData, this);
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