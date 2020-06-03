using System.Reflection;
using UnityEngine;

namespace MVVM
{
    public abstract class NonInteractableViewBase<TPLD> : MonoBehaviour
    where TPLD : IPLDBase
    {        
        private VMBase _cachedViewModel;
        private PropertyInfo _cachedVMProperty;
        private PropertyChangeValidator _propertyChangeValidator;

        private void Awake()
        {
            Init();

            AwakeCustomActions();
        }

        private void OnDestroy()
        {
            _cachedViewModel.OnPropertyChanged -= OnPropertyChanged;

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

        protected abstract void ParsePLD(TPLD pld);
        protected virtual void AwakeCustomActions() { }
        protected virtual void OnDestroyCustomActions() { }
    }
}