﻿using System;
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
        public Action<UIAnimController> OnPreIntro;
        public Action<UIAnimController> OnPostIntro;
        public Action<UIAnimController> OnPreOutro;
        public Action<UIAnimController> OnPostOutro; 
        #endregion

        [SerializeField] private UIAnimTriggerBase _trigger;
        [SerializeField] private List<UIAnimBehaviourImplementer> _animBehavColl;

        public EUIAnimState CurState { get; private set; }

        private void Awake()
        {
            ResetSequence();

            if (_trigger == null)
                return;

            _trigger.OnIntroTriggered += OnIntroTriggered;
            _trigger.OnOutroTriggered += OnOutroTriggered;
        }

        private void OnDestroy()
        {
            if (_trigger == null)
                return;

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

            OnPreIntro?.Invoke(this);

            _animBehavColl.ForEach(s => s.PlayIntro(OnIntroSequenceFinished));
        }

        private void PlayOutroSequence()
        {
            CurState = EUIAnimState.PreOutro;

            OnPreOutro?.Invoke(this);

            _animBehavColl.ForEach(s => s.PlayOutro(OnOutroSequenceFinished));
        }

        private void OnIntroSequenceFinished()
        {
            if (_animBehavColl.FindAll(s => s.IsPlaying).Count == 0)
            {
                CurState = EUIAnimState.PostIntro;

                OnPostIntro?.Invoke(this);
            }
        }

        private void OnOutroSequenceFinished()
        {
            if (_animBehavColl.FindAll(s => s.IsPlaying).Count == 0)
            {
                CurState = EUIAnimState.PostOutro;

                OnPostOutro?.Invoke(this);
            }
        }

        private void ResetSequence()
        {
            _animBehavColl.ForEach(s => s.ResetAnim());

            CurState = EUIAnimState.None;
        }
    }
}