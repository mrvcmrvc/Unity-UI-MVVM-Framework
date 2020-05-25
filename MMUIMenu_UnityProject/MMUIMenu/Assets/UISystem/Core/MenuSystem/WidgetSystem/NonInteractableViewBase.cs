using System.Reflection;
using UnityEngine;

namespace MVVM
{
    public abstract class NonInteractableViewBase<TPLD> : MonoBehaviour
    where TPLD : IPLDBase
    {
        private VMBase _cachedViewModel;
        private PropertyInfo _cachedVMProperty;

        private int _prevPLDHash = -1;

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
            _cachedViewModel = GetComponentInParent<VMBase>();

            _cachedViewModel.OnPropertyChanged += OnPropertyChanged;

            _cachedVMProperty = BindingExtensions.GetPropertyInfoOf<TPLD>(_cachedViewModel);
        }

        private void OnPropertyChanged()
        {
            TPLD newPLD = (TPLD)_cachedVMProperty.GetValue(_cachedViewModel);

            if (newPLD.GetHashCode().Equals(_prevPLDHash))
                return;

            _prevPLDHash = newPLD.GetHashCode();

            ParsePLD(newPLD);
        }

        protected abstract void ParsePLD(TPLD pld);
        protected virtual void AwakeCustomActions() { }
        protected virtual void OnDestroyCustomActions() { }
    }
}