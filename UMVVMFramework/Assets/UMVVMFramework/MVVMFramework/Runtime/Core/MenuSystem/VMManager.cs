using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVVM
{
    public class VMManager : MonoBehaviour
    {
        #region Events
        public Action<VMBase> OnVMInitCompleted;
        #endregion

        private static VMManager _instance;
        public static VMManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<VMManager>();

                return _instance;
            }
        }

        [SerializeField] private bool _toggleDebug;

        public List<VMBase> VMList { get; private set; }
        public List<VMBase> ActiveVMColl { get; private set; }

        private List<VMBase> _closeVMColl = new List<VMBase>();
        private List<VMBase> _nextOpeningVMColl = new List<VMBase>();
        private bool _isDeactivationFinished;

        private void Awake()
        {
            ActiveVMColl = new List<VMBase>();

            _isDeactivationFinished = true;

            VMBase.OnVMStateChanged_Static += ONVMStateChanged;
            VMBase.OnVMInited += OnNewVMInitCompleted;
            VMBase.OnVMDestroyed += OnVMDestroyed;

            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDestroy()
        {
            VMBase.OnVMStateChanged_Static -= ONVMStateChanged;
            VMBase.OnVMInited -= OnNewVMInitCompleted;
            VMBase.OnVMDestroyed -= OnVMDestroyed;

            SceneManager.sceneUnloaded -= OnSceneUnloaded;

            _instance = null;
        }

        public void OnBackPressed()
        {
            if (ActiveVMColl.Count > 0)
                CloseVM(ActiveVMColl[ActiveVMColl.Count - 1]);
        }

        private void OnSceneUnloaded(Scene unloadedScene)
        {
            if (_toggleDebug)
                Debug.Log("Scene Unloaded, Removing All Registered Windows!");

            ActiveVMColl.Clear();

            _isDeactivationFinished = true;
        }

        public void OpenVM(VMBase vmRequestedToActivate)
        {
            if (ActiveVMColl.Contains(vmRequestedToActivate))
                return;

            if (ActiveVMColl.Count > 0)
            {
                if (vmRequestedToActivate.DisableVMsUnderneath)
                {
                    foreach (var underneathVM in ActiveVMColl)
                    {
                        if (underneathVM != vmRequestedToActivate)
                            _closeVMColl.Add(underneathVM);

                        if (underneathVM.DisableVMsUnderneath)
                            break;
                    }
                }

                vmRequestedToActivate.transform.SetAsLastSibling();
            }

            _nextOpeningVMColl.Add(vmRequestedToActivate);

            if (!_isDeactivationFinished)
                return;

            StartCloseVM(null);
        }

        private void ONVMStateChanged(VMBase closedVM, EVMState curState)
        {
            if (!curState.Equals(EVMState.Deactive))
                return;

            StartCloseVM(null);
        }

        private void StartCloseVM(Action callback)
        {
            if (_closeVMColl.Count == 0)
            {
                callback?.Invoke();

                if (_nextOpeningVMColl == null)
                {
                    foreach (var vm in ActiveVMColl)
                    {
                        if (vm.VMState.Equals(EVMState.Deactive))
                            vm.Activate();

                        if (vm.DisableVMsUnderneath)
                            break;
                    }
                }
                else
                {
                    foreach(VMBase next in _nextOpeningVMColl)
                    {
                        if (next.VMState.Equals(EVMState.Deactive))
                        {
                            ActiveVMColl.Add(next);

                            next.Activate();
                        }
                    }

                    _nextOpeningVMColl.Clear();
                }

                _isDeactivationFinished = true;

                return;
            }

            _isDeactivationFinished = false;

            var instance = _closeVMColl[0];
            _closeVMColl.RemoveAt(0);

            if (instance.VMState.Equals(EVMState.Active))
                instance.Deactivate();
        }

        public void CloseVM(VMBase instance, Action callback = null)
        {
            if (ActiveVMColl.Count == 0)
            {
                Debug.LogWarningFormat(instance, "{0} cannot be closed because VM list is empty", instance.GetType());
                return;
            }

            ActiveVMColl.Remove(instance);

            if (_toggleDebug)
                Debug.Log("Close Top VM called, closing window: " + instance.GetType());

            if (_closeVMColl.Contains(instance))
            {
                Debug.LogWarningFormat("Close VM list already contains window: " + instance.GetType());
                return;
            }

            _closeVMColl.Add(instance);

            if (_closeVMColl.Count >= 2)
                return;

            StartCloseVM(callback);
        }

        public void CloseAllVMs(Action callback)
        {
            if (ActiveVMColl.Count == 0)
                callback?.Invoke();

            for (int i = ActiveVMColl.Count - 1; i >= 0; i--)
                CloseVM(ActiveVMColl[i], callback);
        }

        public void CloseAllVMsExcept(Type vmType, Action callback)
        {
            if (ActiveVMColl.Count == 0)
                callback?.Invoke();

            for (int i = ActiveVMColl.Count - 1; i >= 0; i--)
            {
                if (ActiveVMColl[ActiveVMColl.Count - 1].GetType().Equals(vmType))
                    continue;

                CloseVM(ActiveVMColl[i], callback);
            }
        }

        public T GetOpenVM<T>()
            where T : VMBase
        {
            return (T)ActiveVMColl.FirstOrDefault(val => val is T);
        }

        public T GetVM<T>()
            where T : VMBase
        {
            T target = (T)VMList.FirstOrDefault(val => val is T);

            if (target == null)
                target = FindObjectOfType<T>();

            return target;
        }

        public bool IsVMActive<T>()
            where T : VMBase
        {
            return GetOpenVM<T>() != null;
        }

        public bool IsAnyVMActive()
        {
            return ActiveVMColl != null && ActiveVMColl.Count > 0;
        }

        public bool IsAnyVMActive(params Type[] vmTypeCollection)
        {
            foreach (Type type in vmTypeCollection)
            {
                VMBase targetUI = ActiveVMColl.FirstOrDefault(val => val.GetType().Equals(type));

                if (targetUI != null)
                    return true;
            }

            return false;
        }

        public bool IsAnyVMActive(out List<VMBase> activeVMCollection, params Type[] vmTypeCollection)
        {
            activeVMCollection = new List<VMBase>();

            foreach (Type type in vmTypeCollection)
            {
                VMBase targetUI = ActiveVMColl.FirstOrDefault(val => val.GetType().Equals(type));

                if (targetUI != null)
                    activeVMCollection.Add(targetUI);
            }

            return activeVMCollection.Count > 0;
        }

        private void OnNewVMInitCompleted(VMBase vm)
        {
            if (VMList == null)
                VMList = new List<VMBase>();

            VMList.Add(vm);

            OnVMInitCompleted?.Invoke(vm);
        }

        private void OnVMDestroyed(VMBase vm)
        {
            if (VMList == null)
                return;

            VMList.Remove(vm);
        }
    }
}