using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVVM
{
    public class UIAnimController : MonoBehaviour
    {
        public enum EUIAnimState
        {
            None,
            PreIntro,
            PostIntro,
            PreOutro,
            PostOutro
        }

        #region Events
        public Action OnPreIntro;
        public Action OnPostIntro;
        public Action OnPreOutro;
        public Action OnPostOutro; 
        #endregion

        [SerializeField] private UIAnimTriggerBase _trigger;
        [SerializeField] private List<UIAnimBehaviourImplementer> _animBehavColl;

        public EUIAnimState CurState { get; private set; }

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
            if (CurState.Equals(EUIAnimState.PreIntro)
                || CurState.Equals(EUIAnimState.PostIntro))
                return;

            ResetSequence();

            PlayIntroSequence();
        }

        private void OnOutroTriggered()
        {
            if (CurState.Equals(EUIAnimState.PreOutro)
                || CurState.Equals(EUIAnimState.PostOutro))
                return;

            PlayOutroSequence();
        }

        public void TriggerIntroManually()
        {
            OnIntroTriggered();
        }

        public void TriggerOutroManually()
        {
            OnOutroTriggered();
        }

        private void PlayIntroSequence()
        {
            CurState = EUIAnimState.PreIntro;

            OnPreIntro?.Invoke();

            _animBehavColl.ForEach(s => s.PlayIntro(OnIntroSequenceFinished));
        }

        private void PlayOutroSequence()
        {
            CurState = EUIAnimState.PreOutro;

            OnPreOutro?.Invoke();

            _animBehavColl.ForEach(s => s.PlayOutro(OnOutroSequenceFinished));
        }

        private void OnIntroSequenceFinished()
        {
            if (_animBehavColl.FindAll(s => s.IsPlaying).Count == 0)
            {
                CurState = EUIAnimState.PostIntro;

                OnPostIntro?.Invoke();
            }
        }

        private void OnOutroSequenceFinished()
        {
            if (_animBehavColl.FindAll(s => s.IsPlaying).Count == 0)
            {
                CurState = EUIAnimState.PostOutro;

                OnPostOutro?.Invoke();
            }
        }

        private void ResetSequence()
        {
            _animBehavColl.ForEach(s => s.ResetAnim());

            CurState = EUIAnimState.None;
        }
    }
}