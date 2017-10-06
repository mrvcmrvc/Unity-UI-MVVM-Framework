namespace MMUISystem.UIButton
{
    public class ElapsedTimeIsLowerThan : ConditionBase
    {
        float _targetDuration;

        public ElapsedTimeIsLowerThan(float targetDuration)
        {
            _targetDuration = targetDuration;
        }

        public override bool CheckCondition(object[] param)
        {
            float passedTime = (float)param[0];

            return passedTime <= _targetDuration;
        }
    }
}
