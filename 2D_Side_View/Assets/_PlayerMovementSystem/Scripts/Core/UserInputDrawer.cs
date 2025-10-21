using UnityEngine;
using UnityEditor;

namespace PlayerControlSystem
{
    [CustomPropertyDrawer(typeof(UserInput))]
    public class UserInputDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // referanslar
            SerializedProperty keyCodeProp = property.FindPropertyRelative("keyCode");
            SerializedProperty mouseButtonProp = property.FindPropertyRelative("mouseButton");
            SerializedProperty axisProp = property.FindPropertyRelative("axis");
            SerializedProperty polarityProp = property.FindPropertyRelative("polarity");

            // üst başlık
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(position, label);
            position.y += EditorGUIUtility.singleLineHeight + 2;

            // üst objeyi PlayerInput olarak çek
            var playerInput = property.serializedObject.targetObject as PlayerInput;

            // device'a göre input alanı
            if (playerInput != null)
            {
                if (playerInput.DeviceType == InputDeviceType.Keyboard)
                {
                    EditorGUI.PropertyField(position, keyCodeProp, new GUIContent("Keys"), true);
                    position.y += EditorGUI.GetPropertyHeight(keyCodeProp, true) + 2;
                }
                else if (playerInput.DeviceType == InputDeviceType.Mouse)
                {
                    EditorGUI.PropertyField(position, mouseButtonProp, new GUIContent("Mouse Buttons"), true);
                    position.y += EditorGUI.GetPropertyHeight(mouseButtonProp, true) + 2;
                }
            }

            // ortak alanlar
            EditorGUI.PropertyField(position, axisProp);
            position.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.PropertyField(position, polarityProp);
            position.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty keyCodeProp = property.FindPropertyRelative("keyCode");
            SerializedProperty mouseButtonProp = property.FindPropertyRelative("mouseButton");

            float height = EditorGUIUtility.singleLineHeight + 4; // başlık

            var playerInput = property.serializedObject.targetObject as PlayerInput;
            if (playerInput != null)
            {
                if (playerInput.DeviceType == InputDeviceType.Keyboard)
                    height += EditorGUI.GetPropertyHeight(keyCodeProp, true) + 2;
                else if (playerInput.DeviceType == InputDeviceType.Mouse)
                    height += EditorGUI.GetPropertyHeight(mouseButtonProp, true) + 2;
            }

            // axis + polarity
            height += (EditorGUIUtility.singleLineHeight + 2) * 2;

            return height;
        }
    }
}

// using UnityEngine;
// using UnityEditor;
// using PlayerControlSystem;


// namespace PlayerControlSystem
// {
//     [CustomPropertyDrawer(typeof(UserInput))]
//     public class UserInputDrawer : PropertyDrawer
//     {
//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             EditorGUI.BeginProperty(position, label, property);

//             SerializedProperty keyCodeProp = property.FindPropertyRelative("keyCode");
//             SerializedProperty mouseButtonProp = property.FindPropertyRelative("mouseButton");
//             SerializedProperty axisProp = property.FindPropertyRelative("axis");
//             SerializedProperty polarityProp = property.FindPropertyRelative("polarity");

//             // Başlık
//             position.height = EditorGUIUtility.singleLineHeight;
//             EditorGUI.LabelField(position, label);

//             position.y += EditorGUIUtility.singleLineHeight + 2;

//             // DeviceType conditional
//             PlayerInput parent = property.serializedObject.targetObject as PlayerInput;
//             if (parent != null)
//             {
//                 if (parent.DeviceType == InputDeviceType.Keyboard)
//                 {
//                     EditorGUI.PropertyField(position, keyCodeProp, new GUIContent("Key Codes"), true);
//                     position.y += EditorGUI.GetPropertyHeight(keyCodeProp) + 2;
//                 }
//                 else if (parent.DeviceType == InputDeviceType.Mouse)
//                 {
//                     EditorGUI.PropertyField(position, mouseButtonProp, new GUIContent("Mouse Buttons"), true);
//                     position.y += EditorGUI.GetPropertyHeight(mouseButtonProp) + 2;
//                 }


//             }
//             EditorGUI.PropertyField(position, axisProp);
//             position.y += EditorGUIUtility.singleLineHeight + 2;
                    
//             EditorGUI.PropertyField(position, polarityProp);
//             position.y += EditorGUIUtility.singleLineHeight + 2;

//             EditorGUI.EndProperty();
//         }

//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             SerializedProperty keyCodeProp = property.FindPropertyRelative("keyCode");
//             SerializedProperty mouseButtonProp = property.FindPropertyRelative("mouseButton");

//             float height = EditorGUIUtility.singleLineHeight + 2; // başlık

//             PlayerInput parent = property.serializedObject.targetObject as PlayerInput;
//             if (parent != null)
//             {
//                 if (parent.DeviceType == InputDeviceType.Keyboard)
//                     height += EditorGUI.GetPropertyHeight(keyCodeProp, true) + 2;
//                 else if (parent.DeviceType == InputDeviceType.Mouse)
//                     height += EditorGUI.GetPropertyHeight(mouseButtonProp, true) + 2;

//                 height += EditorGUIUtility.singleLineHeight + 2; // ValueType field
//             }

//             return height;
//         }
//     }

// }
