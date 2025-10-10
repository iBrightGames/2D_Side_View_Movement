using UnityEngine;

#region Movement Executor

public static class MovementExecutor
{
    public static void ApplyForce(Movement force, Vector3 inputDirection, Transform t = null, Rigidbody2D rb = null)
    {
        // Determine final direction: use input if specified and valid, otherwise use configured direction
        Vector3 finalDirection = force.UseInputDirection && inputDirection.sqrMagnitude > 0.001f 
            ? inputDirection.normalized * force.Direction.magnitude 
            : force.Direction;

        // Route to appropriate handler based on movement type
        switch (force)
        {
            case ContinuousRigidbodyMovement cRb:
                ApplyRigidbody2D(cRb.ForceType, finalDirection, rb, cRb.ForceMode2D);
                break;
            
            case TriggeredRigidbodyMovement tRb:
                ApplyRigidbody2D(tRb.ForceType, finalDirection, rb, tRb.ForceMode2D);
                break;
            
            case ChargedRigidbodyMovement chRb:
                ApplyRigidbody2D(chRb.ForceType, finalDirection, rb, chRb.ForceMode2D);
                break;
            
            case ContinuousTransformMovement cTr:
                ApplyTransform(cTr.ForceType, finalDirection, t);
                break;
            
            case TriggeredTransformMovement tTr:
                ApplyTransform(tTr.ForceType, finalDirection, t);
                break;
            
            case ChargedTransformMovement chTr:
                ApplyTransform(chTr.ForceType, finalDirection, t);
                break;
            
            default:
                Debug.LogError($"Unknown Movement type: {force.GetType().Name}");
                break;
        }
    }

    private static void ApplyTransform(ForceType forceType, Vector3 direction, Transform t)
    {
        if (t == null)
        {
            Debug.LogWarning("Transform is null, cannot apply movement");
            return;
        }

        switch (forceType)
        {
            case ForceType.Linear:
                // Move in world space
                t.Translate(direction * Time.deltaTime, Space.World);
                break;
            
            case ForceType.Angular:
                // Rotate around local axes
                t.Rotate(direction * Time.deltaTime, Space.Self);
                break;
            
            case ForceType.Scaler:
                // Scale relative to current scale
                t.localScale += direction * Time.deltaTime;
                break;
            
            default:
                Debug.LogError($"Unknown ForceType: {forceType}");
                break;
        }
    }

    private static void ApplyRigidbody2D(ForceType forceType, Vector3 direction, Rigidbody2D rb, ForceMode2D forceMode)
    {
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D is null, cannot apply force");
            return;
        }

        switch (forceType)
        {
            case ForceType.Linear:
                // Apply linear force (2D, ignoring Z component)
                rb.AddForce(new Vector2(direction.x, direction.y), forceMode);
                break;
            
            case ForceType.Angular:
                // Apply torque (rotation around Z-axis in 2D)
                rb.AddTorque(direction.z, forceMode);
                break;
            
            case ForceType.Scaler:
                // Scale is not physics-based, modify transform directly
                rb.transform.localScale += direction * Time.deltaTime;
                break;
            
            default:
                Debug.LogError($"Unknown ForceType: {forceType}");
                break;
        }
    }
}

#endregion


