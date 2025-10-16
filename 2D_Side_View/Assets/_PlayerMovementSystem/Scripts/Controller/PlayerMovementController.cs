using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct InputMovementBridge
{
    public InputConfig inputConfig;
    public MovementConfig movementConfig;
}

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private InputMovementBridge[] inputMovementBridges;
    [SerializeField] private bool showDebugLogs = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (showDebugLogs) Debug.Log($"[Movement] Awake - Rigidbody2D: {rb != null}");
    }
    
    private void OnEnable()
    {
        if (showDebugLogs) Debug.Log($"[Movement] OnEnable - Bridge count: {inputMovementBridges.Length}");
        
        foreach (var bridge in inputMovementBridges)
        {
            if (bridge.inputConfig.inputType == InputType.Triggered && bridge.inputConfig != null)
            {
                bridge.inputConfig.inputActionReference.action.performed += ctx =>
                {
                    if (showDebugLogs) Debug.Log($"[Movement] Triggered input performed: {bridge.inputConfig.inputActionReference.action.name}");
                    ExecuteTriggeredMovement(bridge);
                };
                bridge.inputConfig.inputActionReference.action.Enable();
                
                if (showDebugLogs) Debug.Log($"[Movement] Enabled triggered input: {bridge.inputConfig.inputActionReference.action.name}");
            }
        }
    }
    
    private void OnDisable()
    {
        foreach (var bridge in inputMovementBridges)
        {
            if (bridge.inputConfig.inputType == InputType.Triggered && bridge.inputConfig != null)
            {
                bridge.inputConfig.inputActionReference.action.performed -= ctx => ExecuteTriggeredMovement(bridge);
            }
        }
    }

    private void Update()
    {
        foreach (var bridge in inputMovementBridges)
        {
            if (bridge.inputConfig.inputType == InputType.Continuous && bridge.inputConfig.inputActionReference != null)
            {
                Vector2? inputDir = bridge.inputConfig.GetDirection();
                
                if (showDebugLogs && inputDir.HasValue && inputDir.Value != Vector2.zero)
                {
                    Debug.Log($"[Movement] Continuous input - Action: {bridge.inputConfig.inputActionReference.action.name}, Direction: {inputDir.Value}");
                }
                
                if (inputDir.HasValue && inputDir.Value != Vector2.zero)
                {
                    ExecuteMovement(bridge, inputDir);
                }
            }
        }
    }
    
    private void ExecuteTriggeredMovement(InputMovementBridge bridge)
    {
        Vector2? inputDir = bridge.inputConfig.GetDirection();
        if (showDebugLogs) Debug.Log($"[Movement] ExecuteTriggeredMovement - InputDir: {inputDir}");
        ExecuteMovement(bridge, inputDir);
    }

    private void ExecuteMovement(InputMovementBridge bridge, Vector2? inputDirection)
    {
        var config = bridge.movementConfig;
        
        if (config.forceType == ForceType.Linear)
        {
            Vector2 force = config.GetLinearDirection(inputDirection, rb.linearVelocity);
            
            if (showDebugLogs)
            {
                Debug.Log($"[Movement] Linear Force - Direction: {force}, Magnitude: {force.magnitude}, ForceMode: {config.forceMode2D}");
                Debug.Log($"[Movement] Before - Velocity: {rb.linearVelocity}, Position: {rb.position}");
            }
            
            MovementExecuter.ApplyLinearForce(rb, force, config.forceMode2D, config);
            
            if (showDebugLogs)
            {
                Debug.Log($"[Movement] After - Velocity: {rb.linearVelocity}");
            }
        }
        else if (config.forceType == ForceType.Angular)
        {
            float torque = config.GetAngularMagnitude();
            
            if (showDebugLogs)
            {
                Debug.Log($"[Movement] Angular Force - Torque: {torque}");
                Debug.Log($"[Movement] Before - AngularVelocity: {rb.angularVelocity}");
            }
            
            MovementExecuter.ApplyAngularForce(rb, torque, config);
            
            if (showDebugLogs)
            {
                Debug.Log($"[Movement] After - AngularVelocity: {rb.angularVelocity}");
            }
        }
    }
}

// using UnityEngine;
// using UnityEngine.InputSystem;

// [System.Serializable]
// public struct InputMovementBridge
// {
//     [SerializeField] public InputConfig inputConfig;
//     [SerializeField] public MovementConfig movementConfig;
// }
// [RequireComponent(typeof(Rigidbody2D))]
// public class PlayerMovementController : MonoBehaviour
// {
//     [SerializeField] Rigidbody2D rigidbody2D;
//     [SerializeField] InputMovementBridge[] inputMovementBridges;

//     void Awake()
//     {
//         rigidbody2D = GetComponent<Rigidbody2D>();
//     }
//     private void OnEnable()
//     {
//         foreach (var bridge in inputMovementBridges)
//         {
//             if (bridge.inputConfig.inputType == InputType.Triggered && bridge.inputConfig != null)
//             {
//                 bridge.inputConfig.inputActionReference.action.performed += ctx =>
//                 {
                    
//                 };
//                 bridge.inputConfig.inputActionReference.action.Enable();
//             }
//         }
//     }

//     private void Update()
//     {
//         foreach (var bridge in inputMovementBridges)
//         {
//             if (bridge.inputConfig.inputType == InputType.Continuous && bridge.inputConfig.inputActionReference != null)
//             {
//                 Vector2 dir = bridge.inputConfig.inputActionReference.action.ReadValue<Vector2>();
//                 if (dir != Vector2.zero)
//                     ApplyMovement(bridge.movementConfig, dir);
//             }
//         }
//     }

//     private void ApplyMovement(MovementConfig config, Vector2 direction)
//     {
//         MovementExecuter.ApplyLinearForce(rigidbody2D, direction, config.forceMode2D);
//     }
// }
