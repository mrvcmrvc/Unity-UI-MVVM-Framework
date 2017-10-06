using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuManager : MonoBehaviour
{
    public static UIMenuManager Instance { get; private set; }

    public Action<UIMenu> ActivateUIMenuRequested, DeactivateUIMenuRequested;

    private Stack<UIMenu> _menuStack = new Stack<UIMenu>();
    private Stack<UIMenu> _closeMenuStack = new Stack<UIMenu>();
    private UIMenu _openMenu;
    private bool _isDeactivationFinished;

    private void Awake()
    {
        Instance = this;

        _isDeactivationFinished = true;
        
        UIMenu.OnPostDeactivation += StartCloseMenu;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDestroy()
    {
        UIMenu.OnPostDeactivation -= StartCloseMenu;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;

        Instance = null;
    }

    public void OnBackPressed()
    {
        if(_menuStack.Count > 0)
            _menuStack.Peek().OnBackPressed(null);
    }

    private void OnSceneUnloaded(Scene unloadedScene)
    {
        _menuStack.Clear();
    }

    public void OpenUIMenu(UIMenu menuRequestedToActivate)
    {
        if (_menuStack.Count > 0)
        {
            if (menuRequestedToActivate.DisableMenusUnderneath)
            {
                foreach (var underneathMenu in _menuStack)
                {
                    if(underneathMenu != menuRequestedToActivate)
                        _closeMenuStack.Push(underneathMenu);

                    if (underneathMenu.DisableMenusUnderneath)
                        break;
                }
            }

            var previousCanvas = _menuStack.Peek().Canvas;

            menuRequestedToActivate.transform.SetAsLastSibling();
        }

        _openMenu = menuRequestedToActivate;

        if (!_isDeactivationFinished)
            return;

        StartCloseMenu(null);
    }

    private void StartCloseMenu(UIMenu closedMenu)
    {
        if (_closeMenuStack.Count == 0)
        {
            if(_openMenu == null)
            {
                foreach (var menu in _menuStack)
                {
                    if (!menu.IsPreActivationFinished)
                        menu.Activate();

                    if (menu.DisableMenusUnderneath)
                        break;
                }
            }
            else
            {
                if (!_openMenu.IsPreActivationFinished)
                {
                    _menuStack.Push(_openMenu);

                    _openMenu.Activate();
                }


                _openMenu = null;
            }

            _isDeactivationFinished = true;

            return;
        }

        _isDeactivationFinished = false;

        var instance = _closeMenuStack.Pop();
        if(!instance.IsPreDeactivationFinished)
            instance.Deactivate();
    }

    public void CloseUIMenu(UIMenu menuRequestedToDeactivate)
    {
        if (_menuStack.Count == 0)
        {
            Debug.LogWarningFormat(menuRequestedToDeactivate, "{0} cannot be closed because menu stack is empty", menuRequestedToDeactivate.GetType());
            return;
        }

        if (_menuStack.Peek() != menuRequestedToDeactivate)
        {
            Debug.LogWarningFormat(menuRequestedToDeactivate, "{0} cannot be closed because it is not on top of stack", menuRequestedToDeactivate.GetType());
            return;
        }

        CloseTopMenu();
    }

    public void CloseTopMenu()
    {
        var instance = _menuStack.Pop();

        _closeMenuStack.Push(instance);

        if (_closeMenuStack.Count >= 2)
            return;

        StartCloseMenu(null);
    }
}
