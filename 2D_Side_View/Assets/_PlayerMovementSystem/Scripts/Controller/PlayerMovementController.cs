// ============================================
// PLAYER MOVEMENT CONTROLLER
// ============================================

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Movement Configuration")]
    [SerializeField] private InputMovementBridge[] inputMovementBridges;

    [Header("Ground Detection")]
    [SerializeField] private bool checkGrounded = true;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 groundCheckOffset = Vector2.zero;
    [SerializeField] private float groundCheckDistance = 0.1f;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = false;

    private bool isGrounded;

    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (showDebugLogs)
        {
            Debug.Log($"[Movement] Awake - Rigidbody2D: {rb != null}, Bridges: {inputMovementBridges.Length}");
        }
    }

    private void OnEnable()
    {
        foreach (var bridge in inputMovementBridges)
        {
            if (bridge.inputConfig == null) continue;

            bridge.inputConfig.Initialize();

            // Triggered input'lar için event subscribe
            if (bridge.inputConfig.inputType == InputType.Triggered)
            {
                var action = bridge.inputConfig.inputActionReference.action;
                action.performed += ctx => OnTriggeredInput(bridge);

                if (showDebugLogs)
                {
                    Debug.Log($"[Movement] Subscribed to triggered input: {action.name}");
                }
            }
        }
    }

    private void OnDisable()
    {
        foreach (var bridge in inputMovementBridges)
        {
            if (bridge.inputConfig == null) continue;

            if (bridge.inputConfig.inputType == InputType.Triggered)
            {
                var action = bridge.inputConfig.inputActionReference.action;
                action.performed -= ctx => OnTriggeredInput(bridge);
            }

            bridge.inputConfig.Cleanup();
        }
    }

    private void Update()
    {
        // Ground check
        if (checkGrounded)
        {
            CheckGrounded();
        }

        // Continuous input'ları işle (Update'te oku, FixedUpdate'te uygula)
        foreach (var bridge in inputMovementBridges)
        {
            if (bridge.inputConfig == null) continue;

            if (bridge.inputConfig.inputType == InputType.Continuous)
            {
                Vector2? inputDir = bridge.inputConfig.GetDirection();

                if (inputDir.HasValue && inputDir.Value != Vector2.zero)
                {
                    if (bridge.CanExecute(isGrounded))
                    {
                        if (showDebugLogs)
                        {
                            Debug.Log($"[Continuous] Input: {inputDir.Value}");
                        }
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // Continuous movement'ları uygula
        foreach (var bridge in inputMovementBridges)
        {
            if (bridge.inputConfig == null) continue;

            if (bridge.inputConfig.inputType == InputType.Continuous)
            {
                Vector2? inputDir = bridge.inputConfig.GetDirection();

                if (inputDir.HasValue && inputDir.Value != Vector2.zero)
                {
                    if (bridge.CanExecute(isGrounded))
                    {
                        MovementExecuter.ExecuteMovement(rb, bridge, inputDir, showDebugLogs);
                    }
                }
            }
        }
    }

    private void OnTriggeredInput(InputMovementBridge bridge)
    {
        if (!bridge.CanExecute(isGrounded))
        {
            if (showDebugLogs)
            {
                Debug.Log($"[Triggered] Movement blocked - Grounded: {isGrounded}, Cooldown active");
            }
            return;
        }

        Vector2? inputDir = bridge.inputConfig.GetDirection();

        if (showDebugLogs)
        {
            Debug.Log($"[Triggered] Input received - Direction: {inputDir}");
        }

        MovementExecuter.ExecuteMovement(rb, bridge, inputDir, showDebugLogs);
    }

    private void CheckGrounded()
    {
        Vector2 checkPosition = (Vector2)transform.position + groundCheckOffset;
        isGrounded = Physics2D.Raycast(checkPosition, Vector2.down, groundCheckDistance, groundLayer);

        if (showDebugLogs)
        {
            Debug.DrawRay(checkPosition, Vector2.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!checkGrounded) return;

        Vector2 checkPosition = (Vector2)transform.position + groundCheckOffset;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(checkPosition, checkPosition + Vector2.down * groundCheckDistance);
        Gizmos.DrawWireSphere(checkPosition + Vector2.down * groundCheckDistance, 0.1f);
    }
}

