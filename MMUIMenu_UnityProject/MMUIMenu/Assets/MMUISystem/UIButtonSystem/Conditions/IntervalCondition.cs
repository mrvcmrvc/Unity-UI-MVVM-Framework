namespace MMUISystem.UIButton
{
    public class IntervalCondition : ConditionBase
    {
        float _intervalDuration;

        public IntervalCondition(float intervalDuration)
        {
            _intervalDuration = intervalDuration;
        }

        public override bool CheckCondition()
        {
            return true;
        }
    }
}

