using UnityEngine;

public static class MovementExecuter
{
    public static void ApplyLinearForce(Rigidbody2D rb, Vector2 force, ForceMode2D forceMode, MovementConfig config)
    {
        rb.AddForce(force, forceMode);

        if (config.limitMaxSpeed && rb.linearVelocity.magnitude > config.maxLinearSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * config.maxLinearSpeed;
        }
    }

    public static void ApplyAngularForce(Rigidbody2D rb, float magnitude, MovementConfig config)
    {
        rb.AddTorque(magnitude);

        if (config.limitMaxAngularSpeed && Mathf.Abs(rb.angularVelocity) > config.maxAngularSpeed)
        {
            rb.angularVelocity = Mathf.Sign(rb.angularVelocity) * config.maxAngularSpeed;
        }
    }
}
// using UnityEngine;

// public static class MovementExecuter
// {
//     public static void ApplyLinearForce(Rigidbody2D rigidbody2D, Vector2 direction, ForceMode2D forceMode2D)
//     {
//         rigidbody2D.AddForce(direction, forceMode2D);
//     }
    
//     public static void ApplyAngularForce(Rigidbody2D rigidbody2D, float magnitude) 
//     {
//         rigidbody2D.AddTorque(magnitude);
//     }
// }