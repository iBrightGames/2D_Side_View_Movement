using UnityEngine;

#region Movement Data Structures

[System.Serializable]
public struct MovementForce
{
    public TargetType targetType;
    public ForceType forceType;
    public Vector3 direction;
    [Range(0, 10)] public float duration;
    public bool isContinuous;

}

public enum TargetType { Rigidbody2D, Transform }
public enum ForceType { Linear, Angular, Scaler }

#endregion

