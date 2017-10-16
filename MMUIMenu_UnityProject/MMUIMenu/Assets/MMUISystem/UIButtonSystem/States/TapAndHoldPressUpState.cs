using UnityEngine;

namespace MMUISystem.UIButton
{
    public class TapAndHoldPressUpState : StateBase
    {
        public TapAndHoldPressUpState()
        {
            StateEnum = InteractionStateEnum.TapAndHoldPressUp;

            Conditions.Add(new ElapsedTimeIsHigherThan());
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

            if (CheckTransitions())
            {
                FireOnStateHandled();

                FireOnNewStateRequested(CommandEnum.Idle);
            }
            else
                FireOnNewStateRequested(CommandEnum.DoubleTap);
        }
    }
}
