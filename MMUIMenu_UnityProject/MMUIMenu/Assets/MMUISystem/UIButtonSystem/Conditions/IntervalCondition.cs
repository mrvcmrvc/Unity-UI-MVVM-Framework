﻿namespace MMUISystem.UIButton
{
    public class ElapsedTimeIsHigherThan : ConditionBase
    {
        float _targetDuration;

        public ElapsedTimeIsHigherThan(float targetDuration)
        {
            _targetDuration = targetDuration;
        }

        public override bool CheckCondition(object[] param)
        {
            float passedTime = (float)param[0];

            return passedTime >= _targetDuration;
        }
    }
}
