using UnityEngine;
using UnityEditor;
using PlayerControlSystem;


namespace PlayerControlSystem
{
    [CustomEditor(typeof(PlayerInput))]
    public class PlayerInputEditor : Editor
    {
        SerializedProperty deviceTypeProp;
        SerializedProperty triggerTypeProp;
        SerializedProperty userInputsProp;
        SerializedProperty pressDurationProp;
        SerializedProperty clickCountProp;

        private void OnEnable()
        {
            deviceTypeProp = serializedObject.FindProperty("DeviceType");
            triggerTypeProp = serializedObject.FindProperty("TriggerType");
            userInputsProp = serializedObject.FindProperty("userInputs");
            pressDurationProp = serializedObject.FindProperty("PressDuration");
            clickCountProp = serializedObject.FindProperty("ClickCount");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(deviceTypeProp);
            EditorGUILayout.PropertyField(triggerTypeProp);

            InputTriggerType trigger = (InputTriggerType)triggerTypeProp.enumValueIndex;
            if (trigger == InputTriggerType.LongPress)
                EditorGUILayout.PropertyField(pressDurationProp);
            if (trigger == InputTriggerType.MultiClick)
                EditorGUILayout.PropertyField(clickCountProp);

            EditorGUILayout.PropertyField(userInputsProp, true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
