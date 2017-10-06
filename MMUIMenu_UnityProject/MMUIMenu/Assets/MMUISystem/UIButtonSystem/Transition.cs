namespace MMUISystem.UIButton
{
    public class Transition
    {
        public InteractionCommandEnum CurState { get; private set; }
        public InteractionCommandEnum RequestedState { get; private set; }
        public InteractionCommandEnum OutcomeState { get; private set; }

        ConditionBase[] _conditions;

        public Transition(InteractionCommandEnum curState, InteractionCommandEnum requestedState, InteractionCommandEnum outcomeState, params ConditionBase[] conditions)
        {
            CurState = curState;
            RequestedState = requestedState;
            OutcomeState = outcomeState;

            _conditions = conditions;
        }

        public bool IsEligibleToTransition()
        {
            bool result = true;

            if (_conditions == null || _conditions.Length == 0)
                return result;

            foreach(var cond in _conditions)
            {
                if(!cond.CheckCondition())
                {
                    result = false;

                    break;
                }
            }

            return result;
        }
    }
}
