
using UnityEngine;
using DG.Tweening;

public class MovementController : MonoBehaviour
{
    [Header("Setup")]
    public MovementDataSO movementData;
    public bool autoStart = true;

    [Header("Debug")]
    public bool showGizmos = true;
    public Color gizmoColor = Color.yellow;
    private Tween activeTween;
    private Rigidbody rb;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;
    }
    void Start()
    {
        if (autoStart && movementData != null)
        {
            StartMovement();
        }
    }
    public void StartMovement()
    {
        if (movementData == null)
        {
            Debug.LogWarning($"[{name}] MovementDataSO is not assigned!");
            return;
        }

        if (activeTween != null && activeTween.IsActive())
        {
            activeTween.Kill();
        }

        LoopType doTweenLoop = ConvertLoopType(movementData.loopType);

        Vector3 targetValue = CalculateTargetValue();

        switch (movementData.doType)
        {
            case DOType.Move:
                activeTween = DOTweenMover.Move(
                    transform, rb, targetValue,
                    movementData.duration, movementData.ease,
                    movementData.useRigidbody, movementData.useLocalSpace,
                    movementData.loopCount, doTweenLoop
                );
                break;

            case DOType.MoveX:
                activeTween = DOTweenMover.MoveX(
                    transform, rb, targetValue.x,
                    movementData.duration, movementData.ease,
                    movementData.useRigidbody,
                    movementData.loopCount, doTweenLoop
                );
                break;

            case DOType.MoveY:
                activeTween = DOTweenMover.MoveY(
                    transform, rb, targetValue.y,
                    movementData.duration, movementData.ease,
                    movementData.useRigidbody,
                    movementData.loopCount, doTweenLoop
                );
                break;

            case DOType.Jump:
                activeTween = DOTweenMover.Jump(
                    transform, rb, targetValue,
                    movementData.jumpPower, movementData.jumpCount,
                    movementData.duration, movementData.ease,
                    movementData.useRigidbody,
                    movementData.loopCount, doTweenLoop
                );
                break;

            case DOType.Rotate:
                activeTween = DOTweenMover.Rotate(
                    transform, targetValue,
                    movementData.duration, movementData.ease,
                    movementData.useLocalSpace,
                    movementData.loopCount, doTweenLoop
                );
                break;

            case DOType.Path:
                activeTween = DOTweenMover.Path(
                    transform, movementData.pathPoints,
                    movementData.duration, movementData.pathType,
                    movementData.pathMode, movementData.ease,
                    movementData.loopCount, doTweenLoop
                );
                break;

            case DOType.LocalPath:
                activeTween = DOTweenMover.LocalPath(
                    transform, movementData.pathPoints,
                    movementData.duration, movementData.pathType,
                    movementData.pathMode, movementData.ease,
                    movementData.loopCount, doTweenLoop
                );
                break;
        }


        activeTween.SetAutoKill(false);
    }
    Vector3 CalculateTargetValue()
    {
        if (!movementData.useRelativeValues)
            return movementData.targetValue;

        switch (movementData.doType)
        {
            case DOType.Move:
                return movementData.useLocalSpace
                    ? transform.localPosition + movementData.targetValue
                    : transform.position + movementData.targetValue;

            case DOType.MoveX:
                return movementData.useRigidbody
                    ? new Vector3(transform.position.x + movementData.targetValue.x, transform.position.y, transform.position.z)
                    : new Vector3(transform.localPosition.x + movementData.targetValue.x, transform.localPosition.y, transform.localPosition.z);

            case DOType.MoveY:
                return movementData.useRigidbody
                    ? new Vector3(transform.position.x, transform.position.y + movementData.targetValue.y, transform.position.z)
                    : new Vector3(transform.localPosition.x, transform.localPosition.y + movementData.targetValue.y, transform.localPosition.z);

            case DOType.Jump:
                return movementData.useLocalSpace
                    ? transform.localPosition + movementData.targetValue
                    : transform.position + movementData.targetValue;

            case DOType.Rotate:
                return movementData.useLocalSpace
                    ? transform.localEulerAngles + movementData.targetValue
                    : transform.eulerAngles + movementData.targetValue;

            case DOType.Path:
            case DOType.LocalPath:
                // path’ler zaten dizi olarak ayarlanıyor, burada relative offset uygulanmaz
                return Vector3.zero;

            default:
                return movementData.targetValue;
        }
    }

    // Vector3 CalculateTargetValue()
    // {
    //     if (movementData.useRelativeValues)
    //     {
    //         switch (movementData.movementType)
    //         {
    //             case MovementType.Linear:
    //                 return movementData.useLocalSpace ?
    //                     transform.localPosition + movementData.targetValue :
    //                     transform.position + movementData.targetValue;

    //             case MovementType.Angular:
    //                 return movementData.useLocalSpace ?
    //                     transform.localEulerAngles + movementData.targetValue :
    //                     transform.eulerAngles + movementData.targetValue;

    //             case MovementType.Scaler:
    //                 return transform.localScale + movementData.targetValue;

    //             default:
    //                 return movementData.targetValue;
    //         }
    //     }
    //     else
    //     {
    //         return movementData.targetValue;
    //     }
    // }
    public void StopMovement()
    {
        if (activeTween != null && activeTween.IsActive())
        {
            activeTween.Kill();
        }
    }
    public void PauseMovement()
    {
        if (activeTween != null && activeTween.IsActive())
        {
            activeTween.Pause();
        }
    }
    public void ResumeMovement()
    {
        if (activeTween != null && activeTween.IsActive())
        {
            activeTween.Play();
        }
    }
    public void ResetToStart()
    {
        StopMovement();
        transform.position = startPosition;
        transform.rotation = startRotation;
        transform.localScale = startScale;
    }
    LoopType ConvertLoopType(LoopTypeCustom customLoop)
    {
        return customLoop switch
        {
            LoopTypeCustom.None => LoopType.Restart,
            LoopTypeCustom.Loop => LoopType.Restart,
            LoopTypeCustom.PingPong => LoopType.Yoyo,
            _ => LoopType.Restart
        };
    }
    void OnDrawGizmos()
    {
        if (!showGizmos || movementData == null) return;

        Gizmos.color = gizmoColor;

        Vector3 startPos = Application.isPlaying ? startPosition : transform.position;
        Vector3 targetPos = startPos;

        switch (movementData.doType)
        {
            case DOType.Move:
            case DOType.Jump:
                targetPos = movementData.useRelativeValues
                    ? startPos + movementData.targetValue
                    : movementData.targetValue;

                Gizmos.DrawLine(startPos, targetPos);
                Gizmos.DrawWireSphere(startPos, 0.2f);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(targetPos, 0.2f);

                DrawArrow(startPos, targetPos);
                break;

            case DOType.MoveX:
                targetPos = startPos + new Vector3(movementData.targetValue.x, 0f, 0f);
                Gizmos.DrawLine(startPos, targetPos);
                Gizmos.DrawWireSphere(startPos, 0.2f);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(targetPos, 0.2f);
                DrawArrow(startPos, targetPos);
                break;

            case DOType.MoveY:
                targetPos = startPos + new Vector3(0f, movementData.targetValue.y, 0f);
                Gizmos.DrawLine(startPos, targetPos);
                Gizmos.DrawWireSphere(startPos, 0.2f);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(targetPos, 0.2f);
                DrawArrow(startPos, targetPos);
                break;

            case DOType.Path:
            case DOType.LocalPath:
                if (movementData.pathPoints != null && movementData.pathPoints.Length > 1)
                {
                    Vector3 prev = movementData.pathPoints[0] + (movementData.useRelativeValues ? startPos : Vector3.zero);
                    for (int i = 1; i < movementData.pathPoints.Length; i++)
                    {
                        Vector3 next = movementData.pathPoints[i] + (movementData.useRelativeValues ? startPos : Vector3.zero);
                        Gizmos.DrawLine(prev, next);
                        prev = next;
                    }
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(prev, 0.2f);
                }
                break;
        }
    }

    // void OnDrawGizmos()
    // {
    //     if (!showGizmos || movementData == null) return;

    //     Gizmos.color = gizmoColor;

    //     Vector3 startPos = Application.isPlaying ? startPosition : transform.position;
    //     Vector3 targetPos = startPos;

    //     if (movementData.movementType == MovementType.Linear)
    //     {
    //         if (movementData.useRelativeValues)
    //         {
    //             targetPos = startPos + movementData.targetValue;
    //         }
    //         else
    //         {
    //             targetPos = movementData.targetValue;
    //         }

    //         // Draw line
    //         Gizmos.DrawLine(startPos, targetPos);

    //         // Draw spheres
    //         Gizmos.DrawWireSphere(startPos, 0.2f);
    //         Gizmos.color = Color.green;
    //         Gizmos.DrawWireSphere(targetPos, 0.2f);

    //         // Draw arrow
    //         DrawArrow(startPos, targetPos);
    //     }
    // }
    void DrawArrow(Vector3 from, Vector3 to)
    {
        Vector3 direction = (to - from).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, direction).normalized * 0.2f;
        Vector3 arrowTip = to - direction * 0.3f;

        Gizmos.DrawLine(to, arrowTip + right);
        Gizmos.DrawLine(to, arrowTip - right);
    }
    void OnDestroy()
    {
        if (activeTween != null && activeTween.IsActive())
        {
            activeTween.Kill();
        }
    }
}


// using UnityEngine;
// using DG.Tweening;

// public class MovementController : MonoBehaviour
// {
//     public MovementDataSO movementData;
//     public bool isActive = true;

//     void Start()
//     {
//         if (!isActive || movementData == null) return;

//         switch (movementData.movementType)
//         {
//             case MovementType.Linear:
//                 DOTweenMover.MoveLinear(transform, movementData.moveOffset, movementData.duration, movementData.ease, ConvertLoopType(movementData.loopType));
//                 break;
//             case MovementType.Angular:
//                 DOTweenMover.RotateAngular(transform, movementData.moveOffset, movementData.duration, movementData.ease, ConvertLoopType(movementData.loopType));
//                 break;
//             case MovementType.Scaler:
//                 DOTweenMover.Scale(transform, movementData.moveOffset, movementData.duration, movementData.ease, ConvertLoopType(movementData.loopType));
//                 break;
//         }
//     }

//     LoopType ConvertLoopType(LoopTypeCustom loop)
//     {
//         return loop switch
//         {
//             LoopTypeCustom.None => LoopType.Restart, // tek sefer için DOTween default
//             LoopTypeCustom.Loop => LoopType.Restart,
//             LoopTypeCustom.PingPong => LoopType.Yoyo,
//             _ => LoopType.Restart
//         };
//     }
// }
