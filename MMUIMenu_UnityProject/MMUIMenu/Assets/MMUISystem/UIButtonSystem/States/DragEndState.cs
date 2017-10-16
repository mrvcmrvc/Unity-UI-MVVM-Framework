using UnityEngine;

namespace MMUISystem.UIButton
{
    public class DragEndState : StateBase
    {
        public DragEndState()
        {
            StateEnum = InteractionStateEnum.DragEnd;
        }

        public override void UpdateFrame()
        {
            if (CanUpdate)
            {
                if (UIButtonUtilities.GetTotalMillisecondsBetween(Time.realtimeSinceStartup, StateHandleTime) > Time.unscaledDeltaTime)
                    FireOnNewStateRequested(CommandEnum.Idle);
            }
        }

        public override void EnterStateHandler(params object[] addParams)
        {
            StateEnterTime = Time.realtimeSinceStartup;

            FireOnEnterStateHandled();
        }

        public override void ExitStateHandler()
        {
            StateExitTime = Time.realtimeSinceStartup;

            CanUpdate = false;

            FireOnExitStateHandled();
        }

        public override void StateHandler()
        {
            StateHandleTime = Time.realtimeSinceStartup;

            CanUpdate = true;

            FireOnStateHandled();
        }
    }
}
