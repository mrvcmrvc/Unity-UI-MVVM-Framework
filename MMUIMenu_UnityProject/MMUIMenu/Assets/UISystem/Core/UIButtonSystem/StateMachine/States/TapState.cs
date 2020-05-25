using UnityEngine;

namespace MVVM
{
    public class TapState : StateBase
    {
        public TapState()
        {
            StateEnum = InteractionStateEnum.Tap;
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

            FireOnEnterStateHandled();
        }

        public override void ExitStateHandler()
        {
            StateExitTime = Time.realtimeSinceStartup;

            FireOnExitStateHandled();
        }

        public override void StateHandler()
        {
            StateHandleTime = Time.realtimeSinceStartup;

            FireOnStateHandled();
        }
    }
}
