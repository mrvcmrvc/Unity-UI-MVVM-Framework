using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MVVM
{
    public class JoystickMoveInputController : MoveInputControllerBase
    {
        private Joystick _joystick;
        private bool _isJoystickPressed;

        public JoystickMoveInputController(bool isActive, bool ignoreZeroDir)
            : base(isActive, ignoreZeroDir)
        {
            _joystick = Object.FindObjectOfType<Joystick>();

            UnityUIButton button = _joystick.GetComponent<UnityUIButton>();

            button.OnButtonPressedDown += OnJoystickPressDown;
            button.OnTappedAndHeld += OnJoystickPressDown;
            button.OnButtonPressedUp += OnJoystickPressUp;
            button.OnButtonTapped += OnJoystickPressUp;
        }

        protected override void DisposeCustomActions()
        {
            UnityUIButton button = _joystick.GetComponent<UnityUIButton>();

            button.OnButtonPressedDown -= OnJoystickPressDown;
            button.OnTappedAndHeld -= OnJoystickPressDown;
            button.OnButtonPressedUp -= OnJoystickPressUp;
            button.OnButtonTapped -= OnJoystickPressUp;
        }

        private void OnJoystickPressDown(PointerEventData eventData)
        {
            _isJoystickPressed = true;
        }

        private void OnJoystickPressUp(PointerEventData eventData)
        {
            _isJoystickPressed = false;
        }

        protected override Vector2 CheckDirection()
        {
            return _joystick.Direction;
        }

        protected override bool IsAnyInput()
        {
            return _isJoystickPressed;
        }

        protected override bool AdditionalInputEligibleCheck()
        {
            return true;
        }
    }
}