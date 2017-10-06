namespace MMUISystem.UIButton
{
    public abstract class StateBase
    {
        public InteractionCommandEnum StateEnum;

        public abstract void EnterStateHandler();
        public abstract void StateHandler();
        public abstract void ExitStateHandler();
    }
}
