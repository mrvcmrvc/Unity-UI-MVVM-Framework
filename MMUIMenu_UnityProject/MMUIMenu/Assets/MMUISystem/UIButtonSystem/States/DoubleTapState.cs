using System;

namespace MMUISystem.UIButton
{
    public class DoubleTapState : StateBase
    {
        public DoubleTapState()
        {
            StateEnum = InteractionStateEnum.DoubleTap;

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
            if (!CheckTransitions())
                FireOnStateHandled();

            FireOnNewStateRequested(CommandEnum.PressUp);
        }
    }
}
