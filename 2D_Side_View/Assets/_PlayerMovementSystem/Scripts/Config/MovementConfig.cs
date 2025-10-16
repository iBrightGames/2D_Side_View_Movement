using UnityEngine;

[CreateAssetMenu(fileName = "MovementConfig", menuName = "Scriptable Objects/MovementConfig")]
public class MovementConfig : ScriptableObject
{
    [Header("Force Settings")]
    public ForceType forceType;
    public ForceMode2D forceMode2D;
    public float forceMagnitude;

    [Header("Direction Settings")]
    public DirectionSource directionSource;
    public Vector2 manualDirection = Vector2.up;
    public bool normalizeVelocity = true;

    [Header("Speed Limits")]
    public bool limitMaxSpeed = false;
    public float maxLinearSpeed = 10f;
    public bool limitMaxAngularSpeed = false;
    public float maxAngularSpeed = 360f;

    public Vector2 GetLinearDirection(Vector2? inputDirection, Vector2 currentVelocity)
    {
        Vector2 direction = directionSource switch
        {
            DirectionSource.FromInput => inputDirection ?? Vector2.zero,
            DirectionSource.Manual => manualDirection,
            DirectionSource.CurrentVelocity => normalizeVelocity ? currentVelocity.normalized : currentVelocity,
            _ => Vector2.zero
        };

        return direction.normalized * forceMagnitude;
    }

    public float GetAngularMagnitude()
    {
        return forceMagnitude;
    }
}
// using UnityEngine;


// [CreateAssetMenu(fileName = "MovementConfig", menuName = "Scriptable Objects/MovementConfig")]
// public class MovementConfig : ScriptableObject
// {
//     public ForceType forceType;
//     public ForceMode2D forceMode2D;
//     public float forceMagnitute;
// }
