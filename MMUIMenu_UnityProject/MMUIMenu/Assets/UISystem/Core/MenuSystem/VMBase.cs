using System;
using System.Collections;
using UnityEngine;

namespace MVVM
{
    public abstract class VMBase : MonoBehaviour
    {
        #region Events
        public static Action<VMBase> OnUIMenuInitCompleted;
        public static Action<VMBase> OnUIMenuDestroyed;

        public Action OnPropertyChanged;
        #endregion

        [SerializeField] private Canvas _canvas;
        [SerializeField] private bool _disableMenusUnderneath;
        [SerializeField] private UIAnimSequence _uiMenuAnimation;

        public bool DisableMenusUnderneath { get { return _disableMenusUnderneath; } }
        public bool IsPreActivationFinished { get; protected set; }
        public bool IsPostActivationFinished { get; protected set; }
        public bool IsPreDeactivationFinished { get; protected set; }
        public bool IsPostDeactivationFinished { get; protected set; }

        public static Action<VMBase> OnPreActivation, OnPostActivation, OnPreDeactivation, OnPostDeactivation;

        private IEnumerator _deactivateRoutine, _activateRoutine;

        protected void Awake()
        {
            RegisterActivationEvents(ActivateUI);

            Init();

            _canvas.enabled = false;

            AwakeCustomActions();
        }

        protected void OnDestroy()
        {
            UnregisterActivationEvents(ActivateUI);

            OnUIMenuDestroyed?.Invoke(this);

            OnDestroyCustomActions();
        }

        protected void Init()
        {
            IsPreDeactivationFinished = true;
            IsPostDeactivationFinished = true;

            IsPreActivationFinished = false;
            IsPostActivationFinished = false;

            InitCustomActions();

            OnUIMenuInitCompleted?.Invoke(this);
        }

        protected virtual IEnumerator PreDeactivateAdditional()
        {
            yield return new WaitForEndOfFrame();
        }

        protected virtual IEnumerator PostDeactivateAdditional()
        {
            yield return new WaitForEndOfFrame();
        }

        protected virtual IEnumerator PreActivateAdditional()
        {
            yield return new WaitForEndOfFrame();
        }

        protected virtual IEnumerator PostActivateAdditional()
        {
            yield return new WaitForEndOfFrame();
        }

        protected void NotifyPropertyChanged()
        {
            OnPropertyChanged?.Invoke();
        }

        protected virtual void AwakeCustomActions() { }

        /// <summary>
        /// Called at Awake
        /// </summary>
        protected virtual void InitCustomActions() { }
        protected virtual void OnDestroyCustomActions() { }

        /// <summary>
        /// Register the callback, given as parameter, to events.
        /// When callback is invoked, system will automatically open UI if it is not active already.
        /// </summary>
        /// <param name="callback"></param>
        protected abstract void RegisterActivationEvents(Action callback);

        /// <summary>
        /// Unregister the callback, given as parameter, from events,
        /// that are listened in "RegisterActivationEvents" method.
        /// </summary>
        /// <param name="callback"></param>
        protected abstract void UnregisterActivationEvents(Action callback);
        protected abstract void ActivateUI();

        #region Activation / Deactivation
        public virtual void Activate()
        {
            _canvas.enabled = true;

            if (_deactivateRoutine != null)
                StopCoroutine(_deactivateRoutine);

            if (_activateRoutine != null)
                StopCoroutine(_activateRoutine);

            _activateRoutine = ActivateRoutine();
            StartCoroutine(_activateRoutine);
        }

        private IEnumerator ActivateRoutine()
        {
            IsPreDeactivationFinished = false;
            IsPostDeactivationFinished = false;

            yield return StartCoroutine(PreActivateAdditional());
            IsPreActivationFinished = true;

            OnPreActivation?.Invoke(this);

            if (_uiMenuAnimation != null)
            {
                _uiMenuAnimation.ResetSequence();

                _uiMenuAnimation.PlayIntroSequence(OnMenuIntroAnimFinished);
            }
            else
                OnMenuIntroAnimFinished();
        }

        private void OnMenuIntroAnimFinished()
        {
            StartCoroutine(PostActivateAdditional());

            IsPostActivationFinished = true;

            OnPostActivation?.Invoke(this);
        }

        public virtual void Deactivate()
        {
            if (_activateRoutine != null)
                StopCoroutine(_activateRoutine);

            if (_deactivateRoutine != null)
                StopCoroutine(_deactivateRoutine);

            _deactivateRoutine = DeactivateRoutine();
            StartCoroutine(_deactivateRoutine);
        }

        private IEnumerator DeactivateRoutine()
        {
            IsPreActivationFinished = false;
            IsPostActivationFinished = false;

            yield return StartCoroutine(PreDeactivateAdditional());
            IsPreDeactivationFinished = true;

            OnPreDeactivation?.Invoke(this);

            if (_uiMenuAnimation != null)
                _uiMenuAnimation.PlayOutroSequence(OnMenuOutroAnimFinished);
            else
                OnMenuOutroAnimFinished();
        }

        private void OnMenuOutroAnimFinished()
        {
            if (_uiMenuAnimation != null)
                _uiMenuAnimation.ResetSequence();

            StartCoroutine(PostDeactivateAdditional());
            IsPostDeactivationFinished = true;

            _canvas.enabled = false;

            OnPostDeactivation?.Invoke(this);
        }
        #endregion
    }

}