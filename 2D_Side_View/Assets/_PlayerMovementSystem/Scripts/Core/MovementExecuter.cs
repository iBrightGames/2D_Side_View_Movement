// ============================================
// MOVEMENT EXECUTOR
// ============================================

using UnityEngine;

public static class MovementExecuter
{
    public static void ExecuteMovement(Rigidbody2D rb, InputMovementBridge bridge, Vector2? inputDirection, bool showDebug = false)
    {
        var config = bridge.movementConfig;

        switch (config.forceType)
        {
            case ForceType.Linear:
                ApplyLinearForce(rb, config, inputDirection, showDebug);
                break;

            case ForceType.Angular:
                ApplyAngularForce(rb, config, inputDirection, showDebug);
                break;

            case ForceType.Velocity:
                ApplyVelocity(rb, config, inputDirection, showDebug);
                break;
        }

        bridge.MarkExecuted();
    }

    private static void ApplyLinearForce(Rigidbody2D rb, ForceConfig config, Vector2? inputDirection, bool showDebug)
    {
        Vector2 force = config.GetLinearDirection(inputDirection, rb.linearVelocity);

        if (showDebug)
        {
            Debug.Log($"[Linear Force] Direction: {force}, Magnitude: {force.magnitude}, Mode: {config.forceMode2D}");
            Debug.Log($"[Before] Velocity: {rb.linearVelocity}");
        }

        if (force != Vector2.zero)
        {
            rb.AddForce(force, config.forceMode2D);
        }

        if (showDebug)
        {
            Debug.Log($"[After] Velocity: {rb.linearVelocity}");
        }
    }

    public static void ApplyAngularForce(Rigidbody2D rb, ForceConfig config, Vector2? inputDirection, bool showDebug)
    {
        float torque = config.GetAngularMagnitude();

        if (showDebug)
        {
            Debug.Log($"[Angular Force] Torque: {torque}");
            Debug.Log($"[Before] AngularVelocity: {rb.angularVelocity}");
        }

        rb.AddTorque(torque, config.forceMode2D);

        if (showDebug)
        {
            Debug.Log($"[After] AngularVelocity: {rb.angularVelocity}");
        }
    }

    private static void ApplyVelocity(Rigidbody2D rb, ForceConfig config, Vector2? inputDirection, bool showDebug)
    {
        Vector2 targetVelocity = config.GetLinearDirection(inputDirection, rb.linearVelocity);

        if (showDebug)
        {
            Debug.Log($"[Velocity] Target: {targetVelocity}");
            Debug.Log($"[Before] Velocity: {rb.linearVelocity}");
        }

        rb.linearVelocity = targetVelocity;

        if (showDebug)
        {
            Debug.Log($"[After] Velocity: {rb.linearVelocity}");
        }
    }
}

