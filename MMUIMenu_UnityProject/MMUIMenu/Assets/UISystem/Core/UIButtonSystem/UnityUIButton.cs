using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MVVM
{
    [AddComponentMenu("UI/Extensions/UIButton"), RequireComponent(typeof(CanvasRenderer))]
    public class UnityUIButton : Button, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public bool StartListeningOnEnable;

        protected bool IsListening;
        public bool IsDeactivated { get; private set; } = false;
        protected PointerEventData LastEventData;

        private UIButtonStateMachine StateMachine = new UIButtonStateMachine();
        private bool _isExitedWhilePressed = false;

        #region Events
        public static Action<PointerEventData, UnityUIButton> OnButtonPressedDown_Static;
        public static Action<PointerEventData, UnityUIButton> OnButtonPressedUp_Static;
        public static Action<PointerEventData, UnityUIButton> OnInputLeftButton_Static;
        public static Action<PointerEventData, UnityUIButton> OnButtonPressed_Static;
        public static Action<PointerEventData, UnityUIButton> OnButtonTapped_Static;
        public static Action<PointerEventData, UnityUIButton> OnButtonDoubleTapped_Static;
        public static Action<PointerEventData, UnityUIButton> OnTappedAndHeld_Static;
        public static Action<PointerEventData, UnityUIButton> OnDeactivePressedDown_Static;
        public static Action<PointerEventData, UnityUIButton> OnDeactivePressedUp_Static;

        public Action<PointerEventData> OnButtonPressedDown;
        public Action<PointerEventData> OnButtonPressedUp;
        public Action<PointerEventData> OnInputLeftButton;
        public Action<PointerEventData> OnButtonPressed;
        public Action<PointerEventData> OnButtonTapped;
        public Action<PointerEventData> OnButtonDoubleTapped;
        public Action<PointerEventData> OnTappedAndHeld;
        public Action<PointerEventData> OnDeactivePressedDown;
        public Action<PointerEventData> OnDeactivePressedUp;

        void FireOnButtonPressDown(PointerEventData eventData)
        {
            OnButtonPressedDown?.Invoke(eventData);

            OnButtonPressedDown_Static?.Invoke(eventData, this);
        }

        void FireOnInputLeftButton(PointerEventData eventData)
        {
            OnInputLeftButton?.Invoke(eventData);

            OnInputLeftButton_Static?.Invoke(eventData, this);
        }

        void FireOnButtonPressUp(PointerEventData eventData)
        {
            OnButtonPressedUp?.Invoke(eventData);

            OnButtonPressedUp_Static?.Invoke(eventData, this);
        }

        void FireOnButtonTap(PointerEventData eventData)
        {
            OnButtonTapped?.Invoke(eventData);

            OnButtonTapped_Static?.Invoke(eventData, this);
        }

        void FireOnButtonDoubleTap(PointerEventData eventData)
        {
            OnButtonDoubleTapped?.Invoke(eventData);

            OnButtonDoubleTapped_Static?.Invoke(eventData, this);
        }

        void FireOnButtonPress(PointerEventData eventData)
        {
            OnButtonPressed?.Invoke(eventData);

            OnButtonPressed_Static?.Invoke(eventData, this);
        }

        void FireOnTapAndHold(PointerEventData eventData)
        {
            OnTappedAndHeld?.Invoke(eventData);

            OnTappedAndHeld_Static?.Invoke(eventData, this);
        }

        void FireOnDeactivePressDown(PointerEventData eventData)
        {
            OnDeactivePressedDown?.Invoke(eventData);

            OnDeactivePressedDown_Static?.Invoke(eventData, this);
        }

        void FireOnDeactivePressUp(PointerEventData eventData)
        {
            OnDeactivePressedUp?.Invoke(eventData);

            OnDeactivePressedUp_Static?.Invoke(eventData, this);
        }
        #endregion

        #region SubGraphicDataForSpriteSwap

        [SerializeField]
        private Graphic SubTargetGraphic;
        [SerializeField]
        private SpriteState SubGraphicData;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (transition != Transition.SpriteSwap)
                return;

            Sprite transitionSprite;
            switch (state)
            {
                case SelectionState.Normal:
                    transitionSprite = null;
                    break;
                case SelectionState.Highlighted:
                    transitionSprite = SubGraphicData.highlightedSprite;
                    break;
                case SelectionState.Pressed:
                    transitionSprite = SubGraphicData.pressedSprite;
                    break;
                case SelectionState.Disabled:
                    transitionSprite = SubGraphicData.disabledSprite;
                    break;
                default:
                    transitionSprite = null;
                    break;
            }

            if (gameObject.activeInHierarchy)
                DoSpriteSwap(transitionSprite);
        }

        void DoSpriteSwap(Sprite newSprite)
        {
            if (SubTargetGraphic == null)
                return;

            ((Image)SubTargetGraphic).overrideSprite = newSprite;
        }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            IsDeactivated = !interactable;

            StateMachine.Init();

            StateMachine.OnStateEntered += OnStateEntered;
            StateMachine.OnStateHandled += OnStateHandled;
            StateMachine.OnStateExited += OnStateExited;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            StateMachine.ResetMachine();

            StateMachine.OnStateEntered -= OnStateEntered;
            StateMachine.OnStateHandled -= OnStateHandled;
            StateMachine.OnStateExited -= OnStateExited;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (StartListeningOnEnable)
                StartListening();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (!Application.isPlaying)
                return;

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

        public virtual void ToggleButton(bool isActive)
        {
            IsDeactivated = !isActive;

            interactable = isActive;
        }

        public virtual void HideButton()
        {
            IsDeactivated = true;

            gameObject.SetActive(false);
        }

        public virtual void ShowButton()
        {
            IsDeactivated = false;

            gameObject.SetActive(true);
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
            if (IsListening && !IsDeactivated)
            {
                LastEventData = eventData;

                TriggerStateMachine(CommandEnum.PressDown);
            }
            else if (IsDeactivated)
                FireOnDeactivePressDown(eventData);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (IsListening && !IsDeactivated)
            {
                LastEventData = eventData;

                TriggerStateMachine(CommandEnum.PressUp);
            }
            else if (IsDeactivated)
                FireOnDeactivePressUp(eventData);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (IsListening && !IsDeactivated)
                _isExitedWhilePressed = true;
        }
        #endregion

        #region StateMachine Implementation
        protected virtual void OnStateEntered(InteractionStateEnum state)
        {
        }

        protected virtual void OnStateHandled(InteractionStateEnum state)
        {
            switch (state)
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
                    if (_isExitedWhilePressed)
                    {
                        _isExitedWhilePressed = false;

                        FireOnInputLeftButton(LastEventData);

                        break;
                    }

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
