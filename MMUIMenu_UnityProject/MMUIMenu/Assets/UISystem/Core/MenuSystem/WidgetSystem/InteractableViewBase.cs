using System.Reflection;
using UnityEngine;

namespace MVVM
{
    public abstract class InteractableViewBase<TPLD, TViewValue> : MonoBehaviour, IVMStateObserver
    where TPLD : IPLDBase
    {
        private VMBase _cachedViewModel;
        private PropertyInfo _cachedVMProperty;
        private MethodInfo _cachedMethodInfo;
        private PropertyChangeValidator _propertyChangeValidator;

        protected abstract string _viewModelMethodName { get; }

        private void Awake()
        {
            Init();

            ((IVMStateObserver)this).RegisterToVMStateEvents();

            RegisterEvents();

            AwakeCustomActions();
        }

        private void OnDestroy()
        {
            _cachedViewModel.OnPropertyChanged -= OnPropertyChanged;

            ((IVMStateObserver)this).UnregisterFromVMStateEvents();

            UnregisterEvents();

            OnDestroyCustomActions();
        }

        private void Init()
        {
            if (_cachedViewModel == null)
                _cachedViewModel = GetComponentInParent<VMBase>();

            _cachedViewModel.OnPropertyChanged += OnPropertyChanged;

            _cachedVMProperty = BindingExtensions.GetPropertyInfoOf<TPLD>(_cachedViewModel);
            _cachedMethodInfo = BindingExtensions.GetMethodInfoOf(_cachedViewModel, _viewModelMethodName);

            _propertyChangeValidator = new PropertyChangeValidator();
        }

        private void RegisterEvents()
        {
            RegisterEventsCustomActions();
        }

        private void UnregisterEvents()
        {
            UnregisterEventsCustomActions();
        }

        private void OnPropertyChanged()
        {
            TPLD newPLD = (TPLD)_cachedVMProperty.GetValue(_cachedViewModel);

            if (!_propertyChangeValidator.IsPropertyDirty(newPLD))
                return;

            ParsePLD(newPLD);
        }

        protected void UpdateViewModel(TViewValue value)
        {
            _cachedMethodInfo.Invoke(_cachedViewModel, new object[] { value });
        }

        #region IVMStateObserver Implementation
        VMBase IVMStateObserver.GetViewModel()
        {
            if (_cachedViewModel == null)
                _cachedViewModel = GetComponentInParent<VMBase>();

            return _cachedViewModel;
        }

        void IVMStateObserver.OnVMPreActivation()
        {
            OnVMPreActivationCustomActions();
        }

        void IVMStateObserver.OnVMPostActivation()
        {
            OnVMPostActivationCustomActions();
        }

        void IVMStateObserver.OnVMPreDeactivation()
        {
            OnVMPreDeactivationCustomActions();
        }

        void IVMStateObserver.OnVMPostDeactivation()
        {
            OnVMPostDeactivationCustomActions();
        }

        protected virtual void OnVMPreActivationCustomActions() { }
        protected virtual void OnVMPostActivationCustomActions() { }
        protected virtual void OnVMPreDeactivationCustomActions() { }
        protected virtual void OnVMPostDeactivationCustomActions() { }
        #endregion

        /// <summary>
        /// Called at Awake
        /// </summary>
        protected abstract void RegisterEventsCustomActions();

        /// <summary>
        /// Called at OnDestroy
        /// </summary>
        protected abstract void UnregisterEventsCustomActions();

        protected abstract void ParsePLD(TPLD pld);
        protected virtual void AwakeCustomActions() { }
        protected virtual void OnDestroyCustomActions() { }
    }
}