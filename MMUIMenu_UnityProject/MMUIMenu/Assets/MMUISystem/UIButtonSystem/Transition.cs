namespace MMUISystem.UIButton
{
    public class Transition
    {
        public InteractionStateEnum CurState { get; private set; }
        public CommandEnum Command { get; private set; }
        public InteractionStateEnum OutcomeState { get; private set; }

        public Transition(InteractionStateEnum curState, CommandEnum command, InteractionStateEnum outcomeState)
        {
            CurState = curState;
            Command = command;
            OutcomeState = outcomeState;
        }
    }
}
