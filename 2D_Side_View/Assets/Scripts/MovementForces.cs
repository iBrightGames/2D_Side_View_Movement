using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;



#region Movement Forces (Abbreviated - use your existing ones)

public enum ForceType { Linear, Angular, Scaler }
public enum CompletionType { None, Time, Rotation, Distance }
public enum MovementExecutionType { Continuous, Triggered, Charged }

public abstract class MovementForceSO : ScriptableObject
{
    [Header("Force Configuration")]
    [SerializeField] protected ForceType forceType;
    [SerializeField] protected Vector3 direction;
    
    [Header("Input Control")]
    [Tooltip("Use input direction instead of configured direction")]
    [SerializeField] protected bool useInputDirection = false;
    
    public ForceType ForceType => forceType;
    public Vector3 Direction => direction;
    public bool UseInputDirection => useInputDirection;
    
    public abstract MovementExecutionType ExecutionType { get; }
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

