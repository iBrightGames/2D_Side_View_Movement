using UnityEngine;
using UnityEngine.InputSystem;

#region  Bridge


[CreateAssetMenu(fileName = "NewInputBridge", menuName = "Movement/Input/InputBridge")]
public class InputMovementBridgeSO : ScriptableObject
{
    [SerializeField, SerializeReference]
    public Movement movement;
    [SerializeField, SerializeReference]
    public BaseInput baseInput;
}

#endregion

#region Input System

[System.Serializable]
public abstract class BaseInput
{
    public abstract bool ShouldExecute();
    public abstract bool IsHeld();
}
[System.Serializable]
public class ExternalInput : BaseInput
{
    public InputActionReference inputAction;

    public override bool ShouldExecute()
    {
        if (inputAction == null || inputAction.action == null) return false;
        
        var action = inputAction.action;
        
        switch (action.type)
        {
            case InputActionType.Button:
                return action.triggered;
            
            case InputActionType.Value:
                if (action.expectedControlType == "Vector2")
                {
                    return action.ReadValue<Vector2>().sqrMagnitude > 0.001f;
                }
                return Mathf.Abs(action.ReadValue<float>()) > 0.001f;
            
            default:
                return false;
        }
    }

    public override bool IsHeld()
    {
        if (inputAction == null || inputAction.action == null) return false;
        
        var action = inputAction.action;
        return action.phase == InputActionPhase.Performed || action.phase == InputActionPhase.Started;
    }

    public Vector3 GetInputDirection()
    {
        if (inputAction == null || inputAction.action == null) return Vector3.zero;
        
        var action = inputAction.action;
        
        if (action.expectedControlType == "Vector2")
        {
            Vector2 input = action.ReadValue<Vector2>();
            return new Vector3(input.x, 0, input.y);
        }
        else
        {
            float input = action.ReadValue<float>();
            return new Vector3(input, 0, 0);
        }
    }
}
[System.Serializable]
public class InternalInput : BaseInput
{
    [Header("Runtime Control")]
    [Tooltip("Toggle this bool from scripts to control movement")]
    public bool IsActive = false;
    
    [Header("Trigger Control")]
    [Tooltip("Set this to true from code to trigger once, then automatically resets")]
    public bool TriggerOnce = false;

    public override bool ShouldExecute()
    {
        if (TriggerOnce)
        {
            TriggerOnce = false; // Auto-reset
            return true;
        }
        return IsActive;
    }

    public override bool IsHeld()
    {
        return IsActive;
    }

    // Helper method for external scripts
    public void Trigger()
    {
        TriggerOnce = true;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}

#endregion

#region Movement System

public enum ForceType { Linear, Angular, Scaler }
public enum CompletionType { None, Time, Rotation, Distance, Condition }
[System.Serializable]
public abstract class Movement
{
    [Header("Force Configuration")]
    [SerializeField] protected ForceType forceType;
    [SerializeField] protected Vector3 direction;
    
    [Header("Input Control")]
    [Tooltip("If true, uses input direction instead of configured direction")]
    [SerializeField] protected bool useInputDirection = false;

    public ForceType ForceType{get => forceType;set => forceType = value;}

    public Vector3 Direction{ get => direction; set => direction = value; }

    public bool UseInputDirection
    {
        get => useInputDirection;
        set => useInputDirection = value;
    }

    public abstract MovementExecutionType ExecutionType { get; }
}

public enum MovementExecutionType
{
    Continuous,  // Executes while input is held
    Triggered,   // Executes once to completion
    Charged      // Hold to charge, release to execute
}

// ============= RIGIDBODY MOVEMENTS =============
[System.Serializable]
public class ContinuousRigidbodyMovement : Movement
{
    [Header("Rigidbody Settings")]
    [SerializeField] private ForceMode2D forceMode2D = ForceMode2D.Force;
    
    [Header("Duration (Optional)")]
    [Tooltip("Max duration in seconds. 0 = unlimited")]
    [Range(0, 10)][SerializeField] private float maxDuration = 0f;

    public ForceMode2D ForceMode2D{get =>forceMode2D;set => forceMode2D = value;}
    public float MaxDuration{get => maxDuration;set => maxDuration = value;}    
    public override MovementExecutionType ExecutionType => MovementExecutionType.Continuous;
}
[System.Serializable]
public class TriggeredRigidbodyMovement : Movement
{
    [Header("Rigidbody Settings")]
    [SerializeField] private ForceMode2D forceMode2D = ForceMode2D.Impulse;
    
    [Header("Completion Settings")]
    [SerializeField] private CompletionType completionType = CompletionType.Time;
    
    [Tooltip("Time in seconds, Degrees for rotation, or Distance in units")]
    [SerializeField] private float completionValue = 1f;
    
    [Tooltip("Can only trigger again after this completes")]
    [SerializeField] private bool blockUntilComplete = true;

    public ForceMode2D ForceMode2D
    {
        get => forceMode2D;
        set => forceMode2D = value;
    }
    public CompletionType CompletionType {get=> completionType;set=> completionType=value;}
    public float CompletionValue{get => completionValue;set => completionValue=value;}
    public bool BlockUntilComplete {get=> blockUntilComplete;set=> blockUntilComplete=value;}
    public override MovementExecutionType ExecutionType => MovementExecutionType.Triggered;
}
[System.Serializable]
public class ChargedRigidbodyMovement : Movement
{
    [Header("Rigidbody Settings")]
    [SerializeField] private ForceMode2D forceMode2D = ForceMode2D.Impulse;
    
    [Header("Charge Settings")]
    [SerializeField] private float minChargeTime = 0.2f;
    [SerializeField] private float maxChargeTime = 2f;
    [SerializeField] private float chargeMultiplier = 2f;

    public ForceMode2D ForceMode2D {get=> forceMode2D;set=> forceMode2D=value;}
    public float MinChargeTime{get => minChargeTime;set => minChargeTime=value;}
    public float MaxChargeTime { get => maxChargeTime; set => maxChargeTime = value; }    
    public float ChargeMultiplier {get=> chargeMultiplier;set=> chargeMultiplier=value;}
    public override MovementExecutionType ExecutionType => MovementExecutionType.Charged;
}

// ============= TRANSFORM MOVEMENTS =============
[System.Serializable]
public class ContinuousTransformMovement : Movement
{
    [Header("Duration (Optional)")]
    [Tooltip("Max duration in seconds. 0 = unlimited")]
    [Range(0, 10)][SerializeField] private float maxDuration = 0f;

    public float MaxDuration {get=> maxDuration;set=> maxDuration=value;}
    public override MovementExecutionType ExecutionType => MovementExecutionType.Continuous;
}
[System.Serializable]
public class TriggeredTransformMovement : Movement
{
    [Header("Completion Settings")]
    [SerializeField] private CompletionType completionType = CompletionType.Time;
    
    [Tooltip("Time in seconds, Degrees for rotation, or Distance in units")]
    [SerializeField] private float completionValue = 1f;
    
    [Tooltip("Can only trigger again after this completes")]
    [SerializeField] private bool blockUntilComplete = true;

    public CompletionType CompletionType{get => completionType;set => completionType=value;}
    public float CompletionValue {get=> completionValue;set=> completionValue=value;}
    public bool BlockUntilComplete {get=> blockUntilComplete;set=> blockUntilComplete=value;}
    public override MovementExecutionType ExecutionType => MovementExecutionType.Triggered;
}
[System.Serializable]
public class ChargedTransformMovement : Movement
{
    [Header("Charge Settings")]
    [SerializeField] private float minChargeTime = 0.2f;
    [SerializeField] private float maxChargeTime = 2f;
    [SerializeField] private float chargeMultiplier = 2f;

    public float MinChargeTime {get=> minChargeTime;set=> minChargeTime=value;}
    public float MaxChargeTime {get=> maxChargeTime;set=> maxChargeTime=value;}
    public float ChargeMultiplier {get=> chargeMultiplier;set=> chargeMultiplier=value;}
    public override MovementExecutionType ExecutionType => MovementExecutionType.Charged;
}

#endregion


