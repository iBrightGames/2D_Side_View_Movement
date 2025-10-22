using UnityEngine;
using UnityEditor;

namespace PlayerControlSystem
{
    [CustomEditor(typeof(UserInput))]
    public class UserInputEditor : Editor
    {
        SerializedProperty actionProp;
        SerializedProperty axisProp;
        SerializedProperty polarityProp;

        private void OnEnable()
        {
            actionProp = serializedObject.FindProperty("action");
            axisProp = serializedObject.FindProperty("axis");
            polarityProp = serializedObject.FindProperty("polarity");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("User Input Config", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(actionProp);
            EditorGUILayout.PropertyField(axisProp);
            EditorGUILayout.PropertyField(polarityProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
