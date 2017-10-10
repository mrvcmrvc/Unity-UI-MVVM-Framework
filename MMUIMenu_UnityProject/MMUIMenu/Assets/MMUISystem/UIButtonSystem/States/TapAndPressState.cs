using System;

namespace MMUISystem.UIButton
{
    public class TapAndPressState : StateBase
    {
        public TapAndPressState()
        {
            StateEnum = InteractionStateEnum.TapAndPress;

            Conditions.Add(new ElapsedTimeIsHigherThan(500));
        }

        public override void EnterStateHandler(params object[] addParams)
        {
            StateEnterTime = DateTime.Now;

            DeltaTimeBetweenPrevState = (StateEnterTime - ((DateTime)addParams[0])).Milliseconds;

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
                FireOnNewStateRequested(CommandEnum.PressDown);
            else
                FireOnStateHandled();
        }
    }
}
