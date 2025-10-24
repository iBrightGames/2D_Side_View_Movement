
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControlSystem
{

    public enum InputAxis { Horizontal, Vertical }
    public enum InputPolarity { Positive, Negative }

    [System.Serializable]
    public class ForceConfig
    {
        [Header("Force Type")]
        public ForceType forceType;

        [Header("Magnitude")]
        public float forceMagnitude = 10f;

        [Tooltip("Hangi eksen?")]
        public InputAxis axis;

        [Tooltip("Hangi yön?")]
        public InputPolarity polarity;

        public Vector2 GetLinearForceVector()
        {
            Vector2 dir = axis switch
            {
                InputAxis.Horizontal => Vector2.right,
                InputAxis.Vertical => Vector2.up,
                _ => Vector2.zero
            };

            float sign = polarity switch
            {
                InputPolarity.Positive => 1f,
                InputPolarity.Negative => -1f,
                _ => 0f
            };

            return dir * forceMagnitude * sign;
        }

        public float GetAngularForce()
        {
            switch (polarity)
            {
                case InputPolarity.Positive: return forceMagnitude;
                case InputPolarity.Negative: return -forceMagnitude;
            }
            return 0;
        }

    }
    [System.Serializable]

    public class UserInput
    {
        public InputActionReference action;

        public bool IsPressed => action != null && action.action.IsPressed();
        public bool WasPerformedThisFrame => action != null && action.action.triggered;


    }

    [System.Serializable]
    public class InputForceBridge
    {
        public UserInput playerInput;
        public ForceConfig forceConfig;
        public bool IsValid => playerInput != null && forceConfig != null;


    }


}

