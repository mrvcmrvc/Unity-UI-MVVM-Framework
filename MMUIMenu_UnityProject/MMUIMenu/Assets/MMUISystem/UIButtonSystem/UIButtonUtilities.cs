using System;

namespace MMUISystem.UIButton
{
    public static class UIButtonUtilities
    {
        public static int SensivityInMilliseconds = 200;

        public static int GetTotalMillisecondsBetween(DateTime from, DateTime to)
        {
            TimeSpan timePassed = from - to;

            return (int)timePassed.TotalMilliseconds;
        }
    }
}
