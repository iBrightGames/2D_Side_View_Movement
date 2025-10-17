// ============================================
// DOTWEEN MOVER (Fixed)
// ============================================

using UnityEngine;
using DG.Tweening;

public static class DOTweenMover
{
    // MOVE (XYZ)
    public static Tween Move(Transform target, Rigidbody rb, Vector3 targetPos, float duration,
                             Ease ease, bool useRigidbody, bool useLocalSpace,
                             int loopCount, LoopType loopType)
    {
        Tween tween;

        if (useRigidbody && rb != null)
            tween = rb.DOMove(targetPos, duration);
        else if (useLocalSpace)
            tween = target.DOLocalMove(targetPos, duration);
        else
            tween = target.DOMove(targetPos, duration);

        tween.SetEase(ease);
        if (loopCount != 0) tween.SetLoops(loopCount, loopType);
        return tween;
    }

    // MOVE X
    public static Tween MoveX(Transform target, Rigidbody rb, float targetX, float duration,
                              Ease ease, bool useRigidbody,
                              int loopCount, LoopType loopType)
    {
        Tween tween;

        if (useRigidbody && rb != null)
            tween = rb.DOMoveX(targetX, duration);
        else
            tween = target.DOMoveX(targetX, duration);

        tween.SetEase(ease);
        if (loopCount != 0) tween.SetLoops(loopCount, loopType);
        return tween;
    }

    // MOVE Y
    public static Tween MoveY(Transform target, Rigidbody rb, float targetY, float duration,
                              Ease ease, bool useRigidbody,
                              int loopCount, LoopType loopType)
    {
        Tween tween;

        if (useRigidbody && rb != null)
            tween = rb.DOMoveY(targetY, duration);
        else
            tween = target.DOMoveY(targetY, duration);

        tween.SetEase(ease);
        if (loopCount != 0) tween.SetLoops(loopCount, loopType);
        return tween;
    }

    // JUMP
    public static Tween Jump(Transform target, Rigidbody rb, Vector3 targetPos, float jumpPower,
                             int numJumps, float duration, Ease ease, bool useRigidbody,
                             int loopCount, LoopType loopType)
    {
        Tween tween;

        if (useRigidbody && rb != null)
            tween = rb.DOJump(targetPos, jumpPower, numJumps, duration);
        else
            tween = target.DOJump(targetPos, jumpPower, numJumps, duration);

        tween.SetEase(ease);
        if (loopCount != 0) tween.SetLoops(loopCount, loopType);
        return tween;
    }

    // ROTATE
    public static Tween Rotate(Transform target, Vector3 targetRotation, float duration,
                               Ease ease, bool useLocalSpace,
                               int loopCount, LoopType loopType)
    {
        Tween tween;

        if (useLocalSpace)
            tween = target.DOLocalRotate(targetRotation, duration);
        else
            tween = target.DORotate(targetRotation, duration);

        tween.SetEase(ease);
        if (loopCount != 0) tween.SetLoops(loopCount, loopType);
        return tween;
    }

    // PATH (world)
    public static Tween Path(Transform target, Vector3[] path, float duration,
                             PathType pathType, PathMode pathMode, Ease ease,
                             int loopCount, LoopType loopType)
    {
        Tween tween = target.DOPath(path, duration, pathType, pathMode);
        tween.SetEase(ease);
        if (loopCount != 0) tween.SetLoops(loopCount, loopType);
        return tween;
    }

    // LOCAL PATH
    public static Tween LocalPath(Transform target, Vector3[] path, float duration,
                                  PathType pathType, PathMode pathMode, Ease ease,
                                  int loopCount, LoopType loopType)
    {
        Tween tween = target.DOLocalPath(path, duration, pathType, pathMode);
        tween.SetEase(ease);
        if (loopCount != 0) tween.SetLoops(loopCount, loopType);
        return tween;
    }



    // // SCALE
    // public static Tween Scale(Transform target, Vector3 targetScale, float duration,
    //                          Ease ease, int loopCount, LoopType loopType)
    // {
    //     Tween tween = target.DOScale(targetScale, duration);
    //     tween.SetEase(ease);

    //     if (loopCount != 0)
    //     {
    //         tween.SetLoops(loopCount, loopType);
    //     }

    //     return tween;
    // }

}

// using UnityEngine;
// using DG.Tweening;

// public static class DOTweenMover
// {
//     public static void MoveLinear(Transform target, Vector3 offset, float duration, Ease ease, LoopType loopType = LoopType.Restart, TweenCallback onComplete = null)
//     {
//         var tween = target.DOMove(target.position + offset, duration).SetEase(ease).SetLoops(loopType == LoopType.Restart ? -1 : 0, loopType);
//         if (onComplete != null) tween.OnComplete(onComplete);
//     }

//     public static void RotateAngular(Transform target, Vector3 offset, float duration, Ease ease, LoopType loopType = LoopType.Restart, TweenCallback onComplete = null)
//     {
//         var tween = target.DORotate(target.eulerAngles + offset, duration).SetEase(ease).SetLoops(loopType == LoopType.Restart ? -1 : 0, loopType);
//         if (onComplete != null) tween.OnComplete(onComplete);
//     }

//     public static void Scale(Transform target, Vector3 offset, float duration, Ease ease, LoopType loopType = LoopType.Restart, TweenCallback onComplete = null)
//     {
//         var tween = target.DOScale(target.localScale + offset, duration).SetEase(ease).SetLoops(loopType == LoopType.Restart ? -1 : 0, loopType);
//         if (onComplete != null) tween.OnComplete(onComplete);
//     }
// }

