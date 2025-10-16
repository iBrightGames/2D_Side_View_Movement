using UnityEngine;
using DG.Tweening;

public class MovementController : MonoBehaviour
{
    public MovementDataSO movementData;
    public bool isActive = true;

    void Start()
    {
        if (!isActive || movementData == null) return;

        switch (movementData.movementType)
        {
            case MovementType.Linear:
                DOTweenMover.MoveLinear(transform, movementData.moveOffset, movementData.duration, movementData.ease, ConvertLoopType(movementData.loopType));
                break;
            case MovementType.Angular:
                DOTweenMover.RotateAngular(transform, movementData.moveOffset, movementData.duration, movementData.ease, ConvertLoopType(movementData.loopType));
                break;
            case MovementType.Scaler:
                DOTweenMover.Scale(transform, movementData.moveOffset, movementData.duration, movementData.ease, ConvertLoopType(movementData.loopType));
                break;
        }
    }

    LoopType ConvertLoopType(LoopTypeCustom loop)
    {
        return loop switch
        {
            LoopTypeCustom.None => LoopType.Restart, // tek sefer için DOTween default
            LoopTypeCustom.Loop => LoopType.Restart,
            LoopTypeCustom.PingPong => LoopType.Yoyo,
            _ => LoopType.Restart
        };
    }
}
