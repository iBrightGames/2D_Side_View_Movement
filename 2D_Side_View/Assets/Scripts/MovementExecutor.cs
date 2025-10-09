using UnityEngine;


#region Movement Executor

public static class MovementExecutor
{
    public static void ApplyForce(MovementForceSO force, Vector3 inputDirection,
                                   MovementModifier[] modifiers, Transform t=null, Rigidbody2D rb=null)
    {
        Vector3 finalDirection = force.UseInputDirection && inputDirection.sqrMagnitude > 0.001f
            ? inputDirection.normalized * force.Direction.magnitude
            : force.Direction;

        // Apply modifiers
        if (modifiers != null)
        {
            foreach (var mod in modifiers)
            {
                if (mod != null)
                    finalDirection = mod.ApplyToDirection(finalDirection);
            }
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
