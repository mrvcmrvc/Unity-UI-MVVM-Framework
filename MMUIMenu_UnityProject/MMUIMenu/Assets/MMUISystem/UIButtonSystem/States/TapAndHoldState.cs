using UnityEngine;

namespace MMUISystem.UIButton
{
    public class TapAndHoldState : StateBase
    {
        public TapAndHoldState()
        {
            StateEnum = InteractionStateEnum.TapAndHold;

            Conditions.Add(new ElapsedTimeIsHigherThan());
        }

        public override void UpdateFrame()
        {
            if (CanUpdate)
                FireOnStateHandled();
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

            if (CheckTransitions())
                FireOnNewStateRequested(CommandEnum.PressDown);
            else
                CanUpdate = true;
        }
    }
}
