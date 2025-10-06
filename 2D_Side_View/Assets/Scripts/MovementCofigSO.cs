using UnityEngine;

#region ScriptableObject Configuration

[CreateAssetMenu(fileName = "NewMovementConfig", menuName = "Movement/Config")]
public class MovementConfigSO : ScriptableObject
{
    [SerializeField] private MovementForce[] movements;

    public MovementForce[] GetAllMovements() => movements;

    // public MovementForce GetMovementByName(string movementName)
    // {
    //     foreach (var movement in movements)
    //     {
    //         if (movement.name == movementName)
    //             return movement;
    //     }

    //     Debug.LogWarning($"[MovementConfigSO] Movement '{movementName}' not found in {name}");
    //     return default;
    // }

    // public MovementForce GetMovementByIndex(int index)
    // {
    //     if (index >= 0 && index < movements.Length)
    //         return movements[index];

    //     Debug.LogWarning($"[MovementConfigSO] Invalid index {index} in {name}");
    //     return default;
    // }

    // public bool HasMovement(string movementName)
    // {
    //     foreach (var movement in movements)
    //     {
    //         if (movement.name == movementName)
    //             return true;
    //     }
    //     return false;
    // }

}

#endregion

