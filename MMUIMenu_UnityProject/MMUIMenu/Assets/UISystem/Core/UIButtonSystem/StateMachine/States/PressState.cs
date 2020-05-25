using UnityEngine;

namespace MVVM
{
    public class PressState : StateBase
    {
        public PressState()
        {
            StateEnum = InteractionStateEnum.Press;
        }

        public override void UpdateFrame()
        {
            if (CanUpdate)
                FireOnStateHandled();
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
        }
    }
}
