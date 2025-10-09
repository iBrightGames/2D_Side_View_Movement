
using UnityEngine;



[System.Serializable]
public class InputMovementBridge
{
    public bool enabled = true;    
    public BaseInputSO inputSource;        
    public MovementForceSO movement;    
    public MovementCondition[] conditions = new MovementCondition[0];
    public MovementModifier[] modifiers = new MovementModifier[0];    
      
    public bool showDebugInfo = false;
    [System.NonSerialized] public bool isExecuting;
    [System.NonSerialized] public float lastExecutionTime = -1f;
    [System.NonSerialized] public int executionCount;
    
    public bool CanExecute(MovementController controller)
        {
        if (!enabled || inputSource == null || movement == null)
            return false;
        foreach (var condition in conditions)
        {
            if (condition != null && !condition.CanExecute(controller))
            {
                if (showDebugInfo)
                {
                    Debug.LogWarning($"[] Condition failed: {condition.GetFailureReason()}");
                    controller.OnConditionFailed?.Invoke(condition.GetFailureReason());
                }
                return false;
            }
        }
        
        return true;
    }
    
    public void MarkExecuted()
    {
        lastExecutionTime = Time.time;
        executionCount++;
        
        foreach (var condition in conditions)
        {
            if (condition is CooldownCondition cooldown)
                cooldown.MarkExecuted();
        }
    }
}




