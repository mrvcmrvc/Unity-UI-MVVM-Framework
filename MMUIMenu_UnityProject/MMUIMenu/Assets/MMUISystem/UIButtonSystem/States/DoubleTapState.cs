using UnityEngine;

namespace MMUISystem.UIButton
{
    public class DoubleTapState : StateBase
    {
        public DoubleTapState()
        {
            StateEnum = InteractionStateEnum.DoubleTap;
        }

        public override void UpdateFrame()
        {
            if (CanUpdate)
            {
            }
        }

        public override void EnterStateHandler(params object[] addParams)
        {
            StateEnterTime = Time.realtimeSinceStartup;

            DeltaTimeBetweenPrevState = UIButtonUtilities.GetTotalMillisecondsBetween(StateEnterTime, (float)addParams[0]);

            FireOnEnterStateHandled();
        }

        public override void ExitStateHandler()
        {
            StateExitTime = Time.realtimeSinceStartup;

            CanUpdate = false;

            DeltaTimeBetweenPrevState = -1;

            FireOnExitStateHandled();
        }

        public override void StateHandler()
        {
            StateHandleTime = Time.realtimeSinceStartup;

            CanUpdate = true;

            FireOnStateHandled();

            FireOnNewStateRequested(CommandEnum.Idle);
        }
    }
}
