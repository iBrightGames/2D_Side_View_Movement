using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;

#region Input Movement Handler

public class InputMovementHandler : MonoBehaviour
{
    [SerializeField] private InputMovementBridgeSO[] inputMovementBridges;
    private MovementController movementController;

    
    private System.Collections.Generic.Dictionary<Movement, Coroutine> activeTriggeredMovements 
        = new System.Collections.Generic.Dictionary<Movement, Coroutine>();// Track active coroutines for triggered movements


    private System.Collections.Generic.Dictionary<Movement, float> continuousMovementTimers 
        = new System.Collections.Generic.Dictionary<Movement, float>();    // Track continuous movements


    private System.Collections.Generic.Dictionary<Movement, float> chargeTimers 
        = new System.Collections.Generic.Dictionary<Movement, float>();    // Track charged movements

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
    }

    private void Update()
    {
        foreach (var bridge in inputMovementBridges)
        {
            if (bridge.baseInput == null || bridge.movement == null) continue;

            ProcessBridge(bridge);
        }
    }

    private void ProcessBridge(InputMovementBridgeSO bridge)
    {
        BaseInput input = bridge.baseInput;
        Movement movement = bridge.movement;

        Vector3 inputDirection = Vector3.zero;
        if (input is ExternalInput externalInput)
        {
            inputDirection = externalInput.GetInputDirection();
        }

        switch (movement.ExecutionType)
        {
            case MovementExecutionType.Continuous:
                HandleContinuousMovement(bridge, input, movement, inputDirection);
                break;

            case MovementExecutionType.Triggered:
                HandleTriggeredMovement(bridge, input, movement, inputDirection);
                break;

            case MovementExecutionType.Charged:
                HandleChargedMovement(bridge, input, movement, inputDirection);
                break;
        }
    }

    private void HandleContinuousMovement(InputMovementBridgeSO bridge, BaseInput input, 
                                          Movement movement, Vector3 inputDirection)
    {
        bool isHeld = input.IsHeld();

        if (isHeld)
        {
            // Check max duration for continuous movements
            float maxDuration = 0f;
            if (movement is ContinuousRigidbodyMovement cRb) maxDuration = cRb.MaxDuration;
            else if (movement is ContinuousTransformMovement cTr) maxDuration = cTr.MaxDuration;

            if (maxDuration > 0)
            {
                if (!continuousMovementTimers.ContainsKey(movement))
                {
                    continuousMovementTimers[movement] = 0f;
                }

                continuousMovementTimers[movement] += Time.deltaTime;

                if (continuousMovementTimers[movement] >= maxDuration)
                {
                    return; // Max duration reached
                }
            }

            movementController.ApplyForce(movement, inputDirection);
        }
        else
        {
            // Reset timer when not held
            if (continuousMovementTimers.ContainsKey(movement))
            {
                continuousMovementTimers.Remove(movement);
            }
        }
    }

    private void HandleTriggeredMovement(InputMovementBridgeSO bridge, BaseInput input, 
                                         Movement movement, Vector3 inputDirection)
    {
        // Check if already executing
        bool isBlocked = activeTriggeredMovements.ContainsKey(movement);
        
        bool shouldBlock = false;
        if (movement is TriggeredRigidbodyMovement tRb) shouldBlock = tRb.BlockUntilComplete;
        else if (movement is TriggeredTransformMovement tTr) shouldBlock = tTr.BlockUntilComplete;

        if (isBlocked && shouldBlock) return;

        if (input.ShouldExecute())
        {
            Coroutine coroutine = null;

            if (movement is TriggeredRigidbodyMovement trigRb)
            {
                coroutine = movementController.StartTriggeredMovement(trigRb, inputDirection);
            }
            else if (movement is TriggeredTransformMovement trigTr)
            {
                coroutine = movementController.StartTriggeredMovement(trigTr, inputDirection);
            }

            if (coroutine != null)
            {
                activeTriggeredMovements[movement] = coroutine;
                StartCoroutine(CleanupTriggeredMovement(movement, coroutine));
            }
        }
    }

    private IEnumerator CleanupTriggeredMovement(Movement movement, Coroutine coroutine)
    {
        yield return coroutine;
        activeTriggeredMovements.Remove(movement);
    }

    private void HandleChargedMovement(InputMovementBridgeSO bridge, BaseInput input, 
                                       Movement movement, Vector3 inputDirection)
    {
        bool isHeld = input.IsHeld();
        bool wasHeld = chargeTimers.ContainsKey(movement);

        if (isHeld)
        {
            // Charging
            if (!chargeTimers.ContainsKey(movement))
            {
                chargeTimers[movement] = 0f;
            }

            float maxCharge = 0f;
            if (movement is ChargedRigidbodyMovement chRb) maxCharge = chRb.MaxChargeTime;
            else if (movement is ChargedTransformMovement chTr) maxCharge = chTr.MaxChargeTime;

            chargeTimers[movement] = Mathf.Min(chargeTimers[movement] + Time.deltaTime, maxCharge);
        }
        else if (wasHeld)
        {
            // Released - Execute charged movement
            float chargeTime = chargeTimers[movement];
            float minCharge = 0f, maxCharge = 1f, multiplier = 1f;

            if (movement is ChargedRigidbodyMovement chRb)
            {
                minCharge = chRb.MinChargeTime;
                maxCharge = chRb.MaxChargeTime;
                multiplier = chRb.ChargeMultiplier;
            }
            else if (movement is ChargedTransformMovement chTr)
            {
                minCharge = chTr.MinChargeTime;
                maxCharge = chTr.MaxChargeTime;
                multiplier = chTr.ChargeMultiplier;
            }

            if (chargeTime >= minCharge)
            {
                float chargePercent = Mathf.Clamp01((chargeTime - minCharge) / (maxCharge - minCharge));
                float forceMult = 1f + (chargePercent * multiplier);
                
                Vector3 chargedDirection = inputDirection * forceMult;
                if (chargedDirection.sqrMagnitude < 0.001f)
                {
                    chargedDirection = movement.Direction * forceMult;
                }

                movementController.ApplyForce(movement, chargedDirection);
            }

            chargeTimers.Remove(movement);
        }
    }
}

#endregion



