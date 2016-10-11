using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WindowControllerUtilities))]
public class WindowControllerUtilitiesEditor : Editor
{
    WindowControllerUtilities myTarget;

    static List<string> _windowTypeList;
    MonoScript _enumScript = null;

    [InitializeOnLoadMethod]
    static void CreateAsset()
    {
        var alreadyCreatedInstance = Resources.Load(WindowControllerUtilities.WindowControllerUtilAssetName) as WindowControllerUtilities;

        if (alreadyCreatedInstance != null)
            return;

        var instance = CreateInstance<WindowControllerUtilities>();

        string fullPath = Path.Combine(Path.Combine("Assets", WindowControllerUtilities.WindowControllerUtilPath),
            WindowControllerUtilities.WindowControllerUtilAssetName + WindowControllerUtilities.WindowControllerUtilAssetExt
        );

        AssetDatabase.CreateAsset(instance, fullPath);
    }

    void OnEnable()
    {
        myTarget = Resources.Load(WindowControllerUtilities.WindowControllerUtilAssetName) as WindowControllerUtilities;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("WARNING: Put your window prefabs to any Resources folder! Otherwise, it wont work.");
        EditorGUILayout.Separator();

        EditorGUI.BeginChangeCheck();
        _enumScript = (MonoScript)EditorGUILayout.ObjectField("Enum", _enumScript, typeof(MonoScript), false);
        if(EditorGUI.EndChangeCheck())
        {
            string wholeContent = _enumScript.text;
            string splitString = _enumScript.name;
            string[] splitContent = wholeContent.Split(new string[] { splitString }, StringSplitOptions.RemoveEmptyEntries);

            string targetContent = splitContent[splitContent.Length - 1];
            targetContent = targetContent.Trim();
            targetContent = targetContent.Trim(new char[] { '{', '}', ' ' });

            string[] values = targetContent.Split(new char[] { ',' });

            _windowTypeList = new List<string>();
            for (int i = 0; i < values.Length; i++)
                _windowTypeList.Add(values[i].Trim());

            myTarget.SaveWindowTypeList(_windowTypeList);
        }

        if(myTarget.WindowTypeList.Count > 0)
            EditorGUILayout.LabelField("Window Type Guideline");
        else
            EditorGUILayout.LabelField("NO Window Type Guideline FOUND");

        foreach (var type in myTarget.WindowTypeList)
        {
            EditorGUILayout.LabelField(type + " = " + myTarget.WindowTypeList.IndexOf(type));
        }

        EditorGUILayout.Separator();

        base.OnInspectorGUI();
    }
}
