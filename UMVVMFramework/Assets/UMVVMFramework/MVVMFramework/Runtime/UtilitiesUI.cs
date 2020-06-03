using UnityEngine;

namespace MVVM
{
    public static partial class UMVCUtilities
    {
        public static Rect ScreenSafeArea()
        {
            return Screen.safeArea;
        }
    }
}
