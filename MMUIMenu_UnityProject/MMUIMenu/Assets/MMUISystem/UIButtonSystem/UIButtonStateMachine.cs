using System;
using System.Collections.Generic;

namespace MMUISystem.UIButton
{
    public class UIButtonStateMachine
    {
        public List<Transition> StateTransitions { get; private set; }
        public List<StateBase> States { get; private set; }

        StateBase _curState;

        #region Events
        public Action<StateBase> OnStateEntered;
        public Action<StateBase> OnStateHandled;
        public Action<StateBase> OnStateExited;

        private void FireOnStateEntered(StateBase curState)
        {
            if (OnStateEntered != null)
                OnStateEntered(curState);
        }

        private void FireOnStateHandled(StateBase curState)
        {
            if (OnStateHandled != null)
                OnStateHandled(curState);
        }

        private void FireOnStateExited(StateBase curState)
        {
            if (OnStateExited != null)
                OnStateExited(curState);
        }
        #endregion

        public void Init()
        {
            States = new List<StateBase>
            {
                { new PressDownState() },
                { new PressUpState() },
            };

            StateTransitions = new List<Transition>
            {
                //{ new Transition(InteractionCommandEnum.PressDown, InteractionCommandEnum.DragBegin), InteractionCommandEnum.DragBegin },
                //{ new Transition(InteractionCommandEnum.PressDown, InteractionCommandEnum.Press, InteractionCommandEnum.Press) },
                { new Transition(InteractionCommandEnum.PressDown, InteractionCommandEnum.PressUp, InteractionCommandEnum.PressUp) },

                //{ new Transition(InteractionCommandEnum.PressUp, InteractionCommandEnum.PressDown, "cond"), InteractionCommandEnum.DelayedPress }, //IsDelayedButton
                { new Transition(InteractionCommandEnum.PressUp, InteractionCommandEnum.PressDown, InteractionCommandEnum.PressDown) }, //!IsDelayedButton

                //{ new Transition(InteractionCommandEnum.Press, InteractionCommandEnum.DragBegin), InteractionCommandEnum.DragBegin },
                //{ new Transition(InteractionCommandEnum.Press, InteractionCommandEnum.PressUp, "cond"), InteractionCommandEnum.PressUp }, //ElapsedTime is higher than given
                //{ new Transition(InteractionCommandEnum.Press, InteractionCommandEnum.PressUp, "cond"), InteractionCommandEnum.Tap }, //ElapsedTime is lower than given

                //{ new Transition(InteractionCommandEnum.Tap, InteractionCommandEnum.PressDown), InteractionCommandEnum.TapAndPress  },

                //{ new Transition(InteractionCommandEnum.TapAndPress, InteractionCommandEnum.DragBegin), InteractionCommandEnum.DragBegin },
                //{ new Transition(InteractionCommandEnum.TapAndPress, InteractionCommandEnum.PressUp, "cond"), InteractionCommandEnum.PressUp }, //ElapsedTime is higher than given
                //{ new Transition(InteractionCommandEnum.TapAndPress, InteractionCommandEnum.PressUp, "cond"), InteractionCommandEnum.DoubleTap }, //ElapsedTime is lower than given

                //{ new Transition(InteractionCommandEnum.DelayedPress, InteractionCommandEnum.Press), InteractionCommandEnum.Press },
                //{ new Transition(InteractionCommandEnum.DelayedPress, InteractionCommandEnum.PressUp), InteractionCommandEnum.PressUp },

                //{ new Transition(InteractionCommandEnum.DragBegin, InteractionCommandEnum.Drag), InteractionCommandEnum.Drag },

                //{ new Transition(InteractionCommandEnum.Drag, InteractionCommandEnum.DragEnd), InteractionCommandEnum.DragEnd },
                //{ new Transition(InteractionCommandEnum.Drag, InteractionCommandEnum.PressUp), InteractionCommandEnum.PressUp },

                //{ new Transition(InteractionCommandEnum.DragEnd, InteractionCommandEnum.Press), InteractionCommandEnum.Press },
                //{ new Transition(InteractionCommandEnum.DragEnd, InteractionCommandEnum.PressUp), InteractionCommandEnum.PressUp },
            };

            ResetState();
        }

        private void ResetState()
        {
            if (_curState != null)
            {
                _curState.ExitStateHandler();
                FireOnStateExited(_curState);
            }

            _curState = FindState(InteractionCommandEnum.PressUp);

            _curState.EnterStateHandler();
            FireOnStateEntered(_curState);

            _curState.StateHandler();
            FireOnStateHandled(_curState);
        }

        public void UpdateState(InteractionCommandEnum requestedState)
        {
            List<Transition> candidateTransitions = FindAllTransitions(requestedState);
            Transition eligibleTransition = FilterCandidateTransByCond(candidateTransitions);
            UnityEngine.Debug.Log(eligibleTransition.CurState + " " + eligibleTransition.RequestedState + " -> " + eligibleTransition.OutcomeState);
            if (eligibleTransition == null)
                return;

            if (_curState != null)
            {
                _curState.ExitStateHandler();
                FireOnStateExited(_curState);
            }

            _curState = FindState(eligibleTransition.OutcomeState);

            _curState.EnterStateHandler();
            FireOnStateEntered(_curState);

            _curState.StateHandler();
            FireOnStateHandled(_curState);
        }

        private StateBase FindState(InteractionCommandEnum curState)
        {
            return States.Find(s => s.StateEnum == curState);
        }

        private List<Transition> FindAllTransitions(InteractionCommandEnum requestedState)
        {
            if (_curState == null)
                return null;

            return StateTransitions.FindAll(tr => tr.CurState == _curState.StateEnum && tr.RequestedState == requestedState);
        }

        private Transition FilterCandidateTransByCond(List<Transition> candidateTransitions)
        {
            if (candidateTransitions == null || candidateTransitions.Count == 0)
                return null;

            return candidateTransitions.Find(tr => tr.IsEligibleToTransition());
        }
    }
}
