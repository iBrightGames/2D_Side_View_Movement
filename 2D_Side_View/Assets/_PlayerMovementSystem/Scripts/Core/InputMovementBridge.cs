// ============================================
// BRIDGE
// ============================================

using UnityEngine;

[System.Serializable]
public class InputMovementBridge
{
    public InputConfig inputConfig;
    public ForceConfig movementConfig;
    
    [Header("Conditions")]
    public bool requiresGrounded;
    public float cooldown;
    
    [HideInInspector] public float lastExecutionTime;
    
    public bool CanExecute(bool isGrounded)
    {
        // Cooldown kontrolü
        if (cooldown > 0f && Time.time - lastExecutionTime < cooldown)
        {
            return false;
        }
        
        // Ground kontrolü
        if (requiresGrounded && !isGrounded)
        {
            return false;
        }
        
        return true;
    }
    
    public void MarkExecuted()
    {
        lastExecutionTime = Time.time;
    }
}



