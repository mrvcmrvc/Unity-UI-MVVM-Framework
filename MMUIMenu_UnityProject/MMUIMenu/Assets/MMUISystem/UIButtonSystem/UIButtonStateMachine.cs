using System;
using System.Collections.Generic;
using UnityEngine;

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
            States = new List<StateBase>
            {
                { new TapState() },
                { new IdleState() },
                { new PressState() },
                { new PressDownState() },
                { new PressUpState() },
                { new TapAndHoldState() },
                { new TapAndHoldPressUpState() },
                { new DoubleTapState() },
                { new DragBeginState() },
                { new DragState() },
                { new DragEndState() },
            };

            States.ForEach(s => s.OnEnterStateHandled += FireOnStateEntered);
            States.ForEach(s => s.OnStateHandled += FireOnStateHandled);
            States.ForEach(s => s.OnExitStateHandled += FireOnStateExited);
            States.ForEach(s => s.OnNewStateRequested += OnNewStateRequested);

            StateTransitions = new List<Transition>
            {
                { new Transition(InteractionStateEnum.Idle, CommandEnum.PressDown, InteractionStateEnum.PressDown) },
                { new Transition(InteractionStateEnum.PressDown, CommandEnum.Press, InteractionStateEnum.Press) },
                { new Transition(InteractionStateEnum.Press, CommandEnum.PressUp, InteractionStateEnum.PressUp) },
                { new Transition(InteractionStateEnum.Press, CommandEnum.DragBegin, InteractionStateEnum.DragBegin) },
                { new Transition(InteractionStateEnum.PressUp, CommandEnum.Idle, InteractionStateEnum.Idle) },
                { new Transition(InteractionStateEnum.PressUp, CommandEnum.Tap, InteractionStateEnum.Tap) },
                { new Transition(InteractionStateEnum.Tap, CommandEnum.PressDown, InteractionStateEnum.TapAndHold) },
                { new Transition(InteractionStateEnum.TapAndHold, CommandEnum.PressDown, InteractionStateEnum.PressDown) },
                { new Transition(InteractionStateEnum.TapAndHold, CommandEnum.DragBegin, InteractionStateEnum.DragBegin) },
                { new Transition(InteractionStateEnum.DragBegin, CommandEnum.Drag, InteractionStateEnum.Drag) },
                { new Transition(InteractionStateEnum.Drag, CommandEnum.DragEnd, InteractionStateEnum.DragEnd) },
                { new Transition(InteractionStateEnum.DragEnd, CommandEnum.Idle, InteractionStateEnum.Idle) },
                { new Transition(InteractionStateEnum.TapAndHold, CommandEnum.PressUp, InteractionStateEnum.TapAndHoldPressUp) },
                { new Transition(InteractionStateEnum.TapAndHoldPressUp, CommandEnum.Idle, InteractionStateEnum.Idle) },
                { new Transition(InteractionStateEnum.TapAndHoldPressUp, CommandEnum.DoubleTap, InteractionStateEnum.DoubleTap) },
                { new Transition(InteractionStateEnum.DoubleTap, CommandEnum.Idle, InteractionStateEnum.Idle) },
            };

            ChangeStateTo(InteractionStateEnum.Idle);
        }

        public void ResetMachine()
        {
            if (States == null || States.Count == 0)
                return;

            States.ForEach(s => s.OnEnterStateHandled -= FireOnStateEntered);
            States.ForEach(s => s.OnStateHandled -= FireOnStateHandled);
            States.ForEach(s => s.OnExitStateHandled -= FireOnStateExited);
            States.ForEach(s => s.OnNewStateRequested -= OnNewStateRequested);
        }

        private void OnNewStateRequested(CommandEnum command)
        {
            UpdateState(command);
        }

        public void UpdateState(CommandEnum command)
        {
            if(CurState == null)
            {
                Debug.LogWarning("UIButton State Machine is called early!");
                return;
            }

            Transition eligibleTransition = FindTransition(CurState.StateEnum, command);
            if (eligibleTransition == null)
            {
                Debug.LogWarning("No Transition found for: " + CurState.StateEnum + " --> " + command);
                return;
            }
            Debug.Log("FROM: " + eligibleTransition.CurState + " TO: " + eligibleTransition.OutcomeState);
            ChangeStateTo(eligibleTransition.OutcomeState);
        }

        private void ChangeStateTo(InteractionStateEnum state)
        {
            float stateEnterTime = CurState == null ? Time.realtimeSinceStartup : CurState.StateEnterTime;

            if (CurState != null)
                CurState.ExitStateHandler();

            CurState = FindState(state);
            Debug.Log(CurState.StateEnum);
            if (CurState == null)
            {
                Debug.LogWarning("CurState is null");
                return;
            }

            CurState.EnterStateHandler(stateEnterTime);

            CurState.StateHandler();
        }

        public void UpdateFrame()
        {
            if (CurState == null)
                return;

            CurState.UpdateFrame();
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
