using UnityEngine;
using UnityEngine.InputSystem;

public enum ExecutionType
{
    Continuous,
    Triggered,
    Manual,
    Timed
    
}

public abstract class BaseInputSO : ScriptableObject
{
    public ExecutionType executionType = ExecutionType.Continuous;
}

[CreateAssetMenu(fileName = "NewExternalInputSO", menuName = "Movement/ExternalInputSO")]
public class ExternalInputSO : BaseInputSO
{
    public InputActionReference inputAction;
}

[CreateAssetMenu(fileName = "NewInternalInputSO", menuName = "Movement/InternalInputSO")]
public class InternalInputSO : BaseInputSO
{
    
}
