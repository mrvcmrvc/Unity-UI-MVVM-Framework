using System;

namespace MMUISystem.UIButton
{
    public class PressState : StateBase
    {
        public DateTime PressEnterTime;

        public PressState()
        {
            StateEnum = InteractionCommandEnum.Press;
        }

        public override void EnterStateHandler()
        {
            PressEnterTime = DateTime.Now;
        }

        public override void ExitStateHandler()
        {
        }

        public override void StateHandler()
        {
        }
    }
}
