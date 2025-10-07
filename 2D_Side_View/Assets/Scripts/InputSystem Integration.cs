using UnityEngine;
using UnityEngine.InputSystem;




[System.Serializable]
public class InputMovementBridge
{
    public MovementForceSO movementSO;
    public BaseInputSO baseInputSO;
}




public abstract class BaseInputSO : ScriptableObject
{

}

[CreateAssetMenu(fileName = "NewExternalInputSO", menuName = "Movement/Input/ExternalInputSO")]
public class ExternalInputSO : BaseInputSO
{
    public InputActionReference inputAction;
}


[CreateAssetMenu(fileName = "NewInternalInputSO", menuName = "Movement/Input/InternalInputSO")]
public class InternalInputSO : BaseInputSO
{
    // The boolean field you want to control at runtime via the Inspector.
    [Header("Runtime Control")]
    [Tooltip("If checked, the movement associated with this SO will be continuously applied.")]
    public bool IsActive = false;

    // // (Optional: Keep the magnitude from the previous example for force strength)
    // [SerializeField] private float movementMagnitude = 1f;
    // public float MovementMagnitude => movementMagnitude; 
}



public abstract class MovementForceSO : ScriptableObject
{
    public abstract ForceType ForceType { get; }
    public abstract Vector3 Direction { get; }
    public abstract float Duration { get; }
}

[CreateAssetMenu(fileName = "NewRigidbodyMovementForce", menuName = "Movement/Force/RigidbodyForce")]
public class RigidbodyMovementForce : MovementForceSO
{
    [SerializeField] private ForceType forceType;
    [SerializeField] private Vector3 direction;
    [Range(0, 10)][SerializeField] private float duration;
    [SerializeField] ForceMode2D forceMode2D;

    public override ForceType ForceType => forceType;
    public override Vector3 Direction => direction;
    public override float Duration => duration;
    public ForceMode2D ForceMode2D => forceMode2D;

}

[CreateAssetMenu(fileName = "NewTransformMovementForce", menuName = "Movement/Force/TransformForce")]
public class TransformMovementForce : MovementForceSO
{
    [SerializeField] private ForceType forceType;
    [SerializeField] private Vector3 direction;
    [Range(0,10)] [SerializeField] private float duration;
    public override ForceType ForceType => forceType;
    public override Vector3 Direction => direction;
    public override float Duration => duration;

}


