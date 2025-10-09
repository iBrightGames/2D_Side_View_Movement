using UnityEngine;

#region Movement Conditions

public abstract class MovementCondition : ScriptableObject
{
    [Header("Condition Info")]
    public string conditionName = "Unnamed Condition";
    
    [TextArea(2, 4)]
    public string description = "";
    
    [Header("Debug")]
    public bool showGizmos = true;
    public Color gizmoColor = Color.yellow;
    
    public abstract bool CanExecute(MovementController controller);
    public abstract string GetFailureReason();
    
    public virtual void DrawGizmos(MovementController controller) { }
}

[CreateAssetMenu(fileName = "GroundedCondition", menuName = "Movement/Conditions/Grounded Check")]
public class GroundedCondition : MovementCondition
{
    [Header("Detection Settings")]
    public LayerMask groundLayers = 1;
    public float checkDistance = 0.15f;
    public Vector2 boxSize = new Vector2(0.8f, 0.1f);
    public Vector2 offset = new Vector2(0f, -0.5f);
    
    public override bool CanExecute(MovementController controller)
    {
        Vector2 checkPos = (Vector2)controller.transform.position + offset;
        return Physics2D.OverlapBox(checkPos, boxSize, 0f, groundLayers);
    }
    
    public override string GetFailureReason() => "Not grounded";
    
    public override void DrawGizmos(MovementController controller)
    {
        if (!showGizmos) return;
        
        Gizmos.color = gizmoColor;
        Vector2 checkPos = (Vector2)controller.transform.position + offset;
        Gizmos.DrawWireCube(checkPos, boxSize);
    }
}

[CreateAssetMenu(fileName = "CooldownCondition", menuName = "Movement/Conditions/Cooldown Timer")]
public class CooldownCondition : MovementCondition
{
    [Header("Cooldown Settings")]
    public float cooldownDuration = 1f;
    public bool resetOnSceneLoad = true;
    
    [Header("Runtime State")]
    [SerializeField] private float lastExecutionTime = -999f;
    
    public override bool CanExecute(MovementController controller)
    {
        return Time.time - lastExecutionTime >= cooldownDuration;
    }
    
    public override string GetFailureReason()
    {
        float remaining = GetRemainingCooldown();
        return $"On cooldown ({remaining:F1}s)";
    }
    
    public void MarkExecuted()
    {
        lastExecutionTime = Time.time;
    }
    
    public float GetRemainingCooldown()
    {
        return Mathf.Max(0f, cooldownDuration - (Time.time - lastExecutionTime));
    }
    
    public float GetCooldownPercent()
    {
        return Mathf.Clamp01((Time.time - lastExecutionTime) / cooldownDuration);
    }
    
    private void OnEnable()
    {
        if (resetOnSceneLoad)
            lastExecutionTime = -999f;
    }
}

[CreateAssetMenu(fileName = "VelocityCondition", menuName = "Movement/Conditions/Velocity Check")]
public class VelocityCondition : MovementCondition
{
    public enum ComparisonType { LessThan, GreaterThan, Between }
    
    [Header("Velocity Settings")]
    public ComparisonType comparison = ComparisonType.GreaterThan;
    public float thresholdMin = 0.5f;
    public float thresholdMax = 10f;
    public bool checkHorizontalOnly = false;
    
    public override bool CanExecute(MovementController controller)
    {
        Vector2 velocity = controller.GetVelocity();
        
        if (checkHorizontalOnly)
            velocity.y = 0;
        
        float speed = velocity.magnitude;
        
        switch (comparison)
        {
            case ComparisonType.LessThan:
                return speed < thresholdMin;
            case ComparisonType.GreaterThan:
                return speed > thresholdMin;
            case ComparisonType.Between:
                return speed >= thresholdMin && speed <= thresholdMax;
            default:
                return false;
        }
    }
    
    public override string GetFailureReason()
    {
        return $"Velocity {comparison} requirement not met";
    }
}

[CreateAssetMenu(fileName = "ComboCondition", menuName = "Movement/Conditions/Combo Chain")]
public class ComboCondition : MovementCondition
{
    [Header("Combo Settings")]
    public MovementForceSO requiredPreviousMovement;
    public float timeWindow = 1f;
    
    public override bool CanExecute(MovementController controller)
    {
        if (requiredPreviousMovement == null) return true;
        
        float timeSince = controller.GetTimeSinceMovement(requiredPreviousMovement);
        return timeSince >= 0 && timeSince <= timeWindow;
    }
    
    public override string GetFailureReason()
    {
        return $"Must use {requiredPreviousMovement?.name ?? "required move"} first";
    }
}

#endregion
