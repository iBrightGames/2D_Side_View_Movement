using UnityEngine;
using DG.Tweening;

public enum MovementType { Linear, Angular, Scaler }
public enum LoopTypeCustom { None, Loop, PingPong }

[CreateAssetMenu(fileName = "MovementData", menuName = "Movement/MovementDataSO")]
public class MovementDataSO : ScriptableObject
{
    public Vector3 moveOffset = Vector3.up;
    public float duration = 1f;
    public Ease ease = Ease.Linear;
    public LoopTypeCustom loopType = LoopTypeCustom.None;
    public MovementType movementType = MovementType.Linear;
}

