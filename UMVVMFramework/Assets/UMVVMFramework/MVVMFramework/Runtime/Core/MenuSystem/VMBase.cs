using System;
using System.Collections.Generic;
using UnityEngine;

namespace MVVM
{
    public enum EVMState
    {
        Active,
        Deactive
    }

    public class VMDeactivationController
    {
        private List<UIAnimController> _outroListeninAnimControllerColl = new List<UIAnimController>();
        private Action _callback;

        public void StopDeactivationListening()
        {
            foreach(UIAnimController animController in _outroListeninAnimControllerColl)
                RemoveAnimController(animController);
        }

        public void ListenOutroAnimsTriggeredByVM(Action<EVMState> targetEvent, Action callback)
        {
            _callback = callback;

            _outroListeninAnimControllerColl = new List<UIAnimController>();

            List<UIAnimTriggerBase> vmListeningAnimTriggerColl = GetTriggersFromVMEvent(targetEvent);

            List<UIAnimController> candidateOutroListeninAnimControllerColl = GetAnimationControllersFromTriggerOutro(vmListeningAnimTriggerColl);

            foreach(UIAnimController animController in candidateOutroListeninAnimControllerColl)
            {
                if (animController.CurState.Equals(UIAnimController.EUIAnimState.PostOutro)
                    || animController.CurState.Equals(UIAnimController.EUIAnimState.PreOutro))
                    continue;

                _outroListeninAnimControllerColl.Add(animController);

                animController.OnPostOutro += OnAnimPostOutro;
            }

            CheckIfAllAnimsCompleted();
        }

        private void OnAnimPostOutro(UIAnimController animController)
        {
            RemoveAnimController(animController);

            CheckIfAllAnimsCompleted();
        }

        private void RemoveAnimController(UIAnimController controller)
        {
            controller.OnPostOutro -= OnAnimPostOutro;

            _outroListeninAnimControllerColl.Remove(controller);
        }

        private void CheckIfAllAnimsCompleted()
        {
            if (_outroListeninAnimControllerColl.Count > 0)
                return;

            _callback?.Invoke();
        }

        private List<UIAnimTriggerBase> GetTriggersFromVMEvent(Action<EVMState> targetEvent)
        {
            List<UIAnimTriggerBase> _animTrigggerColl = new List<UIAnimTriggerBase>();

            Delegate[] delegates = targetEvent.GetInvocationList();

            foreach (Delegate del in delegates)
            {
                if (!(del.Target is UIAnimTriggerBase))
                    continue;

                _animTrigggerColl.Add(del.Target as UIAnimTriggerBase);
            }

            return _animTrigggerColl;
        }

        private List<UIAnimController> GetAnimationControllersFromTriggerOutro(List<UIAnimTriggerBase> triggerColl)
        {
            List<UIAnimController> _animControllerColl = new List<UIAnimController>();

            foreach (UIAnimTriggerBase trigger in triggerColl)
                _animControllerColl.AddRange(GetAnimationControllersFromTriggerOutro(trigger));

            return _animControllerColl;
        }

        private List<UIAnimController> GetAnimationControllersFromTriggerOutro(UIAnimTriggerBase trigger)
        {
            List<UIAnimController> _animControllerColl = new List<UIAnimController>();

            Delegate[] delegates = trigger.OnOutroTriggered.GetInvocationList();

            foreach (Delegate del in delegates)
            {
                if (!(del.Target is UIAnimController))
                    continue;

                _animControllerColl.Add(del.Target as UIAnimController);
            }

            return _animControllerColl;
        }
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

        [SerializeField] private bool _disableVMsUnderneath;

        public bool DisableVMsUnderneath { get { return _disableVMsUnderneath; } }

        private Canvas _canvas;
        private VMDeactivationController _deactivationController;

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

            _deactivationController = new VMDeactivationController();

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
            _canvas.enabled = true;

            _deactivationController.StopDeactivationListening();

            VMState = EVMState.Active;

            OnActivateCustomActions();
        }

        /// <summary>
        /// Do not call this manually, instead call DeactivateUI()
        /// </summary>
        public void Deactivate()
        {
            _deactivationController.ListenOutroAnimsTriggeredByVM(OnVMStateChanged, OnOutroAnimsFinished);

            VMState = EVMState.Deactive;
        }

        private void OnOutroAnimsFinished()
        {
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