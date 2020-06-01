using System;
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
        public static Action<VMBase> OnUIMenuInited;
        public static Action<VMBase> OnUIMenuDestroyed;

        public static Action<VMBase, EVMState> OnVMStateChanged_Static;
        public Action<EVMState> OnVMStateChanged;

        public Action OnPropertyChanged;
        #endregion

        [SerializeField] private Canvas _canvas;
        [SerializeField] private bool _disableMenusUnderneath;

        public bool DisableMenusUnderneath { get { return _disableMenusUnderneath; } }

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

            OnUIMenuDestroyed?.Invoke(this);

            OnDestroyCustomActions();
        }

        protected void Init()
        {
            InitCustomActions();

            OnUIMenuInited?.Invoke(this);
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
            _canvas.enabled = true;

            VMState = EVMState.Active;

            OnActivateCustomActions();
        }

        /// <summary>
        /// Do not call this manually, instead call DeactivateUI()
        /// </summary>
        public void Deactivate()
        {
            _canvas.enabled = false;

            VMState = EVMState.Deactive;

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