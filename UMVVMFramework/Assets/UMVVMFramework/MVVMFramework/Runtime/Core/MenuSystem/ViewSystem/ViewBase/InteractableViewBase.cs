using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MVVM
{
    public abstract class InteractableViewBase<TPLD> : MonoBehaviour
    where TPLD : IPLDBase
    {
        private VMBase _cachedViewModel;
        private PropertyInfo _cachedVMProperty;
        private Dictionary<string, MethodInfo> _cachedMethodInfoColl;
        private PropertyChangeValidator _propertyChangeValidator;

        protected abstract List<string> _viewModelMethodNameColl { get; }

        private void Awake()
        {
            Init();

            AwakeCustomActions();
        }

        private void OnDestroy()
        {
            _cachedViewModel.OnPropertyChanged -= OnPropertyChanged;
            _cachedViewModel.OnVMStateChanged -= OnVMStateChanged;

            OnDestroyCustomActions();
        }

        private void Init()
        {
            if (_cachedViewModel == null)
                _cachedViewModel = GetComponentInParent<VMBase>();

            _cachedViewModel.OnPropertyChanged += OnPropertyChanged;

            _cachedViewModel.OnVMStateChanged += OnVMStateChanged;

            _cachedVMProperty = BindingExtensions.GetPropertyInfoOf<TPLD>(_cachedViewModel);

            _cachedMethodInfoColl = new Dictionary<string, MethodInfo>();
            foreach (string methodName in _viewModelMethodNameColl)
                _cachedMethodInfoColl.Add(methodName, BindingExtensions.GetMethodInfoOf(_cachedViewModel, methodName));

            _propertyChangeValidator = new PropertyChangeValidator();
        }
        
        private void OnVMStateChanged(EVMState state)
        {
            switch (state)
            {
                case EVMState.Active:
                    RegisterEvents();
                    break;
                case EVMState.Deactive:
                    UnregisterEvents();
                    break;
            }
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

        protected void UpdateViewModel(string methodName, params object[] parameters)
        {
            MethodInfo targetMethodInfo = _cachedMethodInfoColl[methodName];

            targetMethodInfo.Invoke(_cachedViewModel, parameters);
        }

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
