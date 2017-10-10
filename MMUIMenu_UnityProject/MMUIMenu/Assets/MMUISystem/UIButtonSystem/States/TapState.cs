using System;

namespace MMUISystem.UIButton
{
    public class TapState : StateBase
    {
        public TapState()
        {
            StateEnum = InteractionStateEnum.Tap;
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
