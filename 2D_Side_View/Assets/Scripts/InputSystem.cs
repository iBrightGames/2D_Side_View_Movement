using UnityEngine;
using UnityEngine.InputSystem;

#region Input System

public abstract class BaseInputSO : ScriptableObject
{    
    [Header("Input Buffer")]
    [Tooltip("Store input for this duration if can't execute immediately")]
    [Range(0f, 0.5f)]
    public float bufferWindow = 0.15f;
    [System.NonSerialized] protected float bufferedInputTime = -1f;
    
    public abstract bool ShouldExecute();
    public abstract bool IsHeld();
    public abstract bool WasReleasedThisFrame();
    
    public bool HasBufferedInput()
    {
        return Time.time - bufferedInputTime < bufferWindow;
    }
    
    public void BufferInput()
    {
        bufferedInputTime = Time.time;
    }
    
    public void ClearBuffer()
    {
        bufferedInputTime = -1f;
    }
    // Helper methods
    public abstract void Trigger();
    public abstract void Activate();
    public abstract void Deactivate();
    public abstract void Toggle();

    public abstract Vector3 GetDirection();
}

[CreateAssetMenu(fileName = "ExternalInput", menuName = "Movement/Input/External Input")]
public class ExternalInputSO : BaseInputSO
{
    [Header("Input Action")]
    public InputActionReference inputAction;
    
    [Header("Sensitivity")]
    [Range(0.01f, 1f)]
    public float deadzone = 0.1f;
    
    private InputAction _cachedAction;
    private InputAction Action
    {
        get
        {
            if (_cachedAction == null && inputAction != null)
                _cachedAction = inputAction.action;
            return _cachedAction;
        }
    }
    
    public override bool ShouldExecute()
    {
        if (Action == null) return false;
        
        switch (Action.type)
        {
            case InputActionType.Button:
                if (Action.triggered)
                {
                    BufferInput();
                    return true;
                }
                return HasBufferedInput();
            
            case InputActionType.Value:
                bool hasInput = GetInputMagnitude() > deadzone;
                if (hasInput) BufferInput();
                return hasInput || HasBufferedInput();
            
            default:
                return false;
        }
    }
    
    public override bool IsHeld()
    {
        if (Action == null) return false;
        return Action.phase == InputActionPhase.Performed || 
               Action.phase == InputActionPhase.Started;
    }
    
    public override bool WasReleasedThisFrame()
    {
        if (Action == null) return false;
        return Action.phase == InputActionPhase.Canceled;
    }
    
    public Vector3 GetInputDirection()
    {
        if (Action == null) return Vector3.zero;
        
        if (Action.expectedControlType == "Vector2")
        {
            Vector2 input = Action.ReadValue<Vector2>();
            return new Vector3(input.x, 0, input.y);
        }
        else
        {
            float input = Action.ReadValue<float>();
            return new Vector3(input, 0, 0);
        }
    }

    public float GetInputMagnitude()=>GetInputDirection().magnitude;

    public override void Trigger()
    {
        throw new System.NotImplementedException();
    }

    public override void Activate()
    {
        throw new System.NotImplementedException();
    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }

    public override void Toggle()
    {
        throw new System.NotImplementedException();
    }

    public override Vector3 GetDirection()
    {
        throw new System.NotImplementedException();
    }



    // Helper methods

}

[CreateAssetMenu(fileName = "InternalInput", menuName = "Movement/Input/Internal Input")]
public class InternalInputSO : BaseInputSO
{
    [Header("Runtime Control")]
    [Tooltip("For continuous execution")]
    public bool isActive = false;
    
    [Tooltip("For one-shot execution (auto-resets)")]
    public bool triggerOnce = false;
    
    
    public override bool ShouldExecute()
    {
        if (triggerOnce)
        {
            triggerOnce = false;
            ClearBuffer();
            return true;
        }
        return isActive;
    }

    public override bool IsHeld() => isActive;


    public override bool WasReleasedThisFrame() => false;
    
    // Helper methods
    public override void Trigger() => triggerOnce = true;
    public override void Activate() => isActive = true;
    public override void Deactivate() => isActive = false;
    public override void Toggle() => isActive = !isActive;

    public override Vector3 GetDirection()
    {
        throw new System.NotImplementedException();
    }
}

#endregion
