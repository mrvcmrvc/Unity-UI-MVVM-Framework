using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneralWindowController : MonoBehaviour
{
    public bool AllWindowsClosed { get; private set; }

    IEnumerator _openwindowRoutine, _deactivateWindowRoutine;
    WindowController _frontMostWindow;

    public List<WindowController> ActiveWindowList { get; private set; }

    public static GeneralWindowController Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        _openwindowRoutine = null;
        _deactivateWindowRoutine = null;

        _frontMostWindow = null;

        ActiveWindowList = new List<WindowController>();
    }

    void OnDestroy()
    {
        Instance = null;
    }

    void Update()
    {
        CheckIfAllWindowsClosed();
    }

    public void CloseAllWindows(bool immediately = false)
    {
        if (ActiveWindowList == null)
            return;

        ActiveWindowList.ForEach(w => CloseWindow(w, immediately));
    }

    public void CloseWindow(int windowType, bool immediately = false)
    {
        if (_deactivateWindowRoutine != null)
            StopCoroutine(_deactivateWindowRoutine);

        _deactivateWindowRoutine = CloseWindowRoutine(windowType, immediately);
        StartCoroutine(_deactivateWindowRoutine);
    }

    public void CloseWindow(WindowController window, bool immediately = false)
    {
        CloseWindow(window.WindowType, immediately);
    }

    IEnumerator CloseWindowRoutine(int activeWindowType, bool immediately = false)
    {
        WindowController activeWindow = ActiveWindowList.Find(w => w.WindowType == activeWindowType);

        if (activeWindow != null)
        {
            activeWindow.Deactivate(immediately);

            while (!activeWindow.IsPreDeactivationFinished)
                yield return new WaitForFixedUpdate();

            ActiveWindowList.Remove(activeWindow);
        }

        ReSortActiveWindows();

        SetFrontMostWindow();
    }

    void CheckIfAllWindowsClosed()
    {
        if (ActiveWindowList.Count == 0)
            AllWindowsClosed = true;
        else
            AllWindowsClosed = false;
    }

    bool CloseWindowIfActive(int windowType)
    {
        bool isAlreadyActive = CheckIfAlreadyActive(windowType);
        if (isAlreadyActive)
            CloseWindow(windowType);

        return isAlreadyActive;
    }

    bool CheckIfAlreadyActive(int windowType)
    {
        foreach (var w in ActiveWindowList)
            if (w.WindowType == windowType)
                return true;

        return false;
    }

    public void OpenWindow(int windowType, bool closeAllActiveWindows = false)
    {
        if(!CloseWindowIfActive(windowType))
        {
            if (_openwindowRoutine != null)
                StopCoroutine(_openwindowRoutine);

            _openwindowRoutine = OpenWindowRoutine(windowType, closeAllActiveWindows);
            StartCoroutine(_openwindowRoutine);
        }
    }

    public void OpenWindow<T>(int windowType, ref T resultWindow, bool closeAllActiveWindows = false) where T : class
    {
        if (!CloseWindowIfActive(windowType))
        {
            if (_openwindowRoutine != null)
                StopCoroutine(_openwindowRoutine);

            _openwindowRoutine = OpenWindowRoutine(windowType, closeAllActiveWindows);
            StartCoroutine(_openwindowRoutine);
        }

        resultWindow = GetWindow<T>();
    }

    IEnumerator OpenWindowRoutine(int windowType, bool closeAllActiveWindows = false)
    {
        WindowController window = null;
        window = WindowControllerUtilities.Instance.UIWindowList.Find(w => w.WindowType == windowType).Window.GetComponent<WindowController>();

        if (window == null || ActiveWindowList.Contains(window))
            yield break;

        ActiveWindowList.Add(window);

        window.Activate(ActiveWindowList.Count - 1);

        SetFrontMostWindow();
    }

    public T GetWindow<T>() where T : class
    {
        if (WindowControllerUtilities.Instance.UIWindowList.Count == 0)
            return null;

        foreach(var window in WindowControllerUtilities.Instance.UIWindowList)
        {
            if (window.Window.GetComponent<WindowController>().GetType() == typeof(T))
                return window.Window.GetComponent<WindowController>() as T;
        }

        return null;
    }

    public bool IsWindowOpen(int windowType)
    {
        return ActiveWindowList.Find(w => w.WindowType == windowType) != null;
    }

    void SetFrontMostWindow()
    {
        if (ActiveWindowList.Count == 0)
            _frontMostWindow = null;
        else
            _frontMostWindow = ActiveWindowList[ActiveWindowList.Count - 1];
    }

    public WindowController GetFrontMostWindow()
    {
        return _frontMostWindow;
    }

    void ReSortActiveWindows()
    {
        for (int i = 0; i < ActiveWindowList.Count; i++)
            ActiveWindowList[i].SetSortOrder(i);
    }
}
