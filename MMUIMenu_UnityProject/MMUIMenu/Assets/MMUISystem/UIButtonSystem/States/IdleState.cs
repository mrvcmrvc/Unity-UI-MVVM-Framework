using System;

namespace MMUISystem.UIButton
{
    public class IdleState : StateBase
    {
        public IdleState()
        {
            StateEnum = InteractionStateEnum.Idle;
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
