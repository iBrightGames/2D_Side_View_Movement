using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "MovementData", menuName = "ScriptableObjects/MovementDataSO")]
public class MovementDataSO : ScriptableObject
{
    [Header("Movement")]
    public DOType doType;

    [Tooltip("For Move/Rotate: target position or rotation\nFor Jump: end position\nFor Scale: scale multiplier")]
    public Vector3 targetValue = Vector3.up;
    public RotateMode rotateMode;

    [Header("Jump Settings")]
    public float jumpPower = 2f;
    public int jumpCount = 1;

    [Header("Path Settings")]
    public Vector2[] pathPoints;
    public PathType pathType = PathType.Linear;
    public PathMode pathMode = PathMode.Full3D;

    [Header("Timing")]
    public float duration = 1f;
    public Ease ease = Ease.Linear;

    [Header("Loop")]
    public LoopTypeCustom loopType = LoopTypeCustom.Loop;
    public int loopCount = -1;

    // [Header("Options")]
    // public bool useRelativeValues = true;
    // public bool useRigidbody = true;
    // public bool useLocalSpace = false;
}
