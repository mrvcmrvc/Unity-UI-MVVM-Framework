using System.Collections.Generic;
using UnityEngine;

namespace MVVM
{
    [CreateAssetMenu(fileName = "ButtonSkinUIInfoSO", menuName = "Settings/Input Settings/UI Input/Skin Settings", order = 1)]
    public class ButtonSkinUIInfoSO : ScriptableObject
    {
        public List<ButtonSkinInfo> ButtonSkinInfoList;
    }

    [System.Serializable]
    public class ButtonSkinInfo
    {
        public Sprite ButtonIdleSkin;
        public Sprite ButtonPressedSkin;
    }
}