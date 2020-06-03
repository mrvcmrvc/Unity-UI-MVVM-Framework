using System;
using System.Collections.Generic;

namespace MVVM
{
    public class UISpriteAnimBehaviour : UIAnimBehaviourImplementer
    {
        public List<UISpriteFrameAnimation> SpriteAnimList;

        private int _remAnimCount;

        private Action _callback;

        public override void PlayIntro(Action callback)
        {
            _remAnimCount = SpriteAnimList.FindAll(s => !s.Loop).Count;

            _callback = callback;

            SpriteAnimList.ForEach(a => a.Play(OnSpriteAnimFinished, true));
        }

        public override void PlayOutro(Action callback)
        {
            _callback = callback;

            _remAnimCount = 1;

            OnSpriteAnimFinished();
        }

        public override void FinishSequence(Action callback)
        {
            _callback = callback;

            SpriteAnimList.FindAll(s => !s.Loop).ForEach(s => s.Finish());

            _remAnimCount = 1;

            OnSpriteAnimFinished();
        }

        public override void ResetAnim()
        {
            SpriteAnimList.ForEach(a => a.Stop());
        }

        protected virtual void OnSpriteAnimFinished()
        {
            _remAnimCount--;

            if (_remAnimCount <= 0)
                CheckForCallback();
        }

        protected virtual void CheckForCallback()
        {
            if (_callback != null)
                _callback();
        }
    }
}