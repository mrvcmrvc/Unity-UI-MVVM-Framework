using UnityEngine;

namespace MMUISystem.UIButton
{
    public class DragState : StateBase
    {
        public DragState()
        {
            StateEnum = InteractionStateEnum.Drag;
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
