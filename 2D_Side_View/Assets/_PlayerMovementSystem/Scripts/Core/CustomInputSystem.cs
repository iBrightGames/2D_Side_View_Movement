// ============================================
// INPUT CONFIGS
// ============================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


namespace PlayerControlSystem
{
    public enum InputDeviceType { Keyboard , Mouse}
    public enum InputTriggerType { Pressed, Released, Held, LongPress, MultiClick }
    public enum InputAxis { Horizontal, Vertical }
    public enum InputPolarity { Positive, Negative }


    [System.Serializable]
    public class UserInput
    {
        [Tooltip("Tuşlar (aynı yön için alternatifler)")]
        public Key[] keyCode;

        [Tooltip("Tuşlar (aynı yön için alternatifler)")]
        public MouseButton[] mouseButton;

        [Tooltip("Hangi eksen?")]
        public InputAxis axis;

        [Tooltip("Hangi yön?")]
        public InputPolarity polarity;

        public Vector2 GetDirection()
        {
            float value = (polarity == InputPolarity.Positive) ? 1f : -1f;

            return axis == InputAxis.Horizontal
                ? new Vector2(value, 0)
                : new Vector2(0, value);
        }
    }


    [CreateAssetMenu(fileName = "PlayerInput", menuName = "PlayerInput")]
    public class PlayerInput : ScriptableObject
    {
        public InputDeviceType DeviceType;
        public InputTriggerType TriggerType;
        public List<UserInput> userInputs;
        public float PressDuration;
        public int ClickCount;

    }



}



