using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MVVM
{
    [RequireComponent(typeof(UISpawnController))]
    public abstract class InteractableCompositeViewBase<TPLD> : MonoBehaviour
    where TPLD : IPLDBase
    {
        private VMBase _cachedViewModel;
        private PropertyInfo _cachedVMProperty;
        private Dictionary<string, MethodInfo> _cachedMethodInfoColl;
        private PropertyChangeValidator _propertyChangeValidator;
        private UISpawnController _spawnController;

        protected abstract List<string> _viewModelMethodNameColl { get; }

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
            if (_cachedViewModel == null)
                _cachedViewModel = GetComponentInParent<VMBase>();
    
            _cachedViewModel.OnPropertyChanged += OnPropertyChanged;
    
            _cachedVMProperty = BindingExtensions.GetListPropertyInfoOf<TPLD>(_cachedViewModel);

            _cachedMethodInfoColl = new Dictionary<string, MethodInfo>();
            foreach (string methodName in _viewModelMethodNameColl)
                _cachedMethodInfoColl.Add(methodName, BindingExtensions.GetMethodInfoOf(_cachedViewModel, methodName));

            _propertyChangeValidator = new PropertyChangeValidator();
    
            _spawnController = GetComponent<UISpawnController>();
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
            List<TPLD> newPLDColl = (List<TPLD>)_cachedVMProperty.GetValue(_cachedViewModel);
    
            if (!_propertyChangeValidator.IsPropertyDirty(newPLDColl))
                return;
    
            List<RectTransform> subContainerColl = PrepareSpawnables(newPLDColl);
    
            ParsePLD(newPLDColl, subContainerColl);
        }

        protected void UpdateViewModel(string methodName, params object[] parameters)
        {
            MethodInfo targetMethodInfo = _cachedMethodInfoColl[methodName];

            targetMethodInfo.Invoke(_cachedViewModel, parameters);
        }

        private List<RectTransform> PrepareSpawnables(List<TPLD> newPLDColl)
        {
            return _spawnController.LoadSpawnables<RectTransform>(newPLDColl.Count, true);
        }

        /// <summary>
        /// Called at Awake
        /// </summary>
        protected abstract void RegisterEventsCustomActions();

        /// <summary>
        /// Called at OnDestroy
        /// </summary>
        protected abstract void UnregisterEventsCustomActions();

        protected abstract void ParsePLD(List<TPLD> pldColl, List<RectTransform> subContainerColl);
        protected virtual void AwakeCustomActions() { }
        protected virtual void OnDestroyCustomActions() { }
    }
}