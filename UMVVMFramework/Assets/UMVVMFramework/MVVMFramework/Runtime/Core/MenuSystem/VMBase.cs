using System;
using System.Collections;
using UnityEngine;

namespace MVVM
{
    public enum EVMState
    {
        Active,
        Deactive
    }

    public abstract class VMBase : MonoBehaviour
    {
        #region Events
        public static Action<VMBase> OnVMInited;
        public static Action<VMBase> OnVMDestroyed;

        public static Action<VMBase, EVMState> OnVMStateChanged_Static;
        public Action<EVMState> OnVMStateChanged;

        public Action OnPropertyChanged;
        #endregion

        [SerializeField] private float _disablingDelay;
        [SerializeField] private bool _disableVMsUnderneath;

        public bool DisableVMsUnderneath { get { return _disableVMsUnderneath; } }

        private Canvas _canvas;
        private IEnumerator _disableDelayRoutine;

        private EVMState _vmState = EVMState.Deactive;
        public EVMState VMState
        {
            get { return _vmState; }
            set
            { 
                if (_vmState.Equals(value))
                    return;
                
                _vmState = value;

                OnVMStateChanged_Static?.Invoke(this, _vmState);
                OnVMStateChanged?.Invoke(_vmState);
            }
        }

        protected void Awake()
        {
            RegisterActivationEvents();

            Init();

            _canvas.enabled = false;

            AwakeCustomActions();
        }

        protected void OnDestroy()
        {
            UnregisterActivationEvents();

            OnVMDestroyed?.Invoke(this);

            OnDestroyCustomActions();
        }

        protected void Init()
        {
            _canvas = GetComponent<Canvas>();

            InitCustomActions();

            OnVMInited?.Invoke(this);
        }

        protected void NotifyPropertyChanged()
        {
            OnPropertyChanged?.Invoke();
        }

        #region Activation / Deactivation

        /// <summary>
        /// Do not call this manually, instead call ActivateUI()
        /// </summary>
        public void Activate()
        {
            if (_disableDelayRoutine != null)
                StopCoroutine(_disableDelayRoutine);

            _canvas.enabled = true;

            VMState = EVMState.Active;

            OnActivateCustomActions();
        }

        /// <summary>
        /// Do not call this manually, instead call DeactivateUI()
        /// </summary>
        public void Deactivate()
        {
            VMState = EVMState.Deactive;

            if (_disableDelayRoutine != null)
                StopCoroutine(_disableDelayRoutine);

            _disableDelayRoutine = DisableDelayRoutine();
            StartCoroutine(_disableDelayRoutine);
        }

        private IEnumerator DisableDelayRoutine()
        {
            yield return new WaitForSecondsRealtime(_disablingDelay);

            _canvas.enabled = false;

            OnDeactivateCustomActions();
        }

        #endregion

        protected virtual void AwakeCustomActions() { }

        /// <summary>
        /// Called at Awake
        /// </summary>
        protected virtual void InitCustomActions() { }
        protected virtual void OnDestroyCustomActions() { }

        protected virtual void OnActivateCustomActions() { }
        protected virtual void OnDeactivateCustomActions() { }

        protected abstract void RegisterActivationEvents();
        protected abstract void UnregisterActivationEvents();
        protected abstract void ActivateUI();
        protected abstract void DeactivateUI();
    }

}