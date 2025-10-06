using UnityEngine;

#region Input System Integration

public class InputMovementBridge : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MovementController movementController;

    [Header("Input Mappings")]
    [SerializeField] private InputMovementMapping[] inputMappings;

    private void Update()
    {
        if (movementController == null || inputMappings == null) return;

        foreach (var mapping in inputMappings)
        {
            if (mapping.baseInputSO == null || mapping.movementSO == null)
                continue;

            var input = mapping.baseInputSO;
            var config = mapping.movementSO;


        }
    }

    private void OnValidate()
    {
        if (movementController == null)
            movementController = GetComponent<MovementController>();
    }

}


#endregion

