using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

#region Input System

public abstract class BaseInputSO : ScriptableObject
{
    [Header("Input Info")]
    public string inputName = "Unnamed Input";
    
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
    
    public float GetInputMagnitude()
    {
        return GetInputDirection().magnitude;
    }
}

[CreateAssetMenu(fileName = "InternalInput", menuName = "Movement/Input/Internal Input")]
public class InternalInputSO : BaseInputSO
{
    [Header("Runtime Control")]
    [Tooltip("For continuous execution")]
    public bool isActive = false;
    
    [Tooltip("For one-shot execution (auto-resets)")]
    public bool triggerOnce = false;
    
    [Header("AI Direction (Optional)")]
    public Vector3 scriptedDirection = Vector3.zero;
    
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
    
    public override bool IsHeld()
    {
        return isActive;
    }
    
    public override bool WasReleasedThisFrame()
    {
        return false;
    }
    
    // Helper methods
    public void Trigger() => triggerOnce = true;
    public void Activate() => isActive = true;
    public void Deactivate() => isActive = false;
    public void Toggle() => isActive = !isActive;
    
    public Vector3 GetDirection() => scriptedDirection;
    public void SetDirection(Vector3 dir) => scriptedDirection = dir;
}

#endregion
