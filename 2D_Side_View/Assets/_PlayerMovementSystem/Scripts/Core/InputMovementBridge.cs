// ============================================
// BRIDGE
// ============================================

using UnityEngine;
using UnityEngine.InputSystem;


namespace PlayerControlSystem
{
    [CreateAssetMenu(fileName = "ForceConfig", menuName = "ForceConfig")]
    public class ForceConfig : ScriptableObject
    {
        [Header("Force Type")]
        public ForceType forceType;

        [Header("Magnitude")]
        public float forceMagnitude = 10f;


    }

    [CreateAssetMenu(fileName = "UserInput", menuName = "UserInput")]
    public class UserInput : ScriptableObject
    {
        public InputActionReference action;

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



    [System.Serializable]
    public class InputMovementBridge
    {
        public UserInput playerInput;
        public ForceConfig movementConfig;

        // Force hesaplama
        public Vector2 CalculateLinearForce(UserInput userInput)
        {
            return userInput.GetDirection() * movementConfig.forceMagnitude;
        }

        // Angular force hesaplama
        public float CalculateAngularForce(UserInput userInput)
        {
            switch (userInput.polarity)
            {
                case InputPolarity.Positive: return movementConfig.forceMagnitude;
                case InputPolarity.Negative: return -movementConfig.forceMagnitude;
            }
            return 0;
        }

        // Velocity hesaplama
        public Vector2 CalculateVelocity(UserInput userInput)
        {
            return userInput.GetDirection() * movementConfig.forceMagnitude;

        }



    }

}





