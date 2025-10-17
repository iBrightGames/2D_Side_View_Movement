
using UnityEngine;
using DG.Tweening;


public static class DOTweenMover
{
    // MOVE (XYZ)
    public static Tween Move(Rigidbody2D rb, Vector3 targetPos, float duration,
                             Ease ease, int loopCount, LoopType loopType)
    {
        Tween tween;
        tween = rb.DOMove(targetPos, duration);
        tween.SetEase(ease);
        if (loopCount != 0) tween.SetLoops(loopCount, loopType);
        return tween;
    }

    // MOVE X
    public static Tween MoveX(Rigidbody2D rb, float targetX, float duration,
                              Ease ease, int loopCount, LoopType loopType)
    {
        Tween tween;
        tween = rb.DOMoveX(targetX, duration);
        tween.SetEase(ease);
        if (loopCount != 0) tween.SetLoops(loopCount, loopType);
        return tween;
    }

    // MOVE Y
    public static Tween MoveY(Rigidbody2D rb, float targetY, float duration,
                              Ease ease, int loopCount, LoopType loopType)
    {
        Tween tween;
        tween = rb.DOMoveY(targetY, duration);
        tween.SetEase(ease);
        if (loopCount != 0) tween.SetLoops(loopCount, loopType);
        return tween;
    }

    // JUMP
    public static Tween Jump(Rigidbody2D rb, Vector3 targetPos, float jumpPower,
                             int numJumps, float duration, Ease ease,
                             int loopCount, LoopType loopType)
    {
        Tween tween;
        tween = rb.DOJump(targetPos, jumpPower, numJumps, duration);
        tween.SetEase(ease);
        if (loopCount != 0) tween.SetLoops(loopCount, loopType);
        return tween;
    }

    // ROTATE
    public static Tween Rotate(Rigidbody2D rb, float toAngle, float duration,
                               Ease ease, int loopCount, LoopType loopType)
    {
        Tween tween;
        tween = rb.DORotate(toAngle, duration);
        tween.SetEase(ease);
        if (loopCount != 0) tween.SetLoops(loopCount, loopType);
        return tween;
    }

    // PATH (world)
    public static Tween Path(Rigidbody2D rb, Vector2[] path, float duration,
                             PathType pathType, PathMode pathMode, Ease ease,
                             int loopCount, LoopType loopType)
    {
        Tween tween = rb.DOPath(path, duration, pathType, pathMode);
        tween.SetEase(ease);
        if (loopCount != 0) tween.SetLoops(loopCount, loopType);
        return tween;
    }

    // LOCAL PATH
    public static Tween LocalPath(Rigidbody2D rb, Vector2[] path, float duration,
                                  PathType pathType, PathMode pathMode, Ease ease,
                                  int loopCount, LoopType loopType)
    {
        Tween tween = rb.DOLocalPath(path, duration, pathType, pathMode);
        tween.SetEase(ease);
        if (loopCount != 0) tween.SetLoops(loopCount, loopType);
        return tween;
    }



}
