using UnityEngine;

namespace MMUISystem.UIButton
{
    public class PressDownState : StateBase
    {
        public PressDownState()
        {
            StateEnum = InteractionStateEnum.PressDown;
        }

        public override void UpdateFrame()
        {
            if(CanUpdate)
            {
                if(UIButtonUtilities.GetTotalMillisecondsBetween(Time.realtimeSinceStartup, StateHandleTime) > Time.unscaledDeltaTime)
                    FireOnNewStateRequested(CommandEnum.Press);
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
