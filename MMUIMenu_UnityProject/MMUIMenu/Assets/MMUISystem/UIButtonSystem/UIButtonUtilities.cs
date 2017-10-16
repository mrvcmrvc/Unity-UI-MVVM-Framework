namespace MMUISystem.UIButton
{
    public static class UIButtonUtilities
    {
        public static int SensivityInMilliseconds = 200;

        public static int GetTotalMillisecondsBetween(float from, float to)
        {
            int fromInMillisec = (int)(from * 1000);
            int toInMillisec = (int)(to * 1000);


            return fromInMillisec - toInMillisec;
        }
    }
}
