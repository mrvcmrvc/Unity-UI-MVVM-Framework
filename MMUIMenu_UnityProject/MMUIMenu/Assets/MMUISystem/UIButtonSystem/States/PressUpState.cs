using System;

namespace MMUISystem.UIButton
{
    public class PressUpState : StateBase
    {
        public PressUpState()
        {
            StateEnum = InteractionStateEnum.PressUp;

            Conditions.Add(new ElapsedTimeIsHigherThan());
        }

        public override void EnterStateHandler(params object[] addParams)
        {
            StateEnterTime = DateTime.Now;

            DeltaTimeBetweenPrevState = UIButtonUtilities.GetTotalMillisecondsBetween(StateEnterTime, (DateTime)addParams[0]);

            FireOnEnterStateHandled();
        }

        public override void ExitStateHandler()
        {
            DeltaTimeBetweenPrevState = -1;

            FireOnExitStateHandled();
        }

        public override void StateHandler()
        {
            if (CheckTransitions())
                FireOnNewStateRequested(CommandEnum.Idle);
            else
                FireOnNewStateRequested(CommandEnum.Tap);
        }
    }
}
