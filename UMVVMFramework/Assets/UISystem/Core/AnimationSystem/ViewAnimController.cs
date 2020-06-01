using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVVM
{
    public class ViewAnimController : MonoBehaviour
    {
        public enum EViewAnimState
        {
            None,
            PreIntro,
            PostIntro,
            PreOutro,
            PostOutro
        }

        [SerializeField] private UIAnimSequenceTriggerBase _trigger;
        [SerializeField] private List<UIMenuAnimationBehaviourImplementer> _animColl;

        public Action<EViewAnimState> OnAnimStateChanged;

        public EViewAnimState CurState { get; private set; }

        private void Awake()
        {
            ResetSequence();

            _trigger.OnIntroTriggered += OnIntroTriggered;
            _trigger.OnOutroTriggered += OnOutroTriggered;
        }

        private void OnDestroy()
        {
            _trigger.OnIntroTriggered -= OnIntroTriggered;
            _trigger.OnOutroTriggered -= OnOutroTriggered;
        }

        private void OnIntroTriggered()
        {
            ResetSequence();

            PlayIntroSequence();
        }

        private void OnOutroTriggered()
        {
            PlayOutroSequence();
        }

        private void PlayIntroSequence()
        {
            CurState = EViewAnimState.PreIntro;
            OnAnimStateChanged?.Invoke(EViewAnimState.PreIntro);

            _animColl.ForEach(s => s.PlayIntro(OnIntroSequenceFinished));
        }

        private void PlayOutroSequence()
        {
            CurState = EViewAnimState.PreOutro;
            OnAnimStateChanged?.Invoke(EViewAnimState.PreOutro);

            _animColl.ForEach(s => s.PlayOutro(OnOutroSequenceFinished));
        }

        private void OnIntroSequenceFinished()
        {
            if (_animColl.FindAll(s => s.IsPlaying).Count == 0)
            {
                CurState = EViewAnimState.PostIntro;
                OnAnimStateChanged?.Invoke(EViewAnimState.PostIntro);
            }
        }

        private void OnOutroSequenceFinished()
        {
            if (_animColl.FindAll(s => s.IsPlaying).Count == 0)
            {
                CurState = EViewAnimState.PostOutro;
                OnAnimStateChanged?.Invoke(EViewAnimState.PostOutro);
            }
        }

        private void ResetSequence()
        {
            _animColl.ForEach(s => s.ResetAnim());

            CurState = EViewAnimState.None;
            OnAnimStateChanged?.Invoke(EViewAnimState.None);
        }
    }
}