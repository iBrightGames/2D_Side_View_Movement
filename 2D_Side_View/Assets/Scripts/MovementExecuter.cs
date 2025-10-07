using UnityEngine;

public static class MovementExecutor
{
    public static void ApplyForce(MovementForceSO force, Transform t = null, Rigidbody2D rb = null)
    {
        switch (force)
        {
            case TransformMovementForce tf:
                if (t == null)
                {
                    Debug.LogError("TransformMovementForce requires a Transform!");
                    return;
                }
                ApplyToTransform(tf, t);
                break;

            case RigidbodyMovementForce rf:
                if (rb == null)
                {
                    Debug.LogError("RigidbodyMovementForce requires a Rigidbody2D!");
                    return;
                }
                ApplyToRigidbody2D(rf, rb);
                break;

            default:
                Debug.LogError("Unknown MovementForce type!");
                break;
        }
    }

    private static void ApplyToTransform(TransformMovementForce f, Transform t)
    {
        switch (f.ForceType)
        {
            case ForceType.Linear: t.Translate(f.Direction * Time.deltaTime); break;
            case ForceType.Angular: t.Rotate(f.Direction * Time.deltaTime); break;
            case ForceType.Scaler: t.localScale += f.Direction * Time.deltaTime; break;
        }
    }

    private static void ApplyToRigidbody2D(RigidbodyMovementForce f, Rigidbody2D rb)
    {
        switch (f.ForceType)
        {
            case ForceType.Linear:

                rb.AddForce(f.Direction, f.ForceMode2D);
                break;
            case ForceType.Angular:
                rb.AddTorque(f.Direction.z, f.ForceMode2D);
                break;
            case ForceType.Scaler:
                rb.transform.localScale += f.Direction * Time.deltaTime;
                break;


        }
    }
}

public enum ForceType { Linear, Angular, Scaler }
