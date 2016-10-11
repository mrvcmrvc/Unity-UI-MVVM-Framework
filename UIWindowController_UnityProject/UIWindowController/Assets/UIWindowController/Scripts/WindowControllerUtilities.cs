using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIWindowInfo
{
    public int WindowType;
    public GameObject Window;

    public WindowController Instance { get; private set; }

    public WindowController SetInstance()
    {
        var instObj = GameObject.Instantiate(Window, Vector3.zero, Quaternion.identity) as GameObject;
        Instance = instObj.GetComponent<WindowController>();

        Transform parent = null;
        if (GeneralWindowController.Instance != null)
            parent = GeneralWindowController.Instance.gameObject.transform;

        instObj.transform.parent = parent;

        return Instance;
    }

    ~UIWindowInfo()
    {
        Instance = null;
    }
}

public class WindowControllerUtilities : ScriptableObject
{
    public static string WindowControllerUtilAssetName = "WindowControllerUtilities";
    public static string WindowControllerUtilPath = "UIWindowController/Resources";
    public static string WindowControllerUtilAssetExt = ".asset";

    public static WindowControllerUtilities Instance { get; private set; }

    public List<UIWindowInfo> UIWindowList;

    public List<string> WindowTypeList { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        Instance = Resources.Load(WindowControllerUtilAssetName) as WindowControllerUtilities;

        if (Instance != null)
            Instance.InitWindows();
        else
            Debug.Log("Instance returned null.");
    }

    void InitWindows()
    {
        foreach (var info in UIWindowList)
            info.SetInstance().Init();
    }

    public void SaveWindowTypeList(List<string> _windowTypeList)
    {
        WindowTypeList = new List<string>(_windowTypeList);
    }
}
