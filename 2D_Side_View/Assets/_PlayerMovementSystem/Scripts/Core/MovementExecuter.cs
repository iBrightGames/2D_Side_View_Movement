using UnityEngine;

namespace PlayerControlSystem
{
    public enum ForceType
    {
        AddForce,
        AddImpulse,
        AddTorque,
        AddAngularImpulse,
        SetVelocity,
        MovePosition,
        AddRelativeForce,
        CustomGravity
    }

    public static class MovementExecuter
    {
        public static void ExecuteMovement(Rigidbody2D rb, InputForceBridge bridge, bool showDebug = false)
        {
            var config = bridge.forceConfig;

            switch (config.forceType)
            {
                case ForceType.AddForce:
                    ApplyAddForce(rb, config.GetLinearForceVector(), showDebug);
                    break;

                case ForceType.AddImpulse:
                    ApplyAddImpulse(rb, config.GetLinearForceVector(), showDebug);
                    break;

                case ForceType.AddTorque:
                    ApplyAddTorque(rb, config.GetAngularForce(), showDebug);
                    break;

                case ForceType.AddAngularImpulse:
                    ApplyAddAngularImpulse(rb, config.GetAngularForce(), showDebug);
                    break;

                case ForceType.SetVelocity:
                    ApplySetVelocity(rb, config.GetLinearForceVector(), showDebug);
                    break;

                case ForceType.MovePosition:
                    ApplyMovePosition(rb, config.GetLinearForceVector(), showDebug);
                    break;

                case ForceType.AddRelativeForce:
                    ApplyAddRelativeForce(rb, config.GetLinearForceVector(), showDebug);
                    break;

                case ForceType.CustomGravity:
                    ApplyCustomGravity(rb, config.GetLinearForceVector(), showDebug);
                    break;
            }
        }

        private static void ApplyAddForce(Rigidbody2D rb, Vector2 force, bool showDebug)
        {
            if (force == Vector2.zero) return;

            rb.AddForce(force, ForceMode2D.Force);
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, 100);
            rb.linearDamping = 1f;

            if (showDebug)
                Debug.Log($"[AddForce] Force: {force}, Velocity: {rb.linearVelocity}");
        }

        private static void ApplyAddImpulse(Rigidbody2D rb, Vector2 impulse, bool showDebug)
        {
            if (impulse == Vector2.zero) return;

            rb.AddForce(impulse, ForceMode2D.Impulse);

            if (showDebug)
                Debug.Log($"[AddImpulse] Impulse: {impulse}, Velocity: {rb.linearVelocity}");
        }

        private static void ApplyAddTorque(Rigidbody2D rb, float torque, bool showDebug)
        {
            if (torque == 0f) return;

            rb.AddTorque(torque, ForceMode2D.Force);

            if (showDebug)
                Debug.Log($"[AddTorque] Torque: {torque}, AngularVelocity: {rb.angularVelocity}");
        }

        private static void ApplyAddAngularImpulse(Rigidbody2D rb, float torque, bool showDebug)
        {
            if (torque == 0f) return;

            rb.AddTorque(torque, ForceMode2D.Impulse);

            if (showDebug)
                Debug.Log($"[AddAngularImpulse] Torque: {torque}, AngularVelocity: {rb.angularVelocity}");
        }

        private static void ApplySetVelocity(Rigidbody2D rb, Vector2 velocity, bool showDebug)
        {
            rb.linearVelocity = velocity;

            if (showDebug)
                Debug.Log($"[SetVelocity] New Velocity: {velocity}");
        }


        private static void ApplyMovePosition(Rigidbody2D rb, Vector2 velocity, bool showDebug)
        {
            Vector2 newPosition = rb.position + velocity * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);

            if (showDebug)
                Debug.Log($"[MovePosition] New Position: {newPosition}");
        }

        private static void ApplyAddRelativeForce(Rigidbody2D rb, Vector2 localForce, bool showDebug)
        {
            if (localForce == Vector2.zero) return;

            rb.AddRelativeForce(localForce, ForceMode2D.Force);

            if (showDebug)
                Debug.Log($"[AddRelativeForce] Local Force: {localForce}, Velocity: {rb.linearVelocity}");
        }

        private static void ApplyCustomGravity(Rigidbody2D rb, Vector2 gravityDirection, bool showDebug)
        {
            if (gravityDirection == Vector2.zero) return;

            float gravityScale = 9.81f;
            rb.AddForce(gravityDirection.normalized * gravityScale, ForceMode2D.Force);

            if (showDebug)
                Debug.Log($"[CustomGravity] Direction: {gravityDirection.normalized}, Force: {gravityScale}");
        }
    }

}
    

