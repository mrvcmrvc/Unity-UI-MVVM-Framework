namespace MMUISystem.UIButton
{
    public class ElapsedTimeIsHigherThan : ConditionBase
    {
        int _targetMilliSeconds;

        public ElapsedTimeIsHigherThan(int targetMilliSeconds)
        {
            _targetMilliSeconds = targetMilliSeconds;
        }

        public override bool CheckCondition(params object[] param)
        {
            int passedTime = (int)param[0];
            UnityEngine.Debug.Log(passedTime);
            return passedTime >= _targetMilliSeconds;
        }
    }
}

