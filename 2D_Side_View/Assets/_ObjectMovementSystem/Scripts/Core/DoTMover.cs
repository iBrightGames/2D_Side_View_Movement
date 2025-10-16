using UnityEngine;
using DG.Tweening;

public static class DOTweenMover
{
    public static void MoveLinear(Transform target, Vector3 offset, float duration, Ease ease, LoopType loopType = LoopType.Restart, TweenCallback onComplete = null)
    {
        var tween = target.DOMove(target.position + offset, duration).SetEase(ease).SetLoops(loopType == LoopType.Restart ? -1 : 0, loopType);
        if (onComplete != null) tween.OnComplete(onComplete);
    }

    public static void RotateAngular(Transform target, Vector3 offset, float duration, Ease ease, LoopType loopType = LoopType.Restart, TweenCallback onComplete = null)
    {
        var tween = target.DORotate(target.eulerAngles + offset, duration).SetEase(ease).SetLoops(loopType == LoopType.Restart ? -1 : 0, loopType);
        if (onComplete != null) tween.OnComplete(onComplete);
    }

    public static void Scale(Transform target, Vector3 offset, float duration, Ease ease, LoopType loopType = LoopType.Restart, TweenCallback onComplete = null)
    {
        var tween = target.DOScale(target.localScale + offset, duration).SetEase(ease).SetLoops(loopType == LoopType.Restart ? -1 : 0, loopType);
        if (onComplete != null) tween.OnComplete(onComplete);
    }
}

