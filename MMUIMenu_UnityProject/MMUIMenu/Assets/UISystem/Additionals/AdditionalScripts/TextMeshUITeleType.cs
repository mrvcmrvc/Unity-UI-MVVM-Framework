using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace MVVM
{
    public enum ETeleType
    {
        Once,
        Loop,
        PingPong,
        OncePingPong
    }

    public class TextMeshUITeleType : MonoBehaviour
    {
        public ETeleType AnimType;
        public float WaitBtwCharacters;
        public float DelayBeforeRestart;
        public bool IgnoreTimescale;

        private TextMeshProUGUI _textMesh;
        private TextMeshProUGUI TextMesh
        {
            get
            {
                if (_textMesh == null)
                    _textMesh = GetComponent<TextMeshProUGUI>();

                return _textMesh;
            }
        }
        private int _totalVisibleCharacters;
        private IEnumerator _teleTypeRoutine;
        private IEnumerator _forwardTeleTypeRoutine;
        private IEnumerator _reverseTeleTypeRoutine;

        public bool IsFinished { get; private set; }
        public bool IsPaused { get; private set; }
        public bool IsInit { get; private set; }

        #region Events
        public Action OnTeleTypeFinished;
        private void FireOnTeleTypeFinished()
        {
            if (OnTeleTypeFinished != null)
                OnTeleTypeFinished();
        }

        public Action OnTeleTypeStarted;
        private void FireOnTeleTypeStarted()
        {
            if (OnTeleTypeStarted != null)
                OnTeleTypeStarted();
        }
        #endregion

        private void Awake()
        {
            _teleTypeRoutine = null;
            _forwardTeleTypeRoutine = null;
            _reverseTeleTypeRoutine = null;

            IsFinished = false;
            IsPaused = false;
            IsInit = false;

            _textMesh = GetComponent<TextMeshProUGUI>();
        }

        public void ResetTeleType()
        {
            KillCoroutine();

            IsFinished = false;
            IsPaused = false;

            TextMesh.maxVisibleCharacters = 0;
        }

        public void StartTeleType()
        {
            if (_teleTypeRoutine != null)
                KillCoroutine();

            IsPaused = false;
            IsInit = true;

            TextMesh.ForceMeshUpdate(true);

            _teleTypeRoutine = TeleTypeRoutine();
            StartCoroutine(_teleTypeRoutine);
        }

        public void RestartTeleType()
        {
            if (!IsInit)
                return;

            if (_teleTypeRoutine != null)
                KillCoroutine();

            _teleTypeRoutine = TeleTypeRoutine();
            StartCoroutine(_teleTypeRoutine);
        }

        public void KillTeleType()
        {
            if (IsFinished)
                return;

            if (_teleTypeRoutine != null)
                KillCoroutine();

            IsFinished = true;
            IsPaused = false;

            TextMesh.maxVisibleCharacters = _totalVisibleCharacters;

            FireOnTeleTypeFinished();
        }

        public void PauseTeleType()
        {
            if (IsPaused)
                return;

            IsPaused = true;

            StopCoroutine(_teleTypeRoutine);
            StopCoroutine(_forwardTeleTypeRoutine);
            StopCoroutine(_reverseTeleTypeRoutine);

        }

        public void ContinueTeleType()
        {
            if (!IsPaused)
                return;

            IsPaused = false;

            StartCoroutine(_teleTypeRoutine);
            StartCoroutine(_forwardTeleTypeRoutine);
            StartCoroutine(_reverseTeleTypeRoutine);
        }

        private void KillCoroutine()
        {
            if (_teleTypeRoutine != null)
                StopCoroutine(_teleTypeRoutine);

            if (_forwardTeleTypeRoutine != null)
                StopCoroutine(_forwardTeleTypeRoutine);

            if (_reverseTeleTypeRoutine != null)
                StopCoroutine(_reverseTeleTypeRoutine);

            _teleTypeRoutine = null;
            _forwardTeleTypeRoutine = null;
            _reverseTeleTypeRoutine = null;
        }

        private IEnumerator TeleTypeRoutine()
        {
            _totalVisibleCharacters = TextMesh.textInfo.characterCount;

            _forwardTeleTypeRoutine = ForwardTeleTypeRoutine();

            switch (AnimType)
            {
                case ETeleType.Once:
                    yield return StartCoroutine(_forwardTeleTypeRoutine);
                    break;
                case ETeleType.Loop:
                    yield return StartCoroutine(_forwardTeleTypeRoutine);

                    if (IgnoreTimescale)
                        yield return new WaitForSecondsRealtime(DelayBeforeRestart);
                    else
                        yield return new WaitForSeconds(DelayBeforeRestart);

                    RestartTeleType();
                    break;
                case ETeleType.OncePingPong:
                    yield return StartCoroutine(_forwardTeleTypeRoutine);

                    _reverseTeleTypeRoutine = ReverseTeleTypeRoutine();
                    yield return StartCoroutine(_reverseTeleTypeRoutine);
                    break;
                case ETeleType.PingPong:
                    yield return StartCoroutine(_forwardTeleTypeRoutine);

                    _reverseTeleTypeRoutine = ReverseTeleTypeRoutine();
                    yield return StartCoroutine(_reverseTeleTypeRoutine);

                    if (IgnoreTimescale)
                        yield return new WaitForSecondsRealtime(DelayBeforeRestart);
                    else
                        yield return new WaitForSeconds(DelayBeforeRestart);

                    RestartTeleType();
                    break;
            }
        }

        private IEnumerator ForwardTeleTypeRoutine()
        {
            int counter = 1;
            int curTotalVisibleCount = 0;
            int curPageCharIndex = TextMesh.textInfo.pageInfo[TextMesh.pageToDisplay - 1].lastCharacterIndex;

            IsFinished = false;

            FireOnTeleTypeStarted();

            while (curTotalVisibleCount < _totalVisibleCharacters)
            {
                curTotalVisibleCount = counter % (_totalVisibleCharacters + 1);
                if (TextMesh.pageToDisplay > 1)
                    curTotalVisibleCount += TextMesh.textInfo.pageInfo[TextMesh.pageToDisplay - 2].lastCharacterIndex;

                TextMesh.maxVisibleCharacters = curTotalVisibleCount;

                if (curPageCharIndex == curTotalVisibleCount)
                    break;

                counter++;

                if (IgnoreTimescale)
                    yield return new WaitForSecondsRealtime(WaitBtwCharacters);
                else
                    yield return new WaitForSeconds(WaitBtwCharacters);
            }

            TextMesh.maxVisibleCharacters = _totalVisibleCharacters;

            IsFinished = true;
            IsPaused = false;

            FireOnTeleTypeFinished();
        }

        private IEnumerator ReverseTeleTypeRoutine()
        {
            int visibleCount = _totalVisibleCharacters;

            IsFinished = false;

            FireOnTeleTypeStarted();

            while (visibleCount > 0)
            {
                TextMesh.maxVisibleCharacters = visibleCount;

                visibleCount--;

                if (IgnoreTimescale)
                    yield return new WaitForSecondsRealtime(WaitBtwCharacters);
                else
                    yield return new WaitForSeconds(WaitBtwCharacters);
            }

            TextMesh.maxVisibleCharacters = 0;

            IsFinished = true;
            IsPaused = false;

            FireOnTeleTypeFinished();
        }
    }
}