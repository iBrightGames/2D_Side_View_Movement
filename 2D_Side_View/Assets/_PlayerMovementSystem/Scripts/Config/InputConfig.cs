
using UnityEngine;
using UnityEngine.InputSystem;



[CreateAssetMenu(fileName = "InputConfig", menuName = "Scriptable Objects/InputConfig")]
public class InputConfig : ScriptableObject
{
    [SerializeField] public InputActionReference inputActionReference;
    [SerializeField] public InputType inputType;
    [SerializeField]
    public Vector2? GetDirection()
    {
        if (inputActionReference == null || inputActionReference.action == null) return null;
        var action = inputActionReference.action;
        if (action.expectedControlType == "Vector2")
        {
            return action.ReadValue<Vector2>();
        }
        return null;
    }


}
