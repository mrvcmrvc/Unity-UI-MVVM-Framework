using System;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MVVM
{
    public class UITimelineAnimBehaviour : UIAnimBehaviourImplementer
    {
        public PlayableDirector Director;
        public TimelineAsset IntroTimeline;
        public TimelineAsset OutroTimeline;

        Action _callback;

        public override void PlayIntro(Action callback)
        {
            Director.stopped -= OnPlayableStopped;
            Director.Stop();

            _callback = callback;

            Director.stopped += OnPlayableStopped;

            Director.Play(IntroTimeline);
        }

        public override void PlayOutro(Action callback)
        {
            Director.stopped -= OnPlayableStopped;
            Director.Stop();

            _callback = callback;

            Director.stopped += OnPlayableStopped;

            Director.Play(OutroTimeline);
        }

        public override void FinishSequence(Action callback)
        {
            Director.time = Director.duration;

            Director.Evaluate();

            if (_callback != null)
                _callback();
        }

        private void OnPlayableStopped(PlayableDirector stoppedDirector)
        {
            if (_callback != null)
                _callback();
        }

        public override void ResetAnim()
        {
            Director.Play(IntroTimeline);
            Director.Stop();

            Director.time = 0;

            Director.Evaluate();
        }
    }
}