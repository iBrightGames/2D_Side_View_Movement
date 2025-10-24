// AnimationStateObserver.cs
using System.Collections.Generic;
using PlayerControlSystem;
using SpriteDatabaseAnimation;
using UnityEngine;

public class AnimationStateObserver : MonoBehaviour
{
    [SerializeField] PlayerMovementController movementController;
    [SerializeField] SpriteDatabaseAnimator spriteAnimator;

    // Mapping: State → Animation Category
    [System.Serializable]
    public class StateAnimationMap
    {
        public string stateName; // "IdleState", "WalkState", etc.
        public string animationCategory; // "Idle", "Walk", etc.
    }

    [SerializeField] List<StateAnimationMap> stateMappings;

    void Start()
    {
        // Subscribe to state changes
        movementController.stateMachine.OnActionChanged += HandleEnvironmentChanged;
        // Eğer ActionState change event'i de varsa ona da subscribe ol
    }

    void HandleEnvironmentChanged(ActionState actionState)
    {
        // State ismini al
        string stateName = actionState.GetType().Name;
        Debug.Log(stateName);
        // Mapping'den animation category bul
        var mapping = stateMappings.Find(m => m.stateName == stateName);
        if (mapping != null)
        {
            Debug.Log(mapping);
            spriteAnimator.SetCategory(mapping.animationCategory);
        }
    }

    void OnDestroy()
    {
        movementController.stateMachine.OnActionChanged -= HandleEnvironmentChanged;
    }
}
