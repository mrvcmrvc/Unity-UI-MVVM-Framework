using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    public static UIMenuManager Instance { get; private set; }

    public Action<UIMenu> ActivateUIMenuRequested, DeactivateUIMenuRequested;

    private Stack<UIMenu> _menuStack = new Stack<UIMenu>();
    private Stack<UIMenu> _closeMenuStack = new Stack<UIMenu>();
    private UIMenu _openMenu;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void OnBackPressed()
    {
        if(_menuStack.Count > 0)
            _menuStack.Peek().OnBackPressed();
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

            menuRequestedToActivate.Canvas.sortingOrder = previousCanvas.sortingOrder + 1;
        }

        _menuStack.Push(menuRequestedToActivate);

        _openMenu = menuRequestedToActivate;

        StartCloseMenu(null);
    }

    private void StartCloseMenu(UIMenu closedMenu)
    {
        UIMenu.OnPostDeactivation -= StartCloseMenu;

        if (_closeMenuStack.Count == 0)
        {
            if(_openMenu == null)
            {
                foreach (var menu in _menuStack)
                {
                    if(!menu.IsPreActivationFinished)
                        menu.Activate();

                    if (menu.DisableMenusUnderneath)
                        break;
                }
            }
            else
            {
                if (!_openMenu.IsPreActivationFinished)
                    _openMenu.Activate();

                _openMenu = null;
            }

            return;
        }

        UIMenu.OnPostDeactivation += StartCloseMenu;

        var instance = _closeMenuStack.Pop();

        if(!instance.IsPreDeactivationFinished)
            instance.Deactivate();
    }

    public void CloseUIMenu(UIMenu menuRequestedToDeactivate)
    {
        if (_menuStack.Count == 0)
        {
            Debug.LogErrorFormat(menuRequestedToDeactivate, "{0} cannot be closed because menu stack is empty", menuRequestedToDeactivate.GetType());
            return;
        }

        if (_menuStack.Peek() != menuRequestedToDeactivate)
        {
            Debug.LogErrorFormat(menuRequestedToDeactivate, "{0} cannot be closed because it is not on top of stack", menuRequestedToDeactivate.GetType());
            return;
        }

        CloseTopMenu();
    }

    public void CloseTopMenu()
    {
        var instance = _menuStack.Pop();

        _closeMenuStack.Push(instance);

        StartCloseMenu(null);
    }
}
