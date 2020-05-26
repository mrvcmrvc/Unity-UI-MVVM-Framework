using System;
using System.Collections;
using UnityEngine;

namespace MVVM
{
    public enum EVMState
    {
        None,
        PreActivation,
        PostActivation,
        PreDeactivation,
        PostDeactivation
    }

    public abstract class VMBase : MonoBehaviour
    {
        #region Events
        public static Action<VMBase> OnUIMenuInited;
        public static Action<VMBase> OnUIMenuDestroyed;

        public static Action<VMBase, EVMState> OnVMStateChanged;

        public Action OnPropertyChanged;
        #endregion

        [SerializeField] private Canvas _canvas;
        [SerializeField] private bool _disableMenusUnderneath;
        [SerializeField] private UIAnimSequence _uiMenuAnimation;

        public bool DisableMenusUnderneath { get { return _disableMenusUnderneath; } }

        private IEnumerator _deactivateRoutine, _activateRoutine;

        private EVMState _vmState;
        public EVMState VMState
        {
            get { return _vmState; }
            set
            { 
                if (_vmState.Equals(value))
                    return;
                
                _vmState = value;
                OnVMStateChanged?.Invoke(this, _vmState);
            }
        }

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
            InitCustomActions();

            OnUIMenuInited?.Invoke(this);
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
            yield return StartCoroutine(PreActivateAdditional());

            VMState = EVMState.PreActivation;

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
            
            VMState = EVMState.PostActivation;
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
            yield return StartCoroutine(PreDeactivateAdditional());

            VMState = EVMState.PreDeactivation;

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

            VMState = EVMState.PostDeactivation;

            _canvas.enabled = false;
        }
        #endregion
    }

}