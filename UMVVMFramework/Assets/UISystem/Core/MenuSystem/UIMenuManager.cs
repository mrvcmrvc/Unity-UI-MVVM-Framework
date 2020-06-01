using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MVVM
{
    public class UIMenuManager : MonoBehaviour
    {
        #region Events
        public Action<VMBase> OnUIMenuInitCompleted;
        #endregion

        private static UIMenuManager _instance;
        public static UIMenuManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<UIMenuManager>();

                return _instance;
            }
        }

        [SerializeField] private bool _toggleDebug;

        public List<VMBase> UIMenuList { get; private set; }
        public List<VMBase> ActiveUIMenuColl { get; private set; }

        private List<VMBase> _closeMenuColl = new List<VMBase>();
        private bool _isDeactivationFinished;
        private List<VMBase> _nextOpeningMenuColl = new List<VMBase>();

        private void Awake()
        {
            ActiveUIMenuColl = new List<VMBase>();

            _isDeactivationFinished = true;

            VMBase.OnVMStateChanged_Static += ONVMStateChanged;
            VMBase.OnUIMenuInited += OnNewUIInitCompleted;
            VMBase.OnUIMenuDestroyed += OnUIMenuDestroyed;

            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDestroy()
        {
            VMBase.OnVMStateChanged_Static -= ONVMStateChanged;
            VMBase.OnUIMenuInited -= OnNewUIInitCompleted;
            VMBase.OnUIMenuDestroyed -= OnUIMenuDestroyed;

            SceneManager.sceneUnloaded -= OnSceneUnloaded;

            _instance = null;
        }

        public void OnBackPressed()
        {
            if (ActiveUIMenuColl.Count > 0)
                CloseUIMenu(ActiveUIMenuColl[ActiveUIMenuColl.Count - 1]);
        }

        private void OnSceneUnloaded(Scene unloadedScene)
        {
            if (_toggleDebug)
                Debug.Log("Scene Unloaded, Removing All Registered Windows!");

            ActiveUIMenuColl.Clear();

            _isDeactivationFinished = true;
        }

        public void OpenUIMenu(VMBase menuRequestedToActivate)
        {
            if (ActiveUIMenuColl.Contains(menuRequestedToActivate))
                return;

            if (ActiveUIMenuColl.Count > 0)
            {
                if (menuRequestedToActivate.DisableMenusUnderneath)
                {
                    foreach (var underneathMenu in ActiveUIMenuColl)
                    {
                        if (underneathMenu != menuRequestedToActivate)
                            _closeMenuColl.Add(underneathMenu);

                        if (underneathMenu.DisableMenusUnderneath)
                            break;
                    }
                }

                menuRequestedToActivate.transform.SetAsLastSibling();
            }

            _nextOpeningMenuColl.Add(menuRequestedToActivate);

            if (!_isDeactivationFinished)
                return;

            StartCloseMenu(null);
        }

        private void ONVMStateChanged(VMBase closedMenu, EVMState curState)
        {
            if (!curState.Equals(EVMState.Deactive))
                return;

            StartCloseMenu(null);
        }

        private void StartCloseMenu(Action callback)
        {
            if (_closeMenuColl.Count == 0)
            {
                callback?.Invoke();

                if (_nextOpeningMenuColl == null)
                {
                    foreach (var menu in ActiveUIMenuColl)
                    {
                        if (menu.VMState.Equals(EVMState.Deactive))
                            menu.Activate();

                        if (menu.DisableMenusUnderneath)
                            break;
                    }
                }
                else
                {
                    foreach(VMBase next in _nextOpeningMenuColl)
                    {
                        if (next.VMState.Equals(EVMState.Deactive))
                        {
                            ActiveUIMenuColl.Add(next);

                            next.Activate();
                        }
                    }

                    _nextOpeningMenuColl.Clear();
                }

                _isDeactivationFinished = true;

                return;
            }

            _isDeactivationFinished = false;

            var instance = _closeMenuColl[0];
            _closeMenuColl.RemoveAt(0);

            if (instance.VMState.Equals(EVMState.Active))
                instance.Deactivate();
        }

        public void CloseUIMenu(VMBase menuRequestedToDeactivate)
        {
            if (_toggleDebug)
                Debug.Log("Trying to Close Window: " + menuRequestedToDeactivate.GetType());

            if (ActiveUIMenuColl.Count == 0)
            {
                Debug.LogWarningFormat(menuRequestedToDeactivate, "{0} cannot be closed because menu list is empty", menuRequestedToDeactivate.GetType());
                return;
            }

            CloseMenu(menuRequestedToDeactivate);
        }

        public void CloseMenu(VMBase instance, Action callback = null)
        {
            ActiveUIMenuColl.Remove(instance);

            if (_toggleDebug)
                Debug.Log("Close Top Menu called, closing window: " + instance.GetType());

            if (_closeMenuColl.Contains(instance))
            {
                Debug.LogWarningFormat("Close Menu list already contains window: " + instance.GetType());
                return;
            }

            _closeMenuColl.Add(instance);

            if (_closeMenuColl.Count >= 2)
                return;

            StartCloseMenu(callback);
        }

        public void CloseAllUIMenus(Action callback)
        {
            if (ActiveUIMenuColl.Count == 0)
                callback?.Invoke();

            for (int i = ActiveUIMenuColl.Count - 1; i >= 0; i--)
                CloseMenu(ActiveUIMenuColl[i], callback);
        }

        public void CloseAllUIMenusExcept(Type menuType, Action callback)
        {
            if (ActiveUIMenuColl.Count == 0)
                callback?.Invoke();

            for (int i = ActiveUIMenuColl.Count - 1; i >= 0; i--)
            {
                if (ActiveUIMenuColl[ActiveUIMenuColl.Count - 1].GetType().Equals(menuType))
                    continue;

                CloseMenu(ActiveUIMenuColl[i], callback);
            }
        }

        public T GetOpenMenu<T>()
            where T : VMBase
        {
            return (T)ActiveUIMenuColl.FirstOrDefault(val => val is T);
        }

        public T GetUIMenu<T>()
            where T : VMBase
        {
            T target = (T)UIMenuList.FirstOrDefault(val => val is T);

            if (target == null)
                target = FindObjectOfType<T>();

            return target;
        }

        public bool IsUIActive<T>()
            where T : VMBase
        {
            return GetOpenMenu<T>() != null;
        }

        public bool IsAnyUIActive()
        {
            return ActiveUIMenuColl != null && ActiveUIMenuColl.Count > 0;
        }

        public bool IsAnyUIActive(params Type[] menuTypeCollection)
        {
            foreach (Type type in menuTypeCollection)
            {
                VMBase targetUI = ActiveUIMenuColl.FirstOrDefault(val => val.GetType().Equals(type));

                if (targetUI != null)
                    return true;
            }

            return false;
        }

        public bool IsAnyUIActive(out List<VMBase> activeUIMenuCollection, params Type[] menuTypeCollection)
        {
            activeUIMenuCollection = new List<VMBase>();

            foreach (Type type in menuTypeCollection)
            {
                VMBase targetUI = ActiveUIMenuColl.FirstOrDefault(val => val.GetType().Equals(type));

                if (targetUI != null)
                    activeUIMenuCollection.Add(targetUI);
            }

            return activeUIMenuCollection.Count > 0;
        }

        private void OnNewUIInitCompleted(VMBase uiMenu)
        {
            if (UIMenuList == null)
                UIMenuList = new List<VMBase>();

            UIMenuList.Add(uiMenu);

            OnUIMenuInitCompleted?.Invoke(uiMenu);
        }

        private void OnUIMenuDestroyed(VMBase uiMenu)
        {
            if (UIMenuList == null)
                return;

            UIMenuList.Remove(uiMenu);
        }
    }
}