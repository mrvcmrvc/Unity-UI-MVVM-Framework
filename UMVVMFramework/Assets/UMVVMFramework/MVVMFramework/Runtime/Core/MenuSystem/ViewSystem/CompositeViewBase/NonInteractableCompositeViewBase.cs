using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MVVM
{
    [RequireComponent(typeof(UISpawnController))]
    public abstract class NonInteractableCompositeViewBase<TPLD> : MonoBehaviour
    where TPLD : IPLDBase
    {
        private VMBase _cachedViewModel;
        private PropertyInfo _cachedVMProperty;
        private PropertyChangeValidator _propertyChangeValidator;
        private UISpawnController _spawnController;

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
            if (_cachedViewModel == null)
                _cachedViewModel = GetComponentInParent<VMBase>();

            _cachedViewModel.OnPropertyChanged += OnPropertyChanged;

            _cachedVMProperty = BindingExtensions.GetListPropertyInfoOf<TPLD>(_cachedViewModel);

            _propertyChangeValidator = new PropertyChangeValidator();

            _spawnController = GetComponent<UISpawnController>();
        }

        private void OnPropertyChanged()
        {
            List<TPLD> newPLDColl = (List<TPLD>)_cachedVMProperty.GetValue(_cachedViewModel);

            if (!_propertyChangeValidator.IsPropertyDirty(newPLDColl))
                return;

            List<RectTransform> subContainerColl = PrepareSpawnables(newPLDColl);

            ParsePLD(newPLDColl, subContainerColl);
        }

        private List<RectTransform> PrepareSpawnables(List<TPLD> newPLDColl)
        {
            return _spawnController.LoadSpawnables<RectTransform>(newPLDColl.Count, true);
        }

        protected abstract void ParsePLD(List<TPLD> pldColl, List<RectTransform> subContainerColl);
        protected virtual void AwakeCustomActions() { }
        protected virtual void OnDestroyCustomActions() { }
    }
}