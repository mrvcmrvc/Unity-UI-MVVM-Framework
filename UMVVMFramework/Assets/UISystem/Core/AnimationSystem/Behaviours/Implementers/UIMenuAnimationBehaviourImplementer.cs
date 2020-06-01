using System;
using UnityEngine;

namespace MVVM
{
    public abstract class UIMenuAnimationBehaviourImplementer : MonoBehaviour, IUIMenuAnimationBehaviour
    {
        public bool IsFinished { get; set; }
        public bool IsPlaying { get; set; }

        public abstract void PlayIntro(Action callback);
        public abstract void PlayOutro(Action callback);
        public abstract void FinishSequence(Action callback);

        public abstract void ResetAnim();
    }
}