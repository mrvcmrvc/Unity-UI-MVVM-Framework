using MVVM;
using UnityEditor.AnimatedValues;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(UnityUIButton), true)]
    [CanEditMultipleObjects]
    public class UnityUIButtonEditor : SelectableEditor
    {
        SerializedProperty m_TransitionProperty;
        SerializedProperty m_SpriteStateProperty;
        SerializedProperty m_TargetGraphicProperty;
        SerializedProperty m_StartListeningProperty;

        AnimBool m_ShowSpriteTrasition = new AnimBool();

        protected override void OnEnable()
        {
            m_TransitionProperty = serializedObject.FindProperty("m_Transition");
            m_SpriteStateProperty = serializedObject.FindProperty("SubGraphicData");
            m_TargetGraphicProperty = serializedObject.FindProperty("SubTargetGraphic");
            m_StartListeningProperty = serializedObject.FindProperty("StartListeningOnEnable");

            base.OnEnable();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(m_StartListeningProperty);

            var trans = (Selectable.Transition)m_TransitionProperty.enumValueIndex;

            m_ShowSpriteTrasition.target = (!m_TransitionProperty.hasMultipleDifferentValues && trans == Button.Transition.SpriteSwap);

            if (EditorGUILayout.BeginFadeGroup(m_ShowSpriteTrasition.faded))
            {
                EditorGUILayout.PropertyField(m_TargetGraphicProperty);
                EditorGUILayout.PropertyField(m_SpriteStateProperty);
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndFadeGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
