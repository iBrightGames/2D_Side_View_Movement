using UnityEngine;

#region Movement Executor

public static class MovementExecutor
{
    public static void ApplyForce(MovementForceSO force, Vector3 inputDirection, Transform t = null, Rigidbody2D rb = null)
    {
        Vector3 finalDirection = force.UseInputDirection && inputDirection.sqrMagnitude > 0.001f 
            ? inputDirection.normalized * force.Direction.magnitude 
            : force.Direction;

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
                Debug.LogError($"Unknown MovementForce type: {force.GetType().Name}");
                break;
        }
    }

    private static void ApplyTransform(ForceType forceType, Vector3 direction, Transform t)
    {
        if (t == null) return;

        switch (forceType)
        {
            case ForceType.Linear:
                t.Translate(direction * Time.deltaTime, Space.World);
                break;
            case ForceType.Angular:
                t.Rotate(direction * Time.deltaTime, Space.Self);
                break;
            case ForceType.Scaler:
                t.localScale += direction * Time.deltaTime;
                break;
        }
    }

    private static void ApplyRigidbody2D(ForceType forceType, Vector3 direction, Rigidbody2D rb, ForceMode2D forceMode)
    {
        if (rb == null) return;

        switch (forceType)
        {
            case ForceType.Linear:
                rb.AddForce(direction, forceMode);
                break;
            case ForceType.Angular:
                rb.AddTorque(direction.z, forceMode);
                break;
            case ForceType.Scaler:
                rb.transform.localScale += direction * Time.deltaTime;
                break;
        }
    }
}

#endregion
