using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "MovementData", menuName = "ScriptableObjects/MovementDataSO")]
public class MovementDataSO : ScriptableObject
{
    [Header("Movement")]
    public DOType doType;

    [Tooltip("For Move/Rotate: target position or rotation\nFor Jump: end position\nFor Scale: scale multiplier")]
    public Vector3 targetValue = Vector3.up;

    [Header("Jump Settings")]
    public float jumpPower = 2f;
    public int jumpCount = 1;

    [Header("Path Settings")]
    public Vector3[] pathPoints;
    public PathType pathType = PathType.Linear;
    public PathMode pathMode = PathMode.Full3D;

    [Header("Timing")]
    public float duration = 1f;
    public Ease ease = Ease.Linear;

    [Header("Loop")]
    public LoopTypeCustom loopType = LoopTypeCustom.Loop;
    public int loopCount = -1;

    [Header("Options")]
    public bool useRelativeValues = true;
    public bool useRigidbody = true;
    public bool useLocalSpace = false;
}

// using UnityEngine;
// using DG.Tweening;

// public enum MovementType { Linear, Angular, Scaler }
// public enum LoopTypeCustom { None, Loop, PingPong }

// [CreateAssetMenu(fileName = "MovementData", menuName = "Movement/MovementDataSO")]
// public class MovementDataSO : ScriptableObject
// {
//     public Vector3 moveOffset = Vector3.up;
//     public float duration = 1f;
//     public Ease ease = Ease.Linear;
//     public LoopTypeCustom loopType = LoopTypeCustom.None;
//     public MovementType movementType = MovementType.Linear;
// }

