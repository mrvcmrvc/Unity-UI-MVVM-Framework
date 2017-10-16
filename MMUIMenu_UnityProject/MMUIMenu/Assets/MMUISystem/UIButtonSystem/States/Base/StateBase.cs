using System;
using System.Collections.Generic;

namespace MMUISystem.UIButton
{
    public abstract class StateBase
    {
        #region Events
        public Action<CommandEnum> OnNewStateRequested;
        protected void FireOnNewStateRequested(CommandEnum command)
        {
            if (OnNewStateRequested != null)
                OnNewStateRequested(command);
        }

        public Action<InteractionStateEnum> OnEnterStateHandled;
        protected void FireOnEnterStateHandled()
        {
            if (OnEnterStateHandled != null)
                OnEnterStateHandled(StateEnum);
        }

        public Action<InteractionStateEnum> OnStateHandled;
        protected void FireOnStateHandled()
        {
            if (OnStateHandled != null)
                OnStateHandled(StateEnum);
        }

        public Action<InteractionStateEnum> OnExitStateHandled;
        protected void FireOnExitStateHandled()
        {
            if (OnExitStateHandled != null)
                OnExitStateHandled(StateEnum);
        }
        #endregion

        public InteractionStateEnum StateEnum;
        public float StateEnterTime, StateHandleTime, StateExitTime;
        public int DeltaTimeBetweenPrevState;

        public List<ConditionBase> Conditions = new List<ConditionBase>();

        protected bool CanUpdate;

        public abstract void EnterStateHandler(params object[] addParams);
        public abstract void StateHandler();
        public abstract void ExitStateHandler();
        public abstract void UpdateFrame();

        protected virtual bool CheckTransitions()
        {
            bool result = true;

            if (Conditions == null || Conditions.Count == 0)
                return result;

            foreach (var cond in Conditions)
            {
                if (!cond.CheckCondition(DeltaTimeBetweenPrevState))
                {
                    result = false;

                    break;
                }
            }

            return result;
        }
    }
}
