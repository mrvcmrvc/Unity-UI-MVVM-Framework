using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MVVM
{
    [RequireComponent(typeof(Image))]
    public class UISpriteAnimation : MonoBehaviour
    {
        int _frameIndex = -1;

        [Range(0, 960)]
        public float Framerate = 60;
        public bool IgnoreTimeScale = true;
        public bool Loop = true;
        public float LoopInterval = 0;
        public float Delay = 0;
        public bool StartImmediately = true;
        public List<UISpriteAnimation> ChainSpriteAnimList;
        public List<Sprite> Frames;

        private bool _isStopped = true;
        public bool IsStopped
        {
            get
            {
                return _isStopped;
            }
        }

        private bool _isPlaying = false;
        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }
        }

        private bool _isPaused = true;
        public bool IsPaused
        {
            get
            {
                return _isPaused;
            }
        }

        private bool _isChildAnim = false;
        public bool IsChildAnim
        {
            get
            {
                return _isChildAnim;
            }
        }

        private Image _image;
        public Image Image
        {
            get
            {
                if (_image == null)
                    _image = GetComponent<Image>();

                return _image;
            }
        }

        private Action _callback;

        float _lastUpdateTime = 0f, _remLoopIntervalTime = 0f, _remDelayTime = 0f;

        private void Awake()
        {
            ChainSpriteAnimList.ForEach(c => c.SetAsChild());
        }

        private void OnEnable()
        {
            if (StartImmediately)
                Play(null);
        }

        private void OnDisable()
        {
            if (!_isStopped)
                Stop(false);
        }

        public void SetAsChild()
        {
            _isChildAnim = true;
        }

        public void Play(Action callback, bool fromBeginning = false)
        {
            _callback = callback;

            if (fromBeginning)
                Stop();

            _remLoopIntervalTime = LoopInterval;
            _remDelayTime = Delay;

            _isStopped = false;
            _isPaused = false;
            _isPlaying = true;
        }

        public void Pause()
        {
            _isStopped = false;
            _isPaused = true;
            _isPlaying = false;
        }

        public void Stop(bool setToFirstFrame = true)
        {
            if (_isStopped)
                return;

            Pause();

            _isStopped = true;

            _remLoopIntervalTime = LoopInterval;
            _remDelayTime = Delay;

            if (setToFirstFrame)
            {
                _frameIndex = 0;
                UpdateSprite();
            }

            ChainSpriteAnimList.ForEach(c => c.Stop(false));
        }

        public void Finish()
        {
            if (Loop)
            {
                if (!_isPlaying)
                    Play(null, true);

                return;
            }

            Pause();

            _remLoopIntervalTime = 0f;
            _remDelayTime = Delay;

            _frameIndex = Frames.Count - 1;

            if (ChainSpriteAnimList.Count == 0)
                UpdateSprite();
            else
                ChainSpriteAnimList.ForEach(c => c.Finish());
        }

        void Update()
        {
            if (IsPaused)
                return;

            if (Frames == null || Frames.Count == 0 || Framerate == 0)
                Stop(false);

            if (_remDelayTime > 0)
            {
                _remDelayTime -= IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;

                return;
            }

            float time = IgnoreTimeScale ? Time.unscaledTime : Time.time;
            if (_lastUpdateTime < time)
            {
                _lastUpdateTime = time;
                int newIndex = _frameIndex + 1;

                if (!Loop && newIndex >= Frames.Count)
                {
                    Stop(false);

                    if (_callback != null)
                        _callback();

                    StartChain();

                    return;
                }
                else if (Loop && newIndex >= Frames.Count)
                {
                    if (_remLoopIntervalTime > 0)
                    {
                        _remLoopIntervalTime -= IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;

                        return;
                    }
                }

                _remLoopIntervalTime = LoopInterval;

                _frameIndex = RepeatIndex(newIndex, Frames.Count);
                UpdateSprite();
            }
        }

        int RepeatIndex(int curIndex, int length)
        {
            if (curIndex >= length)
                return 0;

            return curIndex;
        }

        void UpdateSprite()
        {
            float time = IgnoreTimeScale ? Time.unscaledTime : Time.time;

            if (Framerate != 0)
                _lastUpdateTime = time + Mathf.Abs(1f / Framerate);

            Image.sprite = Frames[_frameIndex];
        }

        void StartChain()
        {
            ChainSpriteAnimList.ForEach(c => c.Play(null, true));
        }
    }
}