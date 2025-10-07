using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;

#region Input System

[System.Serializable]
public class InputMovementBridge
{
    public MovementForceSO movementSO;
    public BaseInputSO baseInputSO;
}

public abstract class BaseInputSO : ScriptableObject
{
    public abstract bool ShouldExecute();
    public abstract bool IsHeld();
}

[CreateAssetMenu(fileName = "NewExternalInputSO", menuName = "Movement/Input/ExternalInputSO")]
public class ExternalInputSO : BaseInputSO
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

[CreateAssetMenu(fileName = "NewInternalInputSO", menuName = "Movement/Input/InternalInputSO")]
public class InternalInputSO : BaseInputSO
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

#region Movement Force ScriptableObjects

public enum ForceType { Linear, Angular, Scaler }
public enum CompletionType { None, Time, Rotation, Distance, Condition }

public abstract class MovementForceSO : ScriptableObject
{
    [Header("Force Configuration")]
    [SerializeField] protected ForceType forceType;
    [SerializeField] protected Vector3 direction;
    
    [Header("Input Control")]
    [Tooltip("If true, uses input direction instead of configured direction")]
    [SerializeField] protected bool useInputDirection = false;

    public ForceType ForceType => forceType;
    public Vector3 Direction => direction;
    public bool UseInputDirection => useInputDirection;

    public abstract MovementExecutionType ExecutionType { get; }
}

public enum MovementExecutionType
{
    Continuous,  // Executes while input is held
    Triggered,   // Executes once to completion
    Charged      // Hold to charge, release to execute
}

// ============= RIGIDBODY MOVEMENTS =============

[CreateAssetMenu(fileName = "NewContinuousRigidbody", menuName = "Movement/Rigidbody/Continuous")]
public class ContinuousRigidbodyMovement : MovementForceSO
{
    [Header("Rigidbody Settings")]
    [SerializeField] private ForceMode2D forceMode2D = ForceMode2D.Force;
    
    [Header("Duration (Optional)")]
    [Tooltip("Max duration in seconds. 0 = unlimited")]
    [Range(0, 10)][SerializeField] private float maxDuration = 0f;

    public ForceMode2D ForceMode2D => forceMode2D;
    public float MaxDuration => maxDuration;
    public override MovementExecutionType ExecutionType => MovementExecutionType.Continuous;
}

[CreateAssetMenu(fileName = "NewTriggeredRigidbody", menuName = "Movement/Rigidbody/Triggered")]
public class TriggeredRigidbodyMovement : MovementForceSO
{
    [Header("Rigidbody Settings")]
    [SerializeField] private ForceMode2D forceMode2D = ForceMode2D.Impulse;
    
    [Header("Completion Settings")]
    [SerializeField] private CompletionType completionType = CompletionType.Time;
    
    [Tooltip("Time in seconds, Degrees for rotation, or Distance in units")]
    [SerializeField] private float completionValue = 1f;
    
    [Tooltip("Can only trigger again after this completes")]
    [SerializeField] private bool blockUntilComplete = true;

    public ForceMode2D ForceMode2D => forceMode2D;
    public CompletionType CompletionType => completionType;
    public float CompletionValue => completionValue;
    public bool BlockUntilComplete => blockUntilComplete;
    public override MovementExecutionType ExecutionType => MovementExecutionType.Triggered;
}

[CreateAssetMenu(fileName = "NewChargedRigidbody", menuName = "Movement/Rigidbody/Charged")]
public class ChargedRigidbodyMovement : MovementForceSO
{
    [Header("Rigidbody Settings")]
    [SerializeField] private ForceMode2D forceMode2D = ForceMode2D.Impulse;
    
    [Header("Charge Settings")]
    [SerializeField] private float minChargeTime = 0.2f;
    [SerializeField] private float maxChargeTime = 2f;
    [SerializeField] private float chargeMultiplier = 2f;

    public ForceMode2D ForceMode2D => forceMode2D;
    public float MinChargeTime => minChargeTime;
    public float MaxChargeTime => maxChargeTime;
    public float ChargeMultiplier => chargeMultiplier;
    public override MovementExecutionType ExecutionType => MovementExecutionType.Charged;
}

// ============= TRANSFORM MOVEMENTS =============

[CreateAssetMenu(fileName = "NewContinuousTransform", menuName = "Movement/Transform/Continuous")]
public class ContinuousTransformMovement : MovementForceSO
{
    [Header("Duration (Optional)")]
    [Tooltip("Max duration in seconds. 0 = unlimited")]
    [Range(0, 10)][SerializeField] private float maxDuration = 0f;

    public float MaxDuration => maxDuration;
    public override MovementExecutionType ExecutionType => MovementExecutionType.Continuous;
}

[CreateAssetMenu(fileName = "NewTriggeredTransform", menuName = "Movement/Transform/Triggered")]
public class TriggeredTransformMovement : MovementForceSO
{
    [Header("Completion Settings")]
    [SerializeField] private CompletionType completionType = CompletionType.Time;
    
    [Tooltip("Time in seconds, Degrees for rotation, or Distance in units")]
    [SerializeField] private float completionValue = 1f;
    
    [Tooltip("Can only trigger again after this completes")]
    [SerializeField] private bool blockUntilComplete = true;

    public CompletionType CompletionType => completionType;
    public float CompletionValue => completionValue;
    public bool BlockUntilComplete => blockUntilComplete;
    public override MovementExecutionType ExecutionType => MovementExecutionType.Triggered;
}

[CreateAssetMenu(fileName = "NewChargedTransform", menuName = "Movement/Transform/Charged")]
public class ChargedTransformMovement : MovementForceSO
{
    [Header("Charge Settings")]
    [SerializeField] private float minChargeTime = 0.2f;
    [SerializeField] private float maxChargeTime = 2f;
    [SerializeField] private float chargeMultiplier = 2f;

    public float MinChargeTime => minChargeTime;
    public float MaxChargeTime => maxChargeTime;
    public float ChargeMultiplier => chargeMultiplier;
    public override MovementExecutionType ExecutionType => MovementExecutionType.Charged;
}

#endregion


