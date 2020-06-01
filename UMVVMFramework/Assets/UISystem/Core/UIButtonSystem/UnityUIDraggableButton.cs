using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MVVM
{
    [Serializable]
    public class DragRestrictionData
    {
        public float MinDegree;
        public float MaxDegree;
    }

    [AddComponentMenu("UI/Extensions/UIButton - Draggable"), DisallowMultipleComponent]
    public class UnityUIDraggableButton : UnityUIButton, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Events
        public static Action<PointerEventData, UnityUIDraggableButton> OnButtonDraggedBegin_Static;
        public static Action<PointerEventData, UnityUIDraggableButton> OnButtonDragged_Static;
        public static Action<PointerEventData, UnityUIDraggableButton> OnButtonDraggedEnd_Static;

        public Action<PointerEventData> OnButtonDraggedBegin;
        public Action<PointerEventData> OnButtonDragged;
        public Action<PointerEventData> OnButtonDraggedEnd;

        void FireOnButtonDragBegin(PointerEventData eventData)
        {
            if (OnButtonDraggedBegin != null)
                OnButtonDraggedBegin(eventData);

            if (OnButtonDraggedBegin_Static != null)
                OnButtonDraggedBegin_Static(eventData, this);
        }

        void FireOnButtonDrag(PointerEventData eventData)
        {
            if (OnButtonDragged != null)
                OnButtonDragged(eventData);

            if (OnButtonDragged_Static != null)
                OnButtonDragged_Static(eventData, this);
        }

        void FireOnButtonDragEnd(PointerEventData eventData)
        {
            if (OnButtonDraggedEnd != null)
                OnButtonDraggedEnd(eventData);

            if (OnButtonDraggedEnd_Static != null)
                OnButtonDraggedEnd_Static(eventData, this);
        }
        #endregion

        public List<DragRestrictionData> RestrictionList = new List<DragRestrictionData>();

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (IsListening
                && CheckIfInsideRestriction(eventData))
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

        private bool CheckIfInsideRestriction(PointerEventData eventData)
        {
            if (RestrictionList.Count == 0)
                return true;

            Vector3 targetDragDir = eventData.delta.normalized;
            Vector3 neighbourVector = new Vector3(1f, 0f);

            float angle = Vector3.Angle(neighbourVector, targetDragDir);
            if (targetDragDir.y < 0)
                angle = 360 - angle;

            foreach (DragRestrictionData restriction in RestrictionList)
            {
                if (angle >= restriction.MinDegree && angle <= restriction.MaxDegree)
                    return false;
            }

            return true;
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
    }
}