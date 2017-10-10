using System;

namespace MMUISystem.UIButton
{
    public class PressState : StateBase
    {
        public PressState()
        {
            StateEnum = InteractionStateEnum.Press;
        }

        public override void EnterStateHandler(params object[] addParams)
        {
            StateEnterTime = DateTime.Now;

            FireOnEnterStateHandled();
        }

        public override void ExitStateHandler()
        {
            FireOnExitStateHandled();
        }

        public override void StateHandler()
        {
            FireOnStateHandled();
        }
    }
}
