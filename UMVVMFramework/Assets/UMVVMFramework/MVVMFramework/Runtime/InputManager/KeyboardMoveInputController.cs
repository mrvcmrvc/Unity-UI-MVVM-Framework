using UnityEngine;

namespace MVVM
{
    public class KeyboardMoveInputController : MoveInputControllerBase
    {
        public KeyboardMoveInputController(bool isActive)
            : base(isActive, false)
        {
        }

        protected override Vector2 CheckDirection()
        {
            Vector2 result = Vector2.zero;

            result.x = Input.GetAxis("Horizontal");
            result.y = Input.GetAxis("Vertical");

            return result;
        }

        protected override bool IsAnyInput()
        {
            return Input.anyKey;
        }

        protected override bool AdditionalInputEligibleCheck()
        {
            return true;
        }
    }
}