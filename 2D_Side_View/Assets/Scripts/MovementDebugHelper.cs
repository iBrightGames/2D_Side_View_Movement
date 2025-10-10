using UnityEngine;

/// <summary>
/// Attach this to your character to debug movement issues
/// Check the Console for diagnostic messages
/// </summary>
public class MovementDebugHelper : MonoBehaviour
{
    [Header("References to Check")]
    [SerializeField] private InputMovementHandler inputHandler;
    [SerializeField] private MovementController movementController;
    [SerializeField] private Rigidbody2D rb;

    [Header("Debug Settings")]
    [SerializeField] private bool logEveryFrame = false;
    [SerializeField] private bool logInputStates = true;
    [SerializeField] private bool logMovementExecutions = true;

    private void Start()
    {
        Debug.Log("=== MOVEMENT DEBUG HELPER STARTED ===");

        // Auto-find components if not assigned
        if (inputHandler == null) inputHandler = GetComponent<InputMovementHandler>();
        if (movementController == null) movementController = GetComponent<MovementController>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        // Check components
        if (inputHandler == null)
            Debug.LogError("❌ InputMovementHandler NOT FOUND!");
        else
            Debug.Log("✓ InputMovementHandler found");

        if (movementController == null)
            Debug.LogError("❌ MovementController NOT FOUND!");
        else
            Debug.Log("✓ MovementController found");

        if (rb == null)
            Debug.LogWarning("⚠ Rigidbody2D NOT FOUND (needed for Rigidbody movements)");
        else
        {
            Debug.Log($"✓ Rigidbody2D found - Body Type: {rb.bodyType}");
            if (rb.bodyType != RigidbodyType2D.Dynamic)
                Debug.LogError("❌ Rigidbody2D must be set to DYNAMIC!");
        }

        // Check bridges
        CheckBridges();
    }

    private void CheckBridges()
    {
        if (inputHandler == null) return;

        var bridgesField = typeof(InputMovementHandler).GetField("inputMovementBridges",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (bridgesField != null)
        {
            var bridges = bridgesField.GetValue(inputHandler) as InputMovementBridgeSO[];

            if (bridges == null || bridges.Length == 0)
            {
                Debug.LogError("❌ NO BRIDGES ASSIGNED! Assign InputMovementBridgeSO assets in the Inspector.");
                return;
            }

            Debug.Log($"Found {bridges.Length} bridge(s):");

            for (int i = 0; i < bridges.Length; i++)
            {
                var bridge = bridges[i];
                if (bridge == null)
                {
                    Debug.LogError($"❌ Bridge [{i}] is NULL!");
                    continue;
                }

                Debug.Log($"\n--- Bridge [{i}]: {bridge.name} ---");

                // Check input
                if (bridge.baseInput == null)
                {
                    Debug.LogError($"❌ Bridge [{i}] has NULL baseInput!");
                }
                else
                {
                    Debug.Log($"✓ Input Type: {bridge.baseInput.GetType().Name}");

                    if (bridge.baseInput is ExternalInput ext)
                    {
                        if (ext.inputAction == null)
                            Debug.LogError($"❌ ExternalInput has NULL InputActionReference!");
                        else
                        {
                            Debug.Log($"  Action: {ext.inputAction.name}");
                            if (ext.inputAction.action == null)
                                Debug.LogError($"❌ InputAction is NULL! Check Input Actions asset.");
                            else if (!ext.inputAction.action.enabled)
                                Debug.LogWarning($"⚠ InputAction is DISABLED! Enable it in code or Input Actions settings.");
                        }
                    }
                    else if (bridge.baseInput is InternalInput intern)
                    {
                        Debug.Log($"  IsActive: {intern.IsActive}, TriggerOnce: {intern.TriggerOnce}");
                    }
                }

                // Check movement
                if (bridge.movement == null)
                {
                    Debug.LogError($"❌ Bridge [{i}] has NULL movement!");
                }
                else
                {
                    Debug.Log($"✓ Movement Type: {bridge.movement.GetType().Name}");
                    Debug.Log($"  ExecutionType: {bridge.movement.ExecutionType}");
                    Debug.Log($"  ForceType: {bridge.movement.ForceType}");
                    Debug.Log($"  Direction: {bridge.movement.Direction}");
                    Debug.Log($"  UseInputDirection: {bridge.movement.UseInputDirection}");
                }
            }
        }
    }

    private void Update()
    {
        if (!logEveryFrame) return;
        if (inputHandler == null) return;

        var bridgesField = typeof(InputMovementHandler).GetField("inputMovementBridges",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (bridgesField != null)
        {
            var bridges = bridgesField.GetValue(inputHandler) as InputMovementBridgeSO[];
            if (bridges == null) return;

            foreach (var bridge in bridges)
            {
                if (bridge?.baseInput == null) continue;

                bool held = bridge.baseInput.IsHeld();
                bool shouldExecute = bridge.baseInput.ShouldExecute();

                if (held || shouldExecute)
                {
                    Vector3 dir = Vector3.zero;
                    if (bridge.baseInput is ExternalInput ext)
                        dir = ext.GetInputDirection();

                    Debug.Log($"[{bridge.name}] Held: {held}, Execute: {shouldExecute}, InputDir: {dir}");
                }
            }
        }

        // Log Rigidbody state
        if (rb != null && logMovementExecutions)
        {
            if (rb.linearVelocity.sqrMagnitude > 0.01f)
                Debug.Log($"Rigidbody Velocity: {rb.linearVelocity}");
        }
    }

    private void OnEnable()
    {
        // Try to enable input actions
        if (inputHandler != null)
        {
            var bridgesField = typeof(InputMovementHandler).GetField("inputMovementBridges",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (bridgesField != null)
            {
                var bridges = bridgesField.GetValue(inputHandler) as InputMovementBridgeSO[];
                if (bridges != null)
                {
                    foreach (var bridge in bridges)
                    {
                        if (bridge?.baseInput is ExternalInput ext && ext.inputAction?.action != null)
                        {
                            ext.inputAction.action.Enable();
                            Debug.Log($"Enabled input action: {ext.inputAction.name}");
                        }
                    }
                }
            }
        }
    }
}
