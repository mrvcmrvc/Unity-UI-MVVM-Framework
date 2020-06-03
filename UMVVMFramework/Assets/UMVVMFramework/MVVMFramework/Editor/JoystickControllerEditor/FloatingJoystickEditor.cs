using UnityEditor;
using UnityEngine;

namespace MVVM
{
    [CustomEditor(typeof(FloatingJoystick))]
    public class FloatingJoystickEditor : JoystickEditor
    {
        private SerializedProperty threshold;
        private SerializedProperty alwaysVisible;
        private SerializedProperty baseDistance;

        protected override void OnEnable()
        {
            threshold = serializedObject.FindProperty("JoystickBaseMoveThreshold");
            alwaysVisible = serializedObject.FindProperty("AlwaysVisible");
            baseDistance = serializedObject.FindProperty("BaseFixedDistance");

            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(threshold, new GUIContent("Distance Threshold", "The distance that can be between finger and joystick."));
            EditorGUILayout.PropertyField(alwaysVisible, new GUIContent("Always Visible", "Keep joystick always visible on the screen."));
            EditorGUILayout.PropertyField(baseDistance, new GUIContent("Distance Bottom Center", "Keep joystick at bottom center by given distance."));

            serializedObject.ApplyModifiedProperties();

        }
    }
}