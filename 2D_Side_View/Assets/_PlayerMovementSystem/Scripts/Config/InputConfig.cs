// ============================================
// INPUT CONFIGS
// ============================================

using UnityEngine;
using UnityEngine.InputSystem;

public enum InputType
{
    Continuous,  // Her frame okunan (movement, look)
    Triggered    // Event-based (jump, dash)
}

public abstract class InputConfig : ScriptableObject
{
    [SerializeReference] public InputActionReference inputActionReference;
    [SerializeReference] public InputType inputType = InputType.Continuous;
    
    protected InputAction cachedAction;
    
    public virtual void Initialize()
    {
        if (inputActionReference != null)
        {
            cachedAction = inputActionReference.action;
            if (cachedAction != null && !cachedAction.enabled)
            {
                cachedAction.Enable();
            }
        }
    }
    
    public virtual void Cleanup()
    {
        if (cachedAction != null)
        {
            cachedAction.Disable();
        }
    }
    
    public abstract Vector2? GetDirection();
}

[CreateAssetMenu(fileName = "ButtonInputConfig", menuName = "Input/ButtonInputConfig")]
public class ButtonInputConfig : InputConfig
{
    [Tooltip("Fixed direction for button press")]
    public Vector2 buttonDirection = Vector2.up;

    public override Vector2? GetDirection()
    {
        if (cachedAction == null) return null;

        float value = cachedAction.ReadValue<float>();
        return value > 0.1f ? buttonDirection : Vector2.zero;
    }

}

[CreateAssetMenu(fileName = "ValueInputConfig", menuName = "Input/ValueInputConfig")]
public class ValueInputConfig : InputConfig
{
    public override Vector2? GetDirection()
    {
        if (cachedAction == null) return null;
        
        string controlType = cachedAction.expectedControlType;
        
        if (controlType == "Vector2")
        {
            return cachedAction.ReadValue<Vector2>();
        }
        
        if (controlType == "Button" || controlType == "Axis")
        {
            float value = cachedAction.ReadValue<float>();
            return value > 0.1f ? Vector2.up : Vector2.zero;
        }
        
        return null;
    }
}


