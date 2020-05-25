using System.Reflection;
using UnityEngine;

namespace MVVM
{
    public abstract class InteractableViewBase<TPLD, TViewValue> : MonoBehaviour
    where TPLD : IPLDBase
    {
        private VMBase _cachedViewModel;
        private PropertyInfo _cachedVMProperty;
        private MethodInfo _cachedMethodInfo;

        private int _prevPLDHash = -1;

        protected abstract string _viewModelMethodName { get; }

        private void Awake()
        {
            Init();

            RegisterEvents();

            AwakeCustomActions();
        }

        private void OnDestroy()
        {
            _cachedViewModel.OnPropertyChanged -= OnPropertyChanged;

            UnregisterEvents();

            OnDestroyCustomActions();
        }

        private void Init()
        {
            _cachedViewModel = GetComponentInParent<VMBase>();

            _cachedViewModel.OnPropertyChanged += OnPropertyChanged;

            _cachedVMProperty = BindingExtensions.GetPropertyInfoOf<TPLD>(_cachedViewModel);
            _cachedMethodInfo = BindingExtensions.GetMethodInfoOf(_cachedViewModel, _viewModelMethodName);
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

            if (newPLD.GetHashCode().Equals(_prevPLDHash))
                return;

            _prevPLDHash = newPLD.GetHashCode();

            ParsePLD(newPLD);
        }

        protected void UpdateViewModel(TViewValue value)
        {
            _cachedMethodInfo.Invoke(_cachedViewModel, new object[] { value });
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