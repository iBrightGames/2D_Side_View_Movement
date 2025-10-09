using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;

// #region Input System

// [System.Serializable]
// public class InputMovementBridge
// {
//     public MovementForceSO movementSO;
//     public BaseInputSO baseInputSO;
// }

// public abstract class BaseInputSO : ScriptableObject
// {
//     public abstract bool ShouldExecute();
//     public abstract bool IsHeld();
// }

// [CreateAssetMenu(fileName = "NewExternalInputSO", menuName = "Movement/Input/ExternalInputSO")]
// public class ExternalInputSO : BaseInputSO
// {
//     public InputActionReference inputAction;

//     public override bool ShouldExecute()
//     {
//         if (inputAction == null || inputAction.action == null) return false;
        
//         var action = inputAction.action;
        
//         switch (action.type)
//         {
//             case InputActionType.Button:
//                 return action.triggered;
            
//             case InputActionType.Value:
//                 if (action.expectedControlType == "Vector2")
//                 {
//                     return action.ReadValue<Vector2>().sqrMagnitude > 0.001f;
//                 }
//                 return Mathf.Abs(action.ReadValue<float>()) > 0.001f;
            
//             default:
//                 return false;
//         }
//     }

//     public override bool IsHeld()
//     {
//         if (inputAction == null || inputAction.action == null) return false;
        
//         var action = inputAction.action;
//         return action.phase == InputActionPhase.Performed || action.phase == InputActionPhase.Started;
//     }

//     public Vector3 GetInputDirection()
//     {
//         if (inputAction == null || inputAction.action == null) return Vector3.zero;
        
//         var action = inputAction.action;
        
//         if (action.expectedControlType == "Vector2")
//         {
//             Vector2 input = action.ReadValue<Vector2>();
//             return new Vector3(input.x, 0, input.y);
//         }
//         else
//         {
//             float input = action.ReadValue<float>();
//             return new Vector3(input, 0, 0);
//         }
//     }
// }

// [CreateAssetMenu(fileName = "NewInternalInputSO", menuName = "Movement/Input/InternalInputSO")]
// public class InternalInputSO : BaseInputSO
// {
//     [Header("Runtime Control")]
//     [Tooltip("Toggle this bool from scripts to control movement")]
//     public bool IsActive = false;
    
//     [Header("Trigger Control")]
//     [Tooltip("Set this to true from code to trigger once, then automatically resets")]
//     public bool TriggerOnce = false;

//     public override bool ShouldExecute()
//     {
//         if (TriggerOnce)
//         {
//             TriggerOnce = false; // Auto-reset
//             return true;
//         }
//         return IsActive;
//     }

//     public override bool IsHeld()
//     {
//         return IsActive;
//     }

//     // Helper method for external scripts
//     public void Trigger()
//     {
//         TriggerOnce = true;
//     }

//     public void Activate()
//     {
//         IsActive = true;
//     }

//     public void Deactivate()
//     {
//         IsActive = false;
//     }
// }

// #endregion

// #region Movement Force ScriptableObjects

// public enum ForceType { Linear, Angular, Scaler }
// public enum CompletionType { None, Time, Rotation, Distance, Condition }

// public abstract class MovementForceSO : ScriptableObject
// {
//     [Header("Force Configuration")]
//     [SerializeField] protected ForceType forceType;
//     [SerializeField] protected Vector3 direction;
    
//     [Header("Input Control")]
//     [Tooltip("If true, uses input direction instead of configured direction")]
//     [SerializeField] protected bool useInputDirection = false;

//     public ForceType ForceType => forceType;
//     public Vector3 Direction => direction;
//     public bool UseInputDirection => useInputDirection;

//     public abstract MovementExecutionType ExecutionType { get; }
// }

// public enum MovementExecutionType
// {
//     Continuous,  // Executes while input is held
//     Triggered,   // Executes once to completion
//     Charged      // Hold to charge, release to execute
// }

// // ============= RIGIDBODY MOVEMENTS =============

// [CreateAssetMenu(fileName = "NewContinuousRigidbody", menuName = "Movement/Rigidbody/Continuous")]
// public class ContinuousRigidbodyMovement : MovementForceSO
// {
//     [Header("Rigidbody Settings")]
//     [SerializeField] private ForceMode2D forceMode2D = ForceMode2D.Force;
    
//     [Header("Duration (Optional)")]
//     [Tooltip("Max duration in seconds. 0 = unlimited")]
//     [Range(0, 10)][SerializeField] private float maxDuration = 0f;

//     public ForceMode2D ForceMode2D => forceMode2D;
//     public float MaxDuration => maxDuration;
//     public override MovementExecutionType ExecutionType => MovementExecutionType.Continuous;
// }

// [CreateAssetMenu(fileName = "NewTriggeredRigidbody", menuName = "Movement/Rigidbody/Triggered")]
// public class TriggeredRigidbodyMovement : MovementForceSO
// {
//     [Header("Rigidbody Settings")]
//     [SerializeField] private ForceMode2D forceMode2D = ForceMode2D.Impulse;
    
//     [Header("Completion Settings")]
//     [SerializeField] private CompletionType completionType = CompletionType.Time;
    
//     [Tooltip("Time in seconds, Degrees for rotation, or Distance in units")]
//     [SerializeField] private float completionValue = 1f;
    
//     [Tooltip("Can only trigger again after this completes")]
//     [SerializeField] private bool blockUntilComplete = true;

//     public ForceMode2D ForceMode2D => forceMode2D;
//     public CompletionType CompletionType => completionType;
//     public float CompletionValue => completionValue;
//     public bool BlockUntilComplete => blockUntilComplete;
//     public override MovementExecutionType ExecutionType => MovementExecutionType.Triggered;
// }

// [CreateAssetMenu(fileName = "NewChargedRigidbody", menuName = "Movement/Rigidbody/Charged")]
// public class ChargedRigidbodyMovement : MovementForceSO
// {
//     [Header("Rigidbody Settings")]
//     [SerializeField] private ForceMode2D forceMode2D = ForceMode2D.Impulse;
    
//     [Header("Charge Settings")]
//     [SerializeField] private float minChargeTime = 0.2f;
//     [SerializeField] private float maxChargeTime = 2f;
//     [SerializeField] private float chargeMultiplier = 2f;

//     public ForceMode2D ForceMode2D => forceMode2D;
//     public float MinChargeTime => minChargeTime;
//     public float MaxChargeTime => maxChargeTime;
//     public float ChargeMultiplier => chargeMultiplier;
//     public override MovementExecutionType ExecutionType => MovementExecutionType.Charged;
// }

// // ============= TRANSFORM MOVEMENTS =============

// [CreateAssetMenu(fileName = "NewContinuousTransform", menuName = "Movement/Transform/Continuous")]
// public class ContinuousTransformMovement : MovementForceSO
// {
//     [Header("Duration (Optional)")]
//     [Tooltip("Max duration in seconds. 0 = unlimited")]
//     [Range(0, 10)][SerializeField] private float maxDuration = 0f;

//     public float MaxDuration => maxDuration;
//     public override MovementExecutionType ExecutionType => MovementExecutionType.Continuous;
// }

// [CreateAssetMenu(fileName = "NewTriggeredTransform", menuName = "Movement/Transform/Triggered")]
// public class TriggeredTransformMovement : MovementForceSO
// {
//     [Header("Completion Settings")]
//     [SerializeField] private CompletionType completionType = CompletionType.Time;
    
//     [Tooltip("Time in seconds, Degrees for rotation, or Distance in units")]
//     [SerializeField] private float completionValue = 1f;
    
//     [Tooltip("Can only trigger again after this completes")]
//     [SerializeField] private bool blockUntilComplete = true;

//     public CompletionType CompletionType => completionType;
//     public float CompletionValue => completionValue;
//     public bool BlockUntilComplete => blockUntilComplete;
//     public override MovementExecutionType ExecutionType => MovementExecutionType.Triggered;
// }

// [CreateAssetMenu(fileName = "NewChargedTransform", menuName = "Movement/Transform/Charged")]
// public class ChargedTransformMovement : MovementForceSO
// {
//     [Header("Charge Settings")]
//     [SerializeField] private float minChargeTime = 0.2f;
//     [SerializeField] private float maxChargeTime = 2f;
//     [SerializeField] private float chargeMultiplier = 2f;

//     public float MinChargeTime => minChargeTime;
//     public float MaxChargeTime => maxChargeTime;
//     public float ChargeMultiplier => chargeMultiplier;
//     public override MovementExecutionType ExecutionType => MovementExecutionType.Charged;
// }

// #endregion

// #region Movement Executor

// public static class MovementExecutor
// {
//     public static void ApplyForce(MovementForceSO force, Vector3 inputDirection, Transform t = null, Rigidbody2D rb = null)
//     {
//         Vector3 finalDirection = force.UseInputDirection && inputDirection.sqrMagnitude > 0.001f 
//             ? inputDirection.normalized * force.Direction.magnitude 
//             : force.Direction;

//         switch (force)
//         {
//             case ContinuousRigidbodyMovement cRb:
//                 ApplyRigidbody2D(cRb.ForceType, finalDirection, rb, cRb.ForceMode2D);
//                 break;
            
//             case TriggeredRigidbodyMovement tRb:
//                 ApplyRigidbody2D(tRb.ForceType, finalDirection, rb, tRb.ForceMode2D);
//                 break;
            
//             case ChargedRigidbodyMovement chRb:
//                 ApplyRigidbody2D(chRb.ForceType, finalDirection, rb, chRb.ForceMode2D);
//                 break;
            
//             case ContinuousTransformMovement cTr:
//                 ApplyTransform(cTr.ForceType, finalDirection, t);
//                 break;
            
//             case TriggeredTransformMovement tTr:
//                 ApplyTransform(tTr.ForceType, finalDirection, t);
//                 break;
            
//             case ChargedTransformMovement chTr:
//                 ApplyTransform(chTr.ForceType, finalDirection, t);
//                 break;
            
//             default:
//                 Debug.LogError($"Unknown MovementForce type: {force.GetType().Name}");
//                 break;
//         }
//     }

//     private static void ApplyTransform(ForceType forceType, Vector3 direction, Transform t)
//     {
//         if (t == null) return;

//         switch (forceType)
//         {
//             case ForceType.Linear:
//                 t.Translate(direction * Time.deltaTime, Space.World);
//                 break;
//             case ForceType.Angular:
//                 t.Rotate(direction * Time.deltaTime, Space.Self);
//                 break;
//             case ForceType.Scaler:
//                 t.localScale += direction * Time.deltaTime;
//                 break;
//         }
//     }

//     private static void ApplyRigidbody2D(ForceType forceType, Vector3 direction, Rigidbody2D rb, ForceMode2D forceMode)
//     {
//         if (rb == null) return;

//         switch (forceType)
//         {
//             case ForceType.Linear:
//                 rb.AddForce(direction, forceMode);
//                 break;
//             case ForceType.Angular:
//                 rb.AddTorque(direction.z, forceMode);
//                 break;
//             case ForceType.Scaler:
//                 rb.transform.localScale += direction * Time.deltaTime;
//                 break;
//         }
//     }
// }

// #endregion

// #region Movement Controller

// public class MovementController : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private Rigidbody2D _rigidbody2D;
//     [SerializeField] private Transform _transform;

//     [Header("Events")]
//     public UnityEvent OnMovementStarted = new UnityEvent();
//     public UnityEvent OnMovementCompleted = new UnityEvent();

//     private void Awake()
//     {
//         if (_transform == null) _transform = transform;
//         if (_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
//     }

//     public void ApplyForce(MovementForceSO force, Vector3 inputDirection = default)
//     {
//         MovementExecutor.ApplyForce(force, inputDirection, _transform, _rigidbody2D);
//     }

//     public Coroutine StartTriggeredMovement(TriggeredRigidbodyMovement movement, Vector3 inputDirection)
//     {
//         return StartCoroutine(ExecuteTriggeredRigidbody(movement, inputDirection));
//     }

//     public Coroutine StartTriggeredMovement(TriggeredTransformMovement movement, Vector3 inputDirection)
//     {
//         return StartCoroutine(ExecuteTriggeredTransform(movement, inputDirection));
//     }

//     private IEnumerator ExecuteTriggeredRigidbody(TriggeredRigidbodyMovement movement, Vector3 inputDirection)
//     {
//         OnMovementStarted?.Invoke();
        
//         float elapsed = 0f;
//         Vector3 startPos = transform.position;
//         Quaternion startRot = transform.rotation;

//         switch (movement.CompletionType)
//         {
//             case CompletionType.Time:
//                 while (elapsed < movement.CompletionValue)
//                 {
//                     ApplyForce(movement, inputDirection);
//                     elapsed += Time.deltaTime;
//                     yield return null;
//                 }
//                 break;

//             case CompletionType.Rotation:
//                 float targetAngle = movement.CompletionValue;
//                 float rotated = 0f;
//                 Quaternion lastRot = transform.rotation;
                
//                 while (Mathf.Abs(rotated) < Mathf.Abs(targetAngle))
//                 {
//                     ApplyForce(movement, inputDirection);
//                     float deltaAngle = Quaternion.Angle(lastRot, transform.rotation);
//                     rotated += deltaAngle;
//                     lastRot = transform.rotation;
//                     yield return null;
//                 }
//                 break;

//             case CompletionType.Distance:
//                 float targetDist = movement.CompletionValue;
//                 float traveled = 0f;
                
//                 while (traveled < targetDist)
//                 {
//                     Vector3 lastPos = transform.position;
//                     ApplyForce(movement, inputDirection);
//                     traveled += Vector3.Distance(lastPos, transform.position);
//                     yield return null;
//                 }
//                 break;

//             default:
//                 ApplyForce(movement, inputDirection);
//                 break;
//         }

//         OnMovementCompleted?.Invoke();
//     }

//     private IEnumerator ExecuteTriggeredTransform(TriggeredTransformMovement movement, Vector3 inputDirection)
//     {
//         OnMovementStarted?.Invoke();
        
//         float elapsed = 0f;
//         Vector3 startPos = transform.position;
//         Quaternion startRot = transform.rotation;

//         switch (movement.CompletionType)
//         {
//             case CompletionType.Time:
//                 while (elapsed < movement.CompletionValue)
//                 {
//                     ApplyForce(movement, inputDirection);
//                     elapsed += Time.deltaTime;
//                     yield return null;
//                 }
//                 break;

//             case CompletionType.Rotation:
//                 float targetAngle = movement.CompletionValue;
//                 float rotated = 0f;
//                 Quaternion lastRot = transform.rotation;
                
//                 while (Mathf.Abs(rotated) < Mathf.Abs(targetAngle))
//                 {
//                     ApplyForce(movement, inputDirection);
//                     float deltaAngle = Quaternion.Angle(lastRot, transform.rotation);
//                     rotated += deltaAngle;
//                     lastRot = transform.rotation;
//                     yield return null;
//                 }
//                 break;

//             case CompletionType.Distance:
//                 float targetDist = movement.CompletionValue;
//                 while (Vector3.Distance(startPos, transform.position) < targetDist)
//                 {
//                     ApplyForce(movement, inputDirection);
//                     yield return null;
//                 }
//                 break;

//             default:
//                 ApplyForce(movement, inputDirection);
//                 break;
//         }

//         OnMovementCompleted?.Invoke();
//     }
// }

// #endregion

// #region Input Movement Handler

// public class InputMovementHandler : MonoBehaviour
// {
//     [SerializeField] private InputMovementBridge[] inputMovementBridges;
//     private MovementController movementController;

//     // Track active coroutines for triggered movements
//     private System.Collections.Generic.Dictionary<MovementForceSO, Coroutine> activeTriggeredMovements 
//         = new System.Collections.Generic.Dictionary<MovementForceSO, Coroutine>();

//     // Track continuous movements
//     private System.Collections.Generic.Dictionary<MovementForceSO, float> continuousMovementTimers 
//         = new System.Collections.Generic.Dictionary<MovementForceSO, float>();

//     // Track charged movements
//     private System.Collections.Generic.Dictionary<MovementForceSO, float> chargeTimers 
//         = new System.Collections.Generic.Dictionary<MovementForceSO, float>();

//     private void Awake()
//     {
//         movementController = GetComponent<MovementController>();
//     }

//     private void Update()
//     {
//         foreach (var bridge in inputMovementBridges)
//         {
//             if (bridge.baseInputSO == null || bridge.movementSO == null) continue;

//             ProcessBridge(bridge);
//         }
//     }

//     private void ProcessBridge(InputMovementBridge bridge)
//     {
//         BaseInputSO input = bridge.baseInputSO;
//         MovementForceSO movement = bridge.movementSO;

//         Vector3 inputDirection = Vector3.zero;
//         if (input is ExternalInputSO externalInput)
//         {
//             inputDirection = externalInput.GetInputDirection();
//         }

//         switch (movement.ExecutionType)
//         {
//             case MovementExecutionType.Continuous:
//                 HandleContinuousMovement(bridge, input, movement, inputDirection);
//                 break;

//             case MovementExecutionType.Triggered:
//                 HandleTriggeredMovement(bridge, input, movement, inputDirection);
//                 break;

//             case MovementExecutionType.Charged:
//                 HandleChargedMovement(bridge, input, movement, inputDirection);
//                 break;
//         }
//     }

//     private void HandleContinuousMovement(InputMovementBridge bridge, BaseInputSO input, 
//                                           MovementForceSO movement, Vector3 inputDirection)
//     {
//         bool isHeld = input.IsHeld();

//         if (isHeld)
//         {
//             // Check max duration for continuous movements
//             float maxDuration = 0f;
//             if (movement is ContinuousRigidbodyMovement cRb) maxDuration = cRb.MaxDuration;
//             else if (movement is ContinuousTransformMovement cTr) maxDuration = cTr.MaxDuration;

//             if (maxDuration > 0)
//             {
//                 if (!continuousMovementTimers.ContainsKey(movement))
//                 {
//                     continuousMovementTimers[movement] = 0f;
//                 }

//                 continuousMovementTimers[movement] += Time.deltaTime;

//                 if (continuousMovementTimers[movement] >= maxDuration)
//                 {
//                     return; // Max duration reached
//                 }
//             }

//             movementController.ApplyForce(movement, inputDirection);
//         }
//         else
//         {
//             // Reset timer when not held
//             if (continuousMovementTimers.ContainsKey(movement))
//             {
//                 continuousMovementTimers.Remove(movement);
//             }
//         }
//     }

//     private void HandleTriggeredMovement(InputMovementBridge bridge, BaseInputSO input, 
//                                          MovementForceSO movement, Vector3 inputDirection)
//     {
//         // Check if already executing
//         bool isBlocked = activeTriggeredMovements.ContainsKey(movement);
        
//         bool shouldBlock = false;
//         if (movement is TriggeredRigidbodyMovement tRb) shouldBlock = tRb.BlockUntilComplete;
//         else if (movement is TriggeredTransformMovement tTr) shouldBlock = tTr.BlockUntilComplete;

//         if (isBlocked && shouldBlock) return;

//         if (input.ShouldExecute())
//         {
//             Coroutine coroutine = null;

//             if (movement is TriggeredRigidbodyMovement trigRb)
//             {
//                 coroutine = movementController.StartTriggeredMovement(trigRb, inputDirection);
//             }
//             else if (movement is TriggeredTransformMovement trigTr)
//             {
//                 coroutine = movementController.StartTriggeredMovement(trigTr, inputDirection);
//             }

//             if (coroutine != null)
//             {
//                 activeTriggeredMovements[movement] = coroutine;
//                 StartCoroutine(CleanupTriggeredMovement(movement, coroutine));
//             }
//         }
//     }

//     private IEnumerator CleanupTriggeredMovement(MovementForceSO movement, Coroutine coroutine)
//     {
//         yield return coroutine;
//         activeTriggeredMovements.Remove(movement);
//     }

//     private void HandleChargedMovement(InputMovementBridge bridge, BaseInputSO input, 
//                                        MovementForceSO movement, Vector3 inputDirection)
//     {
//         bool isHeld = input.IsHeld();
//         bool wasHeld = chargeTimers.ContainsKey(movement);

//         if (isHeld)
//         {
//             // Charging
//             if (!chargeTimers.ContainsKey(movement))
//             {
//                 chargeTimers[movement] = 0f;
//             }

//             float maxCharge = 0f;
//             if (movement is ChargedRigidbodyMovement chRb) maxCharge = chRb.MaxChargeTime;
//             else if (movement is ChargedTransformMovement chTr) maxCharge = chTr.MaxChargeTime;

//             chargeTimers[movement] = Mathf.Min(chargeTimers[movement] + Time.deltaTime, maxCharge);
//         }
//         else if (wasHeld)
//         {
//             // Released - Execute charged movement
//             float chargeTime = chargeTimers[movement];
//             float minCharge = 0f, maxCharge = 1f, multiplier = 1f;

//             if (movement is ChargedRigidbodyMovement chRb)
//             {
//                 minCharge = chRb.MinChargeTime;
//                 maxCharge = chRb.MaxChargeTime;
//                 multiplier = chRb.ChargeMultiplier;
//             }
//             else if (movement is ChargedTransformMovement chTr)
//             {
//                 minCharge = chTr.MinChargeTime;
//                 maxCharge = chTr.MaxChargeTime;
//                 multiplier = chTr.ChargeMultiplier;
//             }

//             if (chargeTime >= minCharge)
//             {
//                 float chargePercent = Mathf.Clamp01((chargeTime - minCharge) / (maxCharge - minCharge));
//                 float forceMult = 1f + (chargePercent * multiplier);
                
//                 Vector3 chargedDirection = inputDirection * forceMult;
//                 if (chargedDirection.sqrMagnitude < 0.001f)
//                 {
//                     chargedDirection = movement.Direction * forceMult;
//                 }

//                 movementController.ApplyForce(movement, chargedDirection);
//             }

//             chargeTimers.Remove(movement);
//         }
//     }
// }

// #endregion

// #region Movement Controller

// // public class MovementController : MonoBehaviour
// // {
// //     [Header("References")]
// //     [SerializeField] private Rigidbody2D _rigidbody2D;
// //     [SerializeField] private Transform _transform;

// //     [Header("Events")]
// //     public UnityEvent OnMovementStarted = new UnityEvent();
// //     public UnityEvent OnMovementCompleted = new UnityEvent();

// //     private void Awake()
// //     {
// //         if (_transform == null) _transform = transform;
// //         if (_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
// //     }

// //     public void ApplyForce(MovementForceSO force, Vector3 inputDirection = default)
// //     {
// //         MovementExecutor.ApplyForce(force, inputDirection, _transform, _rigidbody2D);
// //     }

// //     public Coroutine StartTriggeredMovement(TriggeredRigidbodyMovement movement, Vector3 inputDirection)
// //     {
// //         return StartCoroutine(ExecuteTriggeredRigidbody(movement, inputDirection));
// //     }

// //     public Coroutine StartTriggeredMovement(TriggeredTransformMovement movement, Vector3 inputDirection)
// //     {
// //         return StartCoroutine(ExecuteTriggeredTransform(movement, inputDirection));
// //     }

// //     private IEnumerator ExecuteTriggeredRigidbody(TriggeredRigidbodyMovement movement, Vector3 inputDirection)
// //     {
// //         OnMovementStarted?.Invoke();
        
// //         float elapsed = 0f;
// //         Vector3 startPos = transform.position;
// //         Quaternion startRot = transform.rotation;

// //         switch (movement.CompletionType)
// //         {
// //             case CompletionType.Time:
// //                 while (elapsed < movement.CompletionValue)
// //                 {
// //                     ApplyForce(movement, inputDirection);
// //                     elapsed += Time.deltaTime;
// //                     yield return null;
// //                 }
// //                 break;

// //             case CompletionType.Rotation:
// //                 float targetAngle = movement.CompletionValue;
// //                 float rotated = 0f;
// //                 Quaternion lastRot = transform.rotation;
                
// //                 while (Mathf.Abs(rotated) < Mathf.Abs(targetAngle))
// //                 {
// //                     ApplyForce(movement, inputDirection);
// //                     float deltaAngle = Quaternion.Angle(lastRot, transform.rotation);
// //                     rotated += deltaAngle;
// //                     lastRot = transform.rotation;
// //                     yield return null;
// //                 }
// //                 break;

// //             case CompletionType.Distance:
// //                 float targetDist = movement.CompletionValue;
// //                 float traveled = 0f;
                
// //                 while (traveled < targetDist)
// //                 {
// //                     Vector3 lastPos = transform.position;
// //                     ApplyForce(movement, inputDirection);
// //                     traveled += Vector3.Distance(lastPos, transform.position);
// //                     yield return null;
// //                 }
// //                 break;

// //             default:
// //                 ApplyForce(movement, inputDirection);
// //                 break;
// //         }

// //         OnMovementCompleted?.Invoke();
// //     }

// //     private IEnumerator ExecuteTriggeredTransform(TriggeredTransformMovement movement, Vector3 inputDirection)
// //     {
// //         OnMovementStarted?.Invoke();
        
// //         float elapsed = 0f;
// //         Vector3 startPos = transform.position;
// //         Quaternion startRot = transform.rotation;

// //         switch (movement.CompletionType)
// //         {
// //             case CompletionType.Time:
// //                 while (elapsed < movement.CompletionValue)
// //                 {
// //                     ApplyForce(movement, inputDirection);
// //                     elapsed += Time.deltaTime;
// //                     yield return null;
// //                 }
// //                 break;

// //             case CompletionType.Rotation:
// //                 float targetAngle = movement.CompletionValue;
// //                 float rotated = 0f;
// //                 Quaternion lastRot = transform.rotation;
                
// //                 while (Mathf.Abs(rotated) < Mathf.Abs(targetAngle))
// //                 {
// //                     ApplyForce(movement, inputDirection);
// //                     float deltaAngle = Quaternion.Angle(lastRot, transform.rotation);
// //                     rotated += deltaAngle;
// //                     lastRot = transform.rotation;
// //                     yield return null;
// //                 }
// //                 break;

// //             case CompletionType.Distance:
// //                 float targetDist = movement.CompletionValue;
// //                 while (Vector3.Distance(startPos, transform.position) < targetDist)
// //                 {
// //                     ApplyForce(movement, inputDirection);
// //                     yield return null;
// //                 }
// //                 break;

// //             default:
// //                 ApplyForce(movement, inputDirection);
// //                 break;
// //         }

// //         OnMovementCompleted?.Invoke();
// //     }
// // }

// #endregion



// #region Input Movement Handler

// public class InputMovementHandler : MonoBehaviour
// {
//     [SerializeField] private InputMovementBridge[] inputMovementBridges;
//     private MovementController movementController;

//     // Track active coroutines for triggered movements
//     private System.Collections.Generic.Dictionary<MovementForceSO, Coroutine> activeTriggeredMovements 
//         = new System.Collections.Generic.Dictionary<MovementForceSO, Coroutine>();

//     // Track continuous movements
//     private System.Collections.Generic.Dictionary<MovementForceSO, float> continuousMovementTimers 
//         = new System.Collections.Generic.Dictionary<MovementForceSO, float>();

//     // Track charged movements
//     private System.Collections.Generic.Dictionary<MovementForceSO, float> chargeTimers 
//         = new System.Collections.Generic.Dictionary<MovementForceSO, float>();

//     private void Awake()
//     {
//         movementController = GetComponent<MovementController>();
//     }

//     private void Update()
//     {
//         foreach (var bridge in inputMovementBridges)
//         {
//             if (bridge.baseInputSO == null || bridge.movementSO == null) continue;

//             ProcessBridge(bridge);
//         }
//     }

//     private void ProcessBridge(InputMovementBridge bridge)
//     {
//         BaseInputSO input = bridge.baseInputSO;
//         MovementForceSO movement = bridge.movementSO;

//         Vector3 inputDirection = Vector3.zero;
//         if (input is ExternalInputSO externalInput)
//         {
//             inputDirection = externalInput.GetInputDirection();
//         }

//         switch (movement.ExecutionType)
//         {
//             case MovementExecutionType.Continuous:
//                 HandleContinuousMovement(bridge, input, movement, inputDirection);
//                 break;

//             case MovementExecutionType.Triggered:
//                 HandleTriggeredMovement(bridge, input, movement, inputDirection);
//                 break;

//             case MovementExecutionType.Charged:
//                 HandleChargedMovement(bridge, input, movement, inputDirection);
//                 break;
//         }
//     }

//     private void HandleContinuousMovement(InputMovementBridge bridge, BaseInputSO input, 
//                                           MovementForceSO movement, Vector3 inputDirection)
//     {
//         bool isHeld = input.IsHeld();

//         if (isHeld)
//         {
//             // Check max duration for continuous movements
//             float maxDuration = 0f;
//             if (movement is ContinuousRigidbodyMovement cRb) maxDuration = cRb.MaxDuration;
//             else if (movement is ContinuousTransformMovement cTr) maxDuration = cTr.MaxDuration;

//             if (maxDuration > 0)
//             {
//                 if (!continuousMovementTimers.ContainsKey(movement))
//                 {
//                     continuousMovementTimers[movement] = 0f;
//                 }

//                 continuousMovementTimers[movement] += Time.deltaTime;

//                 if (continuousMovementTimers[movement] >= maxDuration)
//                 {
//                     return; // Max duration reached
//                 }
//             }

//             movementController.ApplyForce(movement, inputDirection);
//         }
//         else
//         {
//             // Reset timer when not held
//             if (continuousMovementTimers.ContainsKey(movement))
//             {
//                 continuousMovementTimers.Remove(movement);
//             }
//         }
//     }

//     private void HandleTriggeredMovement(InputMovementBridge bridge, BaseInputSO input, 
//                                          MovementForceSO movement, Vector3 inputDirection)
//     {
//         // Check if already executing
//         bool isBlocked = activeTriggeredMovements.ContainsKey(movement);
        
//         bool shouldBlock = false;
//         if (movement is TriggeredRigidbodyMovement tRb) shouldBlock = tRb.BlockUntilComplete;
//         else if (movement is TriggeredTransformMovement tTr) shouldBlock = tTr.BlockUntilComplete;

//         if (isBlocked && shouldBlock) return;

//         if (input.ShouldExecute())
//         {
//             Coroutine coroutine = null;

//             if (movement is TriggeredRigidbodyMovement trigRb)
//             {
//                 coroutine = movementController.StartTriggeredMovement(trigRb, inputDirection);
//             }
//             else if (movement is TriggeredTransformMovement trigTr)
//             {
//                 coroutine = movementController.StartTriggeredMovement(trigTr, inputDirection);
//             }

//             if (coroutine != null)
//             {
//                 activeTriggeredMovements[movement] = coroutine;
//                 StartCoroutine(CleanupTriggeredMovement(movement, coroutine));
//             }
//         }
//     }

//     private IEnumerator CleanupTriggeredMovement(MovementForceSO movement, Coroutine coroutine)
//     {
//         yield return coroutine;
//         activeTriggeredMovements.Remove(movement);
//     }

//     private void HandleChargedMovement(InputMovementBridge bridge, BaseInputSO input, 
//                                        MovementForceSO movement, Vector3 inputDirection)
//     {
//         bool isHeld = input.IsHeld();
//         bool wasHeld = chargeTimers.ContainsKey(movement);

//         if (isHeld)
//         {
//             // Charging
//             if (!chargeTimers.ContainsKey(movement))
//             {
//                 chargeTimers[movement] = 0f;
//             }

//             float maxCharge = 0f;
//             if (movement is ChargedRigidbodyMovement chRb) maxCharge = chRb.MaxChargeTime;
//             else if (movement is ChargedTransformMovement chTr) maxCharge = chTr.MaxChargeTime;

//             chargeTimers[movement] = Mathf.Min(chargeTimers[movement] + Time.deltaTime, maxCharge);
//         }
//         else if (wasHeld)
//         {
//             // Released - Execute charged movement
//             float chargeTime = chargeTimers[movement];
//             float minCharge = 0f, maxCharge = 1f, multiplier = 1f;

//             if (movement is ChargedRigidbodyMovement chRb)
//             {
//                 minCharge = chRb.MinChargeTime;
//                 maxCharge = chRb.MaxChargeTime;
//                 multiplier = chRb.ChargeMultiplier;
//             }
//             else if (movement is ChargedTransformMovement chTr)
//             {
//                 minCharge = chTr.MinChargeTime;
//                 maxCharge = chTr.MaxChargeTime;
//                 multiplier = chTr.ChargeMultiplier;
//             }

//             if (chargeTime >= minCharge)
//             {
//                 float chargePercent = Mathf.Clamp01((chargeTime - minCharge) / (maxCharge - minCharge));
//                 float forceMult = 1f + (chargePercent * multiplier);
                
//                 Vector3 chargedDirection = inputDirection * forceMult;
//                 if (chargedDirection.sqrMagnitude < 0.001f)
//                 {
//                     chargedDirection = movement.Direction * forceMult;
//                 }

//                 movementController.ApplyForce(movement, chargedDirection);
//             }

//             chargeTimers.Remove(movement);
//         }
//     }
// }

// #endregion



// #region Input System

// [System.Serializable]
// public class InputMovementBridge
// {
//     public MovementForceSO movementSO;
//     public BaseInputSO baseInputSO;
// }

// public abstract class BaseInputSO : ScriptableObject
// {
//     public abstract bool ShouldExecute();
//     public abstract bool IsHeld();
// }

// [CreateAssetMenu(fileName = "NewExternalInputSO", menuName = "Movement/Input/ExternalInputSO")]
// public class ExternalInputSO : BaseInputSO
// {
//     public InputActionReference inputAction;

//     public override bool ShouldExecute()
//     {
//         if (inputAction == null || inputAction.action == null) return false;
        
//         var action = inputAction.action;
        
//         switch (action.type)
//         {
//             case InputActionType.Button:
//                 return action.triggered;
            
//             case InputActionType.Value:
//                 if (action.expectedControlType == "Vector2")
//                 {
//                     return action.ReadValue<Vector2>().sqrMagnitude > 0.001f;
//                 }
//                 return Mathf.Abs(action.ReadValue<float>()) > 0.001f;
            
//             default:
//                 return false;
//         }
//     }

//     public override bool IsHeld()
//     {
//         if (inputAction == null || inputAction.action == null) return false;
        
//         var action = inputAction.action;
//         return action.phase == InputActionPhase.Performed || action.phase == InputActionPhase.Started;
//     }

//     public Vector3 GetInputDirection()
//     {
//         if (inputAction == null || inputAction.action == null) return Vector3.zero;
        
//         var action = inputAction.action;
        
//         if (action.expectedControlType == "Vector2")
//         {
//             Vector2 input = action.ReadValue<Vector2>();
//             return new Vector3(input.x, 0, input.y);
//         }
//         else
//         {
//             float input = action.ReadValue<float>();
//             return new Vector3(input, 0, 0);
//         }
//     }
// }

// [CreateAssetMenu(fileName = "NewInternalInputSO", menuName = "Movement/Input/InternalInputSO")]
// public class InternalInputSO : BaseInputSO
// {
//     [Header("Runtime Control")]
//     [Tooltip("Toggle this bool from scripts to control movement")]
//     public bool IsActive = false;
    
//     [Header("Trigger Control")]
//     [Tooltip("Set this to true from code to trigger once, then automatically resets")]
//     public bool TriggerOnce = false;

//     public override bool ShouldExecute()
//     {
//         if (TriggerOnce)
//         {
//             TriggerOnce = false; // Auto-reset
//             return true;
//         }
//         return IsActive;
//     }

//     public override bool IsHeld()
//     {
//         return IsActive;
//     }

//     // Helper method for external scripts
//     public void Trigger()
//     {
//         TriggerOnce = true;
//     }

//     public void Activate()
//     {
//         IsActive = true;
//     }

//     public void Deactivate()
//     {
//         IsActive = false;
//     }
// }

// #endregion

// #region Movement Force ScriptableObjects

// public enum ForceType { Linear, Angular, Scaler }
// public enum CompletionType { None, Time, Rotation, Distance, Condition }

// public abstract class MovementForceSO : ScriptableObject
// {
//     [Header("Force Configuration")]
//     [SerializeField] protected ForceType forceType;
//     [SerializeField] protected Vector3 direction;
    
//     [Header("Input Control")]
//     [Tooltip("If true, uses input direction instead of configured direction")]
//     [SerializeField] protected bool useInputDirection = false;

//     public ForceType ForceType => forceType;
//     public Vector3 Direction => direction;
//     public bool UseInputDirection => useInputDirection;

//     public abstract MovementExecutionType ExecutionType { get; }
// }

// public enum MovementExecutionType
// {
//     Continuous,  // Executes while input is held
//     Triggered,   // Executes once to completion
//     Charged      // Hold to charge, release to execute
// }

// // ============= RIGIDBODY MOVEMENTS =============

// [CreateAssetMenu(fileName = "NewContinuousRigidbody", menuName = "Movement/Rigidbody/Continuous")]
// public class ContinuousRigidbodyMovement : MovementForceSO
// {
//     [Header("Rigidbody Settings")]
//     [SerializeField] private ForceMode2D forceMode2D = ForceMode2D.Force;
    
//     [Header("Duration (Optional)")]
//     [Tooltip("Max duration in seconds. 0 = unlimited")]
//     [Range(0, 10)][SerializeField] private float maxDuration = 0f;

//     public ForceMode2D ForceMode2D => forceMode2D;
//     public float MaxDuration => maxDuration;
//     public override MovementExecutionType ExecutionType => MovementExecutionType.Continuous;
// }

// [CreateAssetMenu(fileName = "NewTriggeredRigidbody", menuName = "Movement/Rigidbody/Triggered")]
// public class TriggeredRigidbodyMovement : MovementForceSO
// {
//     [Header("Rigidbody Settings")]
//     [SerializeField] private ForceMode2D forceMode2D = ForceMode2D.Impulse;
    
//     [Header("Completion Settings")]
//     [SerializeField] private CompletionType completionType = CompletionType.Time;
    
//     [Tooltip("Time in seconds, Degrees for rotation, or Distance in units")]
//     [SerializeField] private float completionValue = 1f;
    
//     [Tooltip("Can only trigger again after this completes")]
//     [SerializeField] private bool blockUntilComplete = true;

//     public ForceMode2D ForceMode2D => forceMode2D;
//     public CompletionType CompletionType => completionType;
//     public float CompletionValue => completionValue;
//     public bool BlockUntilComplete => blockUntilComplete;
//     public override MovementExecutionType ExecutionType => MovementExecutionType.Triggered;
// }

// [CreateAssetMenu(fileName = "NewChargedRigidbody", menuName = "Movement/Rigidbody/Charged")]
// public class ChargedRigidbodyMovement : MovementForceSO
// {
//     [Header("Rigidbody Settings")]
//     [SerializeField] private ForceMode2D forceMode2D = ForceMode2D.Impulse;
    
//     [Header("Charge Settings")]
//     [SerializeField] private float minChargeTime = 0.2f;
//     [SerializeField] private float maxChargeTime = 2f;
//     [SerializeField] private float chargeMultiplier = 2f;

//     public ForceMode2D ForceMode2D => forceMode2D;
//     public float MinChargeTime => minChargeTime;
//     public float MaxChargeTime => maxChargeTime;
//     public float ChargeMultiplier => chargeMultiplier;
//     public override MovementExecutionType ExecutionType => MovementExecutionType.Charged;
// }

// // ============= TRANSFORM MOVEMENTS =============

// [CreateAssetMenu(fileName = "NewContinuousTransform", menuName = "Movement/Transform/Continuous")]
// public class ContinuousTransformMovement : MovementForceSO
// {
//     [Header("Duration (Optional)")]
//     [Tooltip("Max duration in seconds. 0 = unlimited")]
//     [Range(0, 10)][SerializeField] private float maxDuration = 0f;

//     public float MaxDuration => maxDuration;
//     public override MovementExecutionType ExecutionType => MovementExecutionType.Continuous;
// }

// [CreateAssetMenu(fileName = "NewTriggeredTransform", menuName = "Movement/Transform/Triggered")]
// public class TriggeredTransformMovement : MovementForceSO
// {
//     [Header("Completion Settings")]
//     [SerializeField] private CompletionType completionType = CompletionType.Time;
    
//     [Tooltip("Time in seconds, Degrees for rotation, or Distance in units")]
//     [SerializeField] private float completionValue = 1f;
    
//     [Tooltip("Can only trigger again after this completes")]
//     [SerializeField] private bool blockUntilComplete = true;

//     public CompletionType CompletionType => completionType;
//     public float CompletionValue => completionValue;
//     public bool BlockUntilComplete => blockUntilComplete;
//     public override MovementExecutionType ExecutionType => MovementExecutionType.Triggered;
// }

// [CreateAssetMenu(fileName = "NewChargedTransform", menuName = "Movement/Transform/Charged")]
// public class ChargedTransformMovement : MovementForceSO
// {
//     [Header("Charge Settings")]
//     [SerializeField] private float minChargeTime = 0.2f;
//     [SerializeField] private float maxChargeTime = 2f;
//     [SerializeField] private float chargeMultiplier = 2f;

//     public float MinChargeTime => minChargeTime;
//     public float MaxChargeTime => maxChargeTime;
//     public float ChargeMultiplier => chargeMultiplier;
//     public override MovementExecutionType ExecutionType => MovementExecutionType.Charged;
// }

// #endregion
// #region Movement Executor

// public static class MovementExecutor
// {
//     public static void ApplyForce(MovementForceSO force, Vector3 inputDirection, Transform t = null, Rigidbody2D rb = null)
//     {
//         Vector3 finalDirection = force.UseInputDirection && inputDirection.sqrMagnitude > 0.001f 
//             ? inputDirection.normalized * force.Direction.magnitude 
//             : force.Direction;

//         switch (force)
//         {
//             case ContinuousRigidbodyMovement cRb:
//                 ApplyRigidbody2D(cRb.ForceType, finalDirection, rb, cRb.ForceMode2D);
//                 break;
            
//             case TriggeredRigidbodyMovement tRb:
//                 ApplyRigidbody2D(tRb.ForceType, finalDirection, rb, tRb.ForceMode2D);
//                 break;
            
//             case ChargedRigidbodyMovement chRb:
//                 ApplyRigidbody2D(chRb.ForceType, finalDirection, rb, chRb.ForceMode2D);
//                 break;
            
//             case ContinuousTransformMovement cTr:
//                 ApplyTransform(cTr.ForceType, finalDirection, t);
//                 break;
            
//             case TriggeredTransformMovement tTr:
//                 ApplyTransform(tTr.ForceType, finalDirection, t);
//                 break;
            
//             case ChargedTransformMovement chTr:
//                 ApplyTransform(chTr.ForceType, finalDirection, t);
//                 break;
            
//             default:
//                 Debug.LogError($"Unknown MovementForce type: {force.GetType().Name}");
//                 break;
//         }
//     }

//     private static void ApplyTransform(ForceType forceType, Vector3 direction, Transform t)
//     {
//         if (t == null) return;

//         switch (forceType)
//         {
//             case ForceType.Linear:
//                 t.Translate(direction * Time.deltaTime, Space.World);
//                 break;
//             case ForceType.Angular:
//                 t.Rotate(direction * Time.deltaTime, Space.Self);
//                 break;
//             case ForceType.Scaler:
//                 t.localScale += direction * Time.deltaTime;
//                 break;
//         }
//     }

//     private static void ApplyRigidbody2D(ForceType forceType, Vector3 direction, Rigidbody2D rb, ForceMode2D forceMode)
//     {
//         if (rb == null) return;

//         switch (forceType)
//         {
//             case ForceType.Linear:
//                 rb.AddForce(direction, forceMode);
//                 break;
//             case ForceType.Angular:
//                 rb.AddTorque(direction.z, forceMode);
//                 break;
//             case ForceType.Scaler:
//                 rb.transform.localScale += direction * Time.deltaTime;
//                 break;
//         }
//     }
// }

// #endregion
