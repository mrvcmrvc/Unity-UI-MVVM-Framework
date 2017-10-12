using System;
using System.Collections.Generic;

namespace MMUISystem.UIButton
{
    public class UIButtonStateMachine
    {
        public List<Transition> StateTransitions { get; private set; }
        public List<StateBase> States { get; private set; }
        public StateBase CurState { get; private set; }

        #region Events
        public Action<InteractionStateEnum> OnStateEntered;
        public Action<InteractionStateEnum> OnStateHandled;
        public Action<InteractionStateEnum> OnStateExited;

        private void FireOnStateEntered(InteractionStateEnum state)
        {
            if (OnStateEntered != null)
                OnStateEntered(state);
        }

        private void FireOnStateHandled(InteractionStateEnum state)
        {
            if (OnStateHandled != null)
                OnStateHandled(state);
        }

        private void FireOnStateExited(InteractionStateEnum state)
        {
            if (OnStateExited != null)
                OnStateExited(state);
        }
        #endregion

        public void Init()
        {
            StateBase.OnEnterStateHandled += FireOnStateEntered;
            StateBase.OnStateHandled += FireOnStateHandled;
            StateBase.OnExitStateHandled += FireOnStateExited;

            StateBase.OnNewStateRequested += OnNewStateRequested;

            States = new List<StateBase>
            {
                { new TapState() },
                { new TapAndPressState() },
                { new DoubleTapState() },
                { new IdleState() },
                { new PressState() },
                { new PressDownState() },
                { new PressUpState() },
            };

            StateTransitions = new List<Transition>
            {
                { new Transition(InteractionStateEnum.Tap, CommandEnum.PressDown, InteractionStateEnum.TapAndPress) },
                { new Transition(InteractionStateEnum.TapAndPress, CommandEnum.PressDown, InteractionStateEnum.PressDown) },
                { new Transition(InteractionStateEnum.TapAndPress, CommandEnum.PressUp, InteractionStateEnum.DoubleTap) },
                { new Transition(InteractionStateEnum.DoubleTap, CommandEnum.PressUp, InteractionStateEnum.Idle) },
                { new Transition(InteractionStateEnum.Idle, CommandEnum.PressDown, InteractionStateEnum.PressDown) },
                { new Transition(InteractionStateEnum.PressDown, CommandEnum.PressDown, InteractionStateEnum.Press) },
                { new Transition(InteractionStateEnum.Press, CommandEnum.PressUp, InteractionStateEnum.PressUp) },
                { new Transition(InteractionStateEnum.PressUp, CommandEnum.Tap, InteractionStateEnum.Tap) },
                { new Transition(InteractionStateEnum.PressUp, CommandEnum.Idle, InteractionStateEnum.Idle) },
            };

            ChangeStateTo(InteractionStateEnum.Idle);
        }

        public void ResetMachine()
        {
            StateBase.OnEnterStateHandled -= FireOnStateEntered;
            StateBase.OnStateHandled -= FireOnStateHandled;
            StateBase.OnExitStateHandled -= FireOnStateExited;

            StateBase.OnNewStateRequested -= OnNewStateRequested;
        }

        private void OnNewStateRequested(CommandEnum command)
        {
            UpdateState(command);
        }

        public void UpdateState(CommandEnum command)
        {
            if(CurState == null)
            {
                UnityEngine.Debug.LogError("UIButton State Machine is called early!");
                return;
            }

            Transition eligibleTransition = FindTransition(CurState.StateEnum, command);
            if (eligibleTransition == null)
                return;

            UnityEngine.Debug.Log(eligibleTransition.CurState + " --> " + eligibleTransition.OutcomeState);

            ChangeStateTo(eligibleTransition.OutcomeState);
        }

        private void ChangeStateTo(InteractionStateEnum state)
        {
            DateTime stateEnterTime = CurState == null ? DateTime.Now : CurState.StateEnterTime;

            if (CurState != null)
                CurState.ExitStateHandler();

            CurState = FindState(state);

            if (CurState == null)
            {
                UnityEngine.Debug.LogError("CurState is null");
                return;
            }

            CurState.EnterStateHandler(stateEnterTime);

            CurState.StateHandler();
        }

        private StateBase FindState(InteractionStateEnum state)
        {
            return States.Find(s => s.StateEnum == state);
        }

        private Transition FindTransition(InteractionStateEnum state, CommandEnum command)
        {
            return StateTransitions.Find(tr => tr.CurState == state && tr.Command == command);
        }
    }
}
