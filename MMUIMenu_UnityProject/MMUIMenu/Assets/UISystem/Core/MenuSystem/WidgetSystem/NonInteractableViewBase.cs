using System.Reflection;
using UnityEngine;

namespace MVVM
{
    public abstract class NonInteractableViewBase<TPLD> : MonoBehaviour, IVMStateObserver
    where TPLD : IPLDBase
    {        
        private VMBase _cachedViewModel;
        private PropertyInfo _cachedVMProperty;
        private PropertyChangeValidator _propertyChangeValidator;

        private void Awake()
        {
            Init();

            ((IVMStateObserver)this).RegisterToVMStateEvents();

            AwakeCustomActions();
        }

        private void OnDestroy()
        {
            _cachedViewModel.OnPropertyChanged -= OnPropertyChanged;

            ((IVMStateObserver)this).UnregisterFromVMStateEvents();

            OnDestroyCustomActions();
        }

        private void Init()
        {
            if(_cachedViewModel == null)
                _cachedViewModel = GetComponentInParent<VMBase>();

            _cachedViewModel.OnPropertyChanged += OnPropertyChanged;

            _cachedVMProperty = BindingExtensions.GetPropertyInfoOf<TPLD>(_cachedViewModel);

            _propertyChangeValidator = new PropertyChangeValidator();
        }

        private void OnPropertyChanged()
        {
            TPLD newPLD = (TPLD)_cachedVMProperty.GetValue(_cachedViewModel);

            if (!_propertyChangeValidator.IsPropertyDirty(newPLD))
                return;

            ParsePLD(newPLD);
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

        protected abstract void ParsePLD(TPLD pld);
        protected virtual void AwakeCustomActions() { }
        protected virtual void OnDestroyCustomActions() { }
    }
}