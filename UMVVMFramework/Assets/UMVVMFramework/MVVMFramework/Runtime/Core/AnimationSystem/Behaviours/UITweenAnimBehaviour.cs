using System;
using System.Collections.Generic;
using UITweening;
using UnityEngine;

namespace MVVM
{
    public class UITweenAnimBehaviour : UIAnimBehaviourImplementer
    {
        [Tooltip("Use Intro tweeners as outro tweeners as well")]
        public bool UseIntroTweenersAsOutro;

        public List<UITweener> IntroTweenList;
        public List<UITweener> OutroTweenList;

        private int _activeTweenCount;

        private Action _callback;
        private List<UITweener> _activeTweenList;

        public override void PlayIntro(Action callback)
        {
            _callback = null;

            IntroTweenList.ForEach(t => t.ResetEventDelegates());
            OutroTweenList.ForEach(t => t.ResetEventDelegates());

            OutroTweenList.ForEach(t => t.KillTween());

            _activeTweenCount = IntroTweenList.Count;
            _activeTweenList = IntroTweenList;

            _callback = callback;

            IntroTweenList.ForEach(t => t.InitValueToFROM());
            IntroTweenList.ForEach(t => t.AddOnFinish(OnTweenFinished, false).PlayForward());
        }

        public override void PlayOutro(Action callback)
        {
            _callback = null;

            IntroTweenList.ForEach(t => t.ResetEventDelegates());
            OutroTweenList.ForEach(t => t.ResetEventDelegates());

            _callback = callback;

            if (!UseIntroTweenersAsOutro)
            {
                IntroTweenList.ForEach(t => t.KillTween());

                _activeTweenList = OutroTweenList;

                _activeTweenList.ForEach(t => t.InitValueToFROM());
            }
            else
            {
                _activeTweenList = IntroTweenList;

                _activeTweenList.ForEach(t => t.InitValueToTO());
            }

            _activeTweenCount = _activeTweenList.Count;

            if (_activeTweenList.Count > 0)
                _activeTweenList.ForEach(t => t.AddOnFinish(OnTweenFinished, false).PlayReverse());
            else
                OnTweenFinished();
        }

        private void OnTweenFinished()
        {
            _activeTweenCount--;

            if (_activeTweenCount <= 0)
            {
                if (_callback != null)
                    _callback();

                _callback = null;

                _activeTweenList = null;
            }
        }

        public override void FinishSequence(Action callback)
        {
            _callback = callback;

            _activeTweenList.ForEach(t => t.ResetEventDelegates());
        }

        public override void ResetAnim()
        {
            IntroTweenList.ForEach(t => t.ResetEventDelegates());
            OutroTweenList.ForEach(t => t.ResetEventDelegates());

            OutroTweenList.ForEach(t => t.InitValueToFROM());
            IntroTweenList.ForEach(t => t.InitValueToFROM());
        }
    }
}