
using UnityEngine;

#region Core Configuration

[System.Serializable]
public class InputMovementBridge
{
    [Header("Identification")]
    public string bridgeName = "New Movement";
    public bool enabled = true;
    
    [Header("Core Components")]
    public BaseInputSO inputSource;
    public MovementForceSO movement;
    
    [Header("Requirements (All Must Pass)")]
    [Tooltip("Movement only executes if ALL conditions pass")]
    public MovementCondition[] conditions = new MovementCondition[0];
    
    [Header("Modifiers (Optional)")]
    [Tooltip("Applied to movement force/duration")]
    public MovementModifier[] modifiers = new MovementModifier[0];
    
    [Header("Feedback (Optional)")]
    public MovementFeedbackConfig feedback = new MovementFeedbackConfig();
    
    [Header("Debug Info")]
    public bool showDebugInfo = false;
    
    // Runtime state (not serialized)
    [System.NonSerialized] public bool isExecuting;
    [System.NonSerialized] public float lastExecutionTime = -1f;
    [System.NonSerialized] public int executionCount;
    
    public bool CanExecute(MovementController controller)
    {
        if (!enabled || inputSource == null || movement == null)
            return false;
        
        // Check all conditions
        foreach (var condition in conditions)
        {
            if (condition != null && !condition.CanExecute(controller))
            {
                if (showDebugInfo)
                {
                    Debug.LogWarning($"[{bridgeName}] Condition failed: {condition.GetFailureReason()}");
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
        
        // Mark cooldown conditions as executed
        foreach (var condition in conditions)
        {
            if (condition is CooldownCondition cooldown)
                cooldown.MarkExecuted();
        }
    }
}

[System.Serializable]
public class MovementFeedbackConfig
{
    [Header("Animation")]
    public string animationTrigger = "";
    public bool lockToAnimationDuration = false;
    
    [Header("Audio")]
    public AudioClip startSound;
    public AudioClip loopSound;
    public AudioClip endSound;
    [Range(0f, 1f)] public float volume = 1f;
    
    [Header("Visual Effects")]
    public GameObject startVFX;
    public GameObject loopVFX;
    public GameObject endVFX;
    public bool attachVFXToTransform = true;
    
    [Header("Screen Effect")]
    public bool enableCameraShake = false;
    [Range(0f, 1f)] public float shakeIntensity = 0.1f;
    [Range(0f, 1f)] public float shakeDuration = 0.2f;
}

#endregion

