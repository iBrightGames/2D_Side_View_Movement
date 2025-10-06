using UnityEngine;

#region Movement Executor

public static class MovementExecutor
{
    public static void ApplyForce(MovementForce force, Transform t = null, Rigidbody2D rb = null)
    {
        if (force.targetType == TargetType.Transform && t == null)
        {
            Debug.LogError("MovementExecutor: Transform required but not provided!");
            return;
        }
        
        if (force.targetType == TargetType.Rigidbody2D && rb == null)
        {
            Debug.LogError("MovementExecutor: Rigidbody2D required but not provided!");
            return;
        }

        switch (force.targetType)
        {
            case TargetType.Transform:
                ApplyToTransform(force, t);
                break;
            case TargetType.Rigidbody2D:
                ApplyToRigidbody2D(force, rb);
                break;
        }
    }

    private static void ApplyToTransform(MovementForce f, Transform t)
    {
        switch (f.forceType)
        {
            case ForceType.Linear:
                t.Translate(f.direction* Time.deltaTime);
                break;
            case ForceType.Angular:
                t.Rotate(f.direction * Time.deltaTime);
                break;
            case ForceType.Scaler:
                t.localScale += f.direction * Time.deltaTime;
                break;
        }
    }

    private static void ApplyToRigidbody2D(MovementForce f, Rigidbody2D rb)
    {
        switch (f.forceType)
        {
            case ForceType.Linear:
                if (f.isContinuous)
                    rb.AddForce(f.direction , ForceMode2D.Force);
                else
                    rb.AddForce(f.direction , ForceMode2D.Impulse);
                break;
            case ForceType.Angular:
                float torque = f.direction.z ;
                if (f.isContinuous)
                    rb.AddTorque(torque, ForceMode2D.Force);
                else
                    rb.AddTorque(torque, ForceMode2D.Impulse);
                break;
        }
    }
}

#endregion

