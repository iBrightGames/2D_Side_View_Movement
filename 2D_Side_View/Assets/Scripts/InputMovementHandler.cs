using UnityEngine;
using UnityEngine.InputSystem;


public class InputMovementHandler : MonoBehaviour
{
    [SerializeField] private InputMovementBridge[] inputMovementBridges;
    private MovementController movementController;

    private void Awake()
    {
        movementController = GetComponent<MovementController>();
    }

    private void Update()
    {
        foreach (var b in inputMovementBridges)
        {
            if (b.baseInputSO is not ExternalInputSO ext)
                continue;

            var action = ext.inputAction.action;

            switch (action.type)
            {
                case InputActionType.Value:
                    HandleValueInput(b, action);
                    break;

                case InputActionType.Button:
                    HandleButtonInput(b, action);
                    break;
            }
        }
    }

    private void HandleValueInput(InputMovementBridge bridge, InputAction action)
    {
        // Vector2 veya float olabilir, kontrol et
        if (action.expectedControlType == "Vector2")
        {
            Vector2 input = action.ReadValue<Vector2>();
            if (input.sqrMagnitude > 0.001f)
            {
                // input yönü ile kuvveti çarp
                Vector3 dir = new Vector3(input.x, 0, input.y);
                movementController.ApplyForce(bridge.movementSO);
            }
        }
        else // float tipi continuous inputlar
        {
            float input = action.ReadValue<float>();
            if (input > 0)
            {
                movementController.ApplyForce(bridge.movementSO);
            }
        }
    }

    private void HandleButtonInput(InputMovementBridge bridge, InputAction action)
    {
        if (action.triggered)
        {
            movementController.ApplyForce(bridge.movementSO);
        }
    }
}

