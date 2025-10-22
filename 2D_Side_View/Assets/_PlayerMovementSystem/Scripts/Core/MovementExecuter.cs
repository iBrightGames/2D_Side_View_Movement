using UnityEngine;

namespace PlayerControlSystem
{
    public static class MovementExecuter
    {
        public static void ExecuteMovement(Rigidbody2D rb,UserInput userInput, InputMovementBridge bridge,  bool showDebug = false)
        {
            var config = bridge.movementConfig;

            switch (config.forceType)
            {
                case ForceType.AddForce:
                    ApplyAddForce(rb, bridge.CalculateLinearForce(userInput), showDebug);
                    break;

                case ForceType.AddImpulse:
                    ApplyAddImpulse(rb, bridge.CalculateLinearForce(userInput), showDebug);
                    break;

                case ForceType.AddTorque:
                    ApplyAddTorque(rb, bridge.CalculateAngularForce(userInput), showDebug);
                    break;

                case ForceType.AddAngularImpulse:
                    ApplyAddAngularImpulse(rb, bridge.CalculateAngularForce(userInput), showDebug);
                    break;

                case ForceType.SetVelocity:
                    ApplySetVelocity(rb, bridge.CalculateVelocity(userInput), showDebug);
                    break;
            }
        }

        private static void ApplyAddForce(Rigidbody2D rb,  Vector2 force, bool showDebug)
        {

            if (force != Vector2.zero)
            {
                rb.AddForce(force, ForceMode2D.Force);
                rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity,100);
                rb.linearDamping = 1f; // sürtünme ekle



                if (showDebug)
                    Debug.Log($"[AddForce] Force: {force}, Velocity: {rb.linearVelocity}");
            }
        }

        private static void ApplyAddImpulse(Rigidbody2D rb,  Vector2 impulse, bool showDebug)
        {

            if (impulse != Vector2.zero)
            {
                rb.AddForce(impulse, ForceMode2D.Impulse);

                if (showDebug)
                    Debug.Log($"[AddImpulse] Impulse: {impulse}, Velocity: {rb.linearVelocity}");
            }
        }

        private static void ApplyAddTorque(Rigidbody2D rb, float torque, bool showDebug)
        {

            if (torque != 0f)
            {
                rb.AddTorque(torque, ForceMode2D.Force);

                if (showDebug)
                    Debug.Log($"[AddTorque] Torque: {torque}, AngularVelocity: {rb.angularVelocity}");
            }
        }

        private static void ApplyAddAngularImpulse(Rigidbody2D rb, float torque, bool showDebug)
        {

            if (torque != 0f)
            {
                rb.AddTorque(torque, ForceMode2D.Impulse);

                if (showDebug)
                    Debug.Log($"[AddAngularImpulse] Torque: {torque}, AngularVelocity: {rb.angularVelocity}");
            }
        }

        private static void ApplySetVelocity(Rigidbody2D rb,  Vector2 velocity, bool showDebug)
        {

            rb.linearVelocity = velocity;

            if (showDebug)
                Debug.Log($"[SetVelocity] New Velocity: {velocity}");
        }
    }
}

