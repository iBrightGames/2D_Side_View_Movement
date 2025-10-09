using UnityEngine;

#region Movement Modifiers

[CreateAssetMenu(fileName = "MovementModifier", menuName = "Movement/Modifier")]
public class MovementModifier : ScriptableObject
{
    [Header("Modifier Info")]
    public string modifierName = "Speed Boost";
    
    [Header("Force Modifiers")]
    [Tooltip("Multiply force/speed")]
    public float forceMultiplier = 1.5f;
    
    [Tooltip("Add flat force")]
    public Vector3 forceAddition = Vector3.zero;
    
    [Header("Duration Modifiers")]
    public float durationMultiplier = 1.0f;
    
    [Header("This Modifier's Duration")]
    [Tooltip("How long this buff lasts (0 = permanent)")]
    public float modifierDuration = 5f;
    
    [Header("Visual")]
    public Color tintColor = Color.cyan;
    public GameObject visualEffect;
    
    public Vector3 ApplyToDirection(Vector3 original)
    {
        return (original * forceMultiplier) + forceAddition;
    }
    
    public float ApplyToDuration(float original)
    {
        return original * durationMultiplier;
    }
}

#endregion
