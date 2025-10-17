// ============================================
// FORCE CONFIGS
// ============================================

using UnityEngine;

public enum ForceType
{
    Linear,
    Angular,
    Velocity
}

public abstract class ForceConfig : ScriptableObject
{
    [Header("Force Settings")]
    public ForceType forceType;
    public ForceMode2D forceMode2D = ForceMode2D.Force;
    public float forceMagnitude = 10f;
    
    public abstract Vector2 GetLinearDirection(Vector2? inputDirection, Vector2 currentVelocity);
    public abstract float GetAngularMagnitude();
}

[CreateAssetMenu(fileName = "LinearForceConfig", menuName = "Forces/LinearForceConfig")]
public class LinearForceConfig : ForceConfig
{
    [Header("Direction Settings")]
    public bool useInputDirection = true;
    public Vector2 fixedDirection = Vector2.right;
    
    [Header("Velocity Settings")]
    [Tooltip("Max velocity for this movement")]
    public float maxVelocity = 0f; // 0 = unlimited
    
    [Tooltip("Reset velocity before applying force")]
    public bool resetVelocityBeforeApply = false;
    public bool resetOnlyY = false;
    
    public override Vector2 GetLinearDirection(Vector2? inputDirection, Vector2 currentVelocity)
    {
        Vector2 direction;
        
        // Direction belirleme
        if (useInputDirection && inputDirection.HasValue && inputDirection.Value != Vector2.zero)
        {
            direction = inputDirection.Value.normalized;
        }
        else
        {
            direction = fixedDirection.normalized;
        }
        
        // Max velocity kontrolü
        if (maxVelocity > 0f)
        {
            float currentSpeed = currentVelocity.magnitude;
            if (currentSpeed >= maxVelocity)
            {
                return Vector2.zero; // Max hıza ulaşıldı
            }
        }
        
        return direction * forceMagnitude;
    }
    
    public override float GetAngularMagnitude()
    {
        return 0f; // Linear force angular kullanmaz
    }
}

[CreateAssetMenu(fileName = "AngularForceConfig", menuName = "Forces/AngularForceConfig")]
public class AngularForceConfig : ForceConfig
{
    [Header("Rotation Settings")]
    [Tooltip("Positive = counter-clockwise, Negative = clockwise")]
    public float rotationDirection = 1f;
    
    [Tooltip("Use input X axis for rotation direction")]
    public bool useInputForDirection = false;
    
    public override Vector2 GetLinearDirection(Vector2? inputDirection, Vector2 currentVelocity)
    {
        return Vector2.zero; // Angular force linear kullanmaz
    }
    
    public override float GetAngularMagnitude()
    {
        return forceMagnitude * rotationDirection;
    }
}

[CreateAssetMenu(fileName = "VelocityForceConfig", menuName = "Forces/VelocityForceConfig")]
public class VelocityForceConfig : ForceConfig
{
    [Header("Velocity Settings")]
    public bool useInputDirection = true;
    public Vector2 fixedDirection = Vector2.right;
    
    [Tooltip("Preserve Y velocity (useful for ground movement)")]
    public bool preserveVerticalVelocity = true;
    
    [Tooltip("Lerp to target velocity instead of instant")]
    public bool smoothTransition = false;
    public float transitionSpeed = 10f;
    
    public override Vector2 GetLinearDirection(Vector2? inputDirection, Vector2 currentVelocity)
    {
        Vector2 direction;
        
        if (useInputDirection && inputDirection.HasValue)
        {
            direction = inputDirection.Value.normalized;
        }
        else
        {
            direction = fixedDirection.normalized;
        }
        
        Vector2 targetVelocity = direction * forceMagnitude;
        
        if (preserveVerticalVelocity)
        {
            targetVelocity.y = currentVelocity.y;
        }
        
        if (smoothTransition)
        {
            return Vector2.Lerp(currentVelocity, targetVelocity, transitionSpeed * Time.fixedDeltaTime);
        }
        
        return targetVelocity;
    }
    
    public override float GetAngularMagnitude()
    {
        return 0f;
    }
}

