﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MMUISystem.UIButton
{
    [AddComponentMenu("UI/Extensions/UIButton")]
    [RequireComponent(typeof(CanvasRenderer), typeof(Image), typeof(Button)), DisallowMultipleComponent]
    public class UnityUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool StartListeningOnEnable;

        protected bool IsListening;
        protected PointerEventData LastEventData;

        private UIButtonStateMachine StateMachine = new UIButtonStateMachine();

        #region Events
        public Action<PointerEventData> OnButtonPressDown;
        public Action<PointerEventData> OnButtonPressUp;
        public Action<PointerEventData> OnButtonPress;

        public Action<PointerEventData> OnButtonTap;
        public Action<PointerEventData> OnButtonDoubleTap;
        public Action<PointerEventData> OnTapAndHold;

        public Action<PointerEventData> OnButtonPressCancel;
        public Action<PointerEventData> OnDelayedButtonPressDown;

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

        void FireOnButtonTap(PointerEventData eventData)
        {
            if (OnButtonTap != null)
                OnButtonTap(eventData);
        }

        void FireOnButtonDoubleTap(PointerEventData eventData)
        {
            if (OnButtonDoubleTap != null)
                OnButtonDoubleTap(eventData);
        }

        void FireOnButtonPress(PointerEventData eventData)
        {
            if (OnButtonPress != null)
                OnButtonPress(eventData);
        }

        void FireOnTapAndHold(PointerEventData eventData)
        {
            if (OnTapAndHold != null)
                OnTapAndHold(eventData);
        }
        #endregion

        protected virtual void Awake()
        {
            StateMachine.Init();

            StateMachine.OnStateEntered += OnStateEntered;
            StateMachine.OnStateHandled += OnStateHandled;
            StateMachine.OnStateExited += OnStateExited;
        }

        protected virtual void OnDestroy()
        {
            StateMachine.ResetMachine();

            StateMachine.OnStateEntered -= OnStateEntered;
            StateMachine.OnStateHandled -= OnStateHandled;
            StateMachine.OnStateExited -= OnStateExited;
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

            StateMachine.OnDisable();
        }

        private void Update()
        {
            if (StateMachine == null)
                return;

            StateMachine.UpdateFrame();
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

                LastEventData = eventData;

                TriggerStateMachine(CommandEnum.PressDown);
            }
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (IsListening)
            {
                if (eventData.dragging)
                    return;

                LastEventData = eventData;

                TriggerStateMachine(CommandEnum.PressUp);
            }
        }
        #endregion

        #region StateMachine Implementation
        protected virtual void OnStateEntered(InteractionStateEnum state)
        {
        }

        protected virtual void OnStateHandled(InteractionStateEnum state)
        {
            switch(state)
            {
                case InteractionStateEnum.DoubleTap:
                    FireOnButtonDoubleTap(LastEventData);
                    break;
                case InteractionStateEnum.Tap:
                    FireOnButtonTap(LastEventData);
                    break;
                case InteractionStateEnum.Press:
                    FireOnButtonPress(LastEventData);
                    break;
                case InteractionStateEnum.PressDown:
                    FireOnButtonPressDown(LastEventData);
                    break;
                case InteractionStateEnum.TapAndHoldPressUp:
                case InteractionStateEnum.PressUp:
                    FireOnButtonPressUp(LastEventData);
                    break;
                case InteractionStateEnum.TapAndHold:
                    FireOnTapAndHold(LastEventData);
                    break;
            }
        }

        protected virtual void OnStateExited(InteractionStateEnum state)
        {
        }
        #endregion

        protected void TriggerStateMachine(CommandEnum command)
        {
            StateMachine.UpdateState(command);
        }
    }
}
