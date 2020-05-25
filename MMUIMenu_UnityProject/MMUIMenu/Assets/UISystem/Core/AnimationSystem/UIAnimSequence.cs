using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVVM
{
    public class UIAnimSequence : MonoBehaviour
    {
        public List<UIMenuAnimationBehaviourImplementer> AnimList;

        public Action Callback { get; private set; }

        protected virtual void Awake()
        {
            ResetSequence();
        }

        public void PlayIntroSequence(Action callback)
        {
            Callback = callback;

            AnimList.ForEach(s => s.PlayIntro(OnSequenceFinished));
        }

        public void PlayOutroSequence(Action callback)
        {
            Callback = callback;

            AnimList.ForEach(s => s.PlayOutro(OnSequenceFinished));
        }

        protected virtual void OnSequenceFinished()
        {
            if (AnimList.FindAll(s => s.IsPlaying).Count == 0)
                CheckForCallback();
        }

        protected virtual void CheckForCallback()
        {
            if (Callback != null)
                Callback();
        }

        public virtual void ResetSequence()
        {
            AnimList.ForEach(s => s.ResetAnim());
        }

        public virtual void FinishSequence(bool canCallback)
        {
            if (canCallback)
                AnimList.ForEach(s => s.FinishSequence(Callback));
            else
                AnimList.ForEach(s => s.FinishSequence(null));
        }
    }
}