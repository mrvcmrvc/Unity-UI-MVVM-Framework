using System;
using UnityEngine;

namespace MVVM
{
    public class UIMenuAnimatorBehaviour : UIMenuAnimationBehaviourImplementer
    {
        public Animator Animator;

        private const string ENTRANCE_ANIM = "EntranceAnim";
        private const string EXIT_ANIM = "ExitAnim";

        private string _activeAnimName = "";

        Action _callback;

        public override void PlayIntro(Action callback)
        {
            _activeAnimName = ENTRANCE_ANIM;

            PlayAnim(callback);
        }

        public override void PlayOutro(Action callback)
        {
            _activeAnimName = EXIT_ANIM;

            PlayAnim(callback);
        }

        private void PlayAnim(Action callback)
        {
            Animator.UnregisterOnComplete(this, _callback);

            _callback = callback;

            Animator.Play(_activeAnimName);

            Animator.OnComplete(this, callback);
        }

        public override void FinishSequence(Action callback)
        {
            Animator.UnregisterOnComplete(this, _callback);

            Animator.Play(ENTRANCE_ANIM, 0, 1.0f);

            if (callback != null)
                callback();
        }

        public override void ResetAnim()
        {
            Animator.UnregisterOnComplete(this, _callback);

            Animator.Play(ENTRANCE_ANIM, 0, 0f);
        }
    }
}