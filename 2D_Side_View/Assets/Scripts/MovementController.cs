using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

#region Movement Controller

public class MovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Transform trans;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    
    [Header("Debug")]
    [SerializeField] private bool debugMode = false;
    
    [Header("Events")]
    public UnityEvent OnMovementStarted = new UnityEvent();
    public UnityEvent OnMovementCompleted = new UnityEvent();
    public UnityEvent OnMovementCancelled = new UnityEvent();
    public UnityEvent<string> OnConditionFailed = new UnityEvent<string>();
    
    // State tracking
    private Dictionary<MovementForceSO, float> movementHistory = new Dictionary<MovementForceSO, float>();
    private List<MovementModifier> activeModifiers = new List<MovementModifier>();
    private MovementForceSO currentMovement;
    
    private void Awake()
    {
        if (trans == null) trans = transform;
        if (rb2D == null) rb2D = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }
    
    public void ApplyForce(MovementForceSO force, Vector3 inputDirection, MovementModifier[] modifiers = null)
    {
        currentMovement = force;
        RecordMovementExecution(force);
        
        // Combine bridge modifiers with active modifiers
        var allModifiers = new List<MovementModifier>(activeModifiers);
        if (modifiers != null) allModifiers.AddRange(modifiers);
        
        MovementExecutor.ApplyForce(force, inputDirection, allModifiers.ToArray(), trans, rb2D);
    }
    
    // public void PlayFeedback(MovementFeedbackConfig feedback)
    // {
    //     // Animation
    //     if (animator != null && !string.IsNullOrEmpty(feedback.animationTrigger))
    //         animator.SetTrigger(feedback.animationTrigger);
        
    //     // Audio
    //     if (audioSource != null && feedback.startSound != null)
    //         audioSource.PlayOneShot(feedback.startSound, feedback.volume);
        
    //     // VFX
    //     if (feedback.startVFX != null)
    //     {
    //         Vector3 spawnPos = feedback.attachVFXToTransform ? transform.position : feedback.startVFX.transform.position;
    //         Instantiate(feedback.startVFX, spawnPos, Quaternion.identity, feedback.attachVFXToTransform ? transform : null);
    //     }
        
    //     // Camera shake (if you have a camera shake system)
    //     if (feedback.enableCameraShake)
    //     {
    //         // Call your camera shake here
    //         // CameraShake.Instance?.Shake(feedback.shakeIntensity, feedback.shakeDuration);
    //     }
    // }
    
    public Vector2 GetVelocity()
    {
        return rb2D != null ? rb2D.linearVelocity : Vector2.zero;
    }
    
    public float GetTimeSinceMovement(MovementForceSO movement)
    {
        return movementHistory.TryGetValue(movement, out float time) ? Time.time - time : -1f;
    }
    
    public void AddModifier(MovementModifier modifier)
    {
        if (!activeModifiers.Contains(modifier))
        {
            activeModifiers.Add(modifier);
            
            if (modifier.modifierDuration > 0)
                StartCoroutine(RemoveModifierAfterDelay(modifier, modifier.modifierDuration));
        }
    }
    
    public void RemoveModifier(MovementModifier modifier)
    {
        activeModifiers.Remove(modifier);
    }
    
    private void RecordMovementExecution(MovementForceSO movement)
    {
        movementHistory[movement] = Time.time;
    }
    
    private IEnumerator RemoveModifierAfterDelay(MovementModifier modifier, float delay)
    {
        yield return new WaitForSeconds(delay);
        RemoveModifier(modifier);
    }
    
    private void OnDrawGizmos()
    {
        if (!debugMode) return;
        
        // Draw velocity
        if (rb2D != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)rb2D.linearVelocity * 0.5f);
        }
    }
}

#endregion
