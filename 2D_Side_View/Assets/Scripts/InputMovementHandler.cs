using UnityEngine;
using System.Collections;
using System.Collections.Generic;



#region Input Movement Handler

public class InputMovementHandler : MonoBehaviour
{
    [SerializeField] private InputMovementBridge[] inputMovementBridges;
    private MovementController movementController;
    
    // State tracking
    private Dictionary<MovementForceSO, Coroutine> activeTriggered = new Dictionary<MovementForceSO, Coroutine>();
    private Dictionary<MovementForceSO, float> continuousTimers = new Dictionary<MovementForceSO, float>();
    private Dictionary<MovementForceSO, float> chargeTimers = new Dictionary<MovementForceSO, float>();
    
    private void Awake()
    {
        movementController = GetComponent<MovementController>();
    }
    
    private void Update()
    {
        foreach (var bridge in inputMovementBridges)
        {
            if (bridge == null || !bridge.enabled) continue;
            ProcessBridge(bridge);
        }
    }
    
    private void ProcessBridge(InputMovementBridge bridge)
    {
        // Check if conditions allow execution
        if (!bridge.CanExecute(movementController))return;
        
        BaseInputSO input = bridge.inputSource;
        MovementForceSO movement = bridge.movement;
        
        Vector3 inputDirection = Vector3.zero;
        if (input is ExternalInputSO external) inputDirection = external.GetInputDirection();
        else if (input is InternalInputSO internalInput) inputDirection = internalInput.GetDirection();
        
        switch (movement.ExecutionType)
        {
            case MovementExecutionType.Continuous:
                HandleContinuous(bridge, input, movement, inputDirection);
                break;
            case MovementExecutionType.Triggered:
                HandleTriggered(bridge, input, movement, inputDirection);
                break;
            case MovementExecutionType.Charged:
                HandleCharged(bridge, input, movement, inputDirection);
                break;
        }
    }
    
    private void HandleContinuous(InputMovementBridge bridge, BaseInputSO input, MovementForceSO movement, Vector3 inputDir)
    {
        if (input.IsHeld())
        {
            if (!bridge.isExecuting)
            {
                bridge.isExecuting = true;
                movementController.OnMovementStarted?.Invoke();
                movementController.PlayFeedback(bridge.feedback);
            }
            
            movementController.ApplyForce(movement, inputDir, bridge.modifiers);
            bridge.MarkExecuted();
        }
        else
        {
            if (bridge.isExecuting)
            {
                bridge.isExecuting = false;
                movementController.OnMovementCompleted?.Invoke();
            }
        }
    }

    private void HandleTriggered(InputMovementBridge bridge, BaseInputSO input, MovementForceSO movement, Vector3 inputDir)
    {
        if (bridge.isExecuting) return;
        
        if (input.ShouldExecute())
        {
            bridge.isExecuting = true;
            bridge.MarkExecuted();
            input.ClearBuffer();
            
            movementController.OnMovementStarted?.Invoke();
            movementController.PlayFeedback(bridge.feedback);
            movementController.ApplyForce(movement, inputDir, bridge.modifiers);
            
            // For triggered movements, mark as complete after a frame
            StartCoroutine(CompleteTriggeredMovement(bridge));
        }
    }
    
    private IEnumerator CompleteTriggeredMovement(InputMovementBridge bridge)
    {
        yield return new WaitForEndOfFrame();
        bridge.isExecuting = false;
        movementController.OnMovementCompleted?.Invoke();
    }
    
    private void HandleCharged(InputMovementBridge bridge, BaseInputSO input, MovementForceSO movement, Vector3 inputDir)
    {
        if (input.IsHeld())
        {
            // Charging
            if (!chargeTimers.ContainsKey(movement))
            {
                chargeTimers[movement] = 0f;
                movementController.OnMovementStarted?.Invoke();
            }
            
            chargeTimers[movement] += Time.deltaTime;
        }
        else if (chargeTimers.ContainsKey(movement))
        {
            // Release - Execute with charge multiplier
            float chargeTime = chargeTimers[movement];
            chargeTimers.Remove(movement);
            
            // Apply charge scaling (simplified - enhance based on your ChargedMovement properties)
            Vector3 chargedDir = inputDir * (1f + chargeTime);
            
            bridge.MarkExecuted();
            movementController.PlayFeedback(bridge.feedback);
            movementController.ApplyForce(movement, chargedDir, bridge.modifiers);
            movementController.OnMovementCompleted?.Invoke();
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (movementController == null) return;
        
        // Draw condition gizmos
        foreach (var bridge in inputMovementBridges)
        {
            if (bridge?.conditions == null) continue;
            
            foreach (var condition in bridge.conditions)
            {
                condition?.DrawGizmos(movementController);
            }
        }
    }
}

#endregion
