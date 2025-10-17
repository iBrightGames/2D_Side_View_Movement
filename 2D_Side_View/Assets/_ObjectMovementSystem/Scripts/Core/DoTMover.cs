using UnityEngine;
using DG.Tweening;

public static class DOTweenMover
{
    // MOVE (XY) - 2D
    public static Tween Move(Rigidbody2D rb, Vector3 targetPos, float duration,
                             Ease ease, int loopCount, LoopType loopType)
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is null!");
            return null;
        }

        Tween tween = rb.DOMove(targetPos, duration);
        return ConfigureTween(tween, ease, loopCount, loopType);
    }

    // MOVE X - 2D
    public static Tween MoveX(Rigidbody2D rb, float targetX, float duration,
                              Ease ease, int loopCount, LoopType loopType)
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is null!");
            return null;
        }

        Tween tween = rb.DOMoveX(targetX, duration);
        return ConfigureTween(tween, ease, loopCount, loopType);
    }

    // MOVE Y - 2D
    public static Tween MoveY(Rigidbody2D rb, float targetY, float duration,
                              Ease ease, int loopCount, LoopType loopType)
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is null!");
            return null;
        }

        Tween tween = rb.DOMoveY(targetY, duration);
        return ConfigureTween(tween, ease, loopCount, loopType);
    }

    // JUMP - 2D
    public static Tween Jump(Rigidbody2D rb, Vector3 targetPos, float jumpPower,
                             int numJumps, float duration, Ease ease,
                             int loopCount, LoopType loopType)
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is null!");
            return null;
        }

        Tween tween = rb.DOJump(targetPos, jumpPower, numJumps, duration);
        return ConfigureTween(tween, ease, loopCount, loopType);
    }

    // ROTATE - 2D (uses Z axis rotation)
    public static Tween Rotate(Rigidbody2D rb, float targetAngle, float duration,
                               Ease ease, int loopCount, LoopType loopType)
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is null!");
            return null;
        }

        Tween tween = rb.DORotate(targetAngle, duration);
        return ConfigureTween(tween, ease, loopCount, loopType);
    }

    // SCALE - Works on Transform (not Rigidbody2D)
    public static Tween Scale(Transform target, Vector3 targetScale, float duration,
                              Ease ease, int loopCount, LoopType loopType)
    {
        if (target == null)
        {
            Debug.LogError("Transform is null!");
            return null;
        }

        Tween tween = target.DOScale(targetScale, duration);
        return ConfigureTween(tween, ease, loopCount, loopType);
    }

    // PATH (world space) - 2D
    public static Tween Path(Rigidbody2D rb, Vector2[] path, float duration,
                             PathType pathType, PathMode pathMode, Ease ease,
                             int loopCount, LoopType loopType)
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is null!");
            return null;
        }

        if (path == null || path.Length < 2)
        {
            Debug.LogWarning("Path requires at least 2 points");
            return null;
        }

        Tween tween = rb.DOPath(path, duration, pathType, pathMode);
        return ConfigureTween(tween, ease, loopCount, loopType);
    }

    // LOCAL PATH - 2D
    public static Tween LocalPath(Rigidbody2D rb, Vector2[] path, float duration,
                                  PathType pathType, PathMode pathMode, Ease ease,
                                  int loopCount, LoopType loopType)
    {
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is null!");
            return null;
        }

        if (path == null || path.Length < 2)
        {
            Debug.LogWarning("Path requires at least 2 points");
            return null;
        }

        Tween tween = rb.DOLocalPath(path, duration, pathType, pathMode);
        return ConfigureTween(tween, ease, loopCount, loopType);
    }

    // Helper method to configure tween properties
    private static Tween ConfigureTween(Tween tween, Ease ease, int loopCount, LoopType loopType)
    {
        if (tween == null) return null;

        tween.SetEase(ease);

        // Handle loop count: 0 = no loop (play once), -1 = infinite, positive = specific count
        if (loopCount != 0)
        {
            tween.SetLoops(loopCount, loopType);
        }

        return tween;
    }

}

// using UnityEngine;
// using DG.Tweening;


// public static class DOTweenMover
// {
//     // MOVE (XYZ)
//     public static Tween Move(Rigidbody2D rb, Vector3 targetPos, float duration,
//                              Ease ease, int loopCount, LoopType loopType)
//     {
//         Tween tween;
//         tween = rb.DOMove(targetPos, duration);
//         tween.SetEase(ease);
//         if (loopCount != 0) tween.SetLoops(loopCount, loopType);
//         return tween;
//     }

//     // MOVE X
//     public static Tween MoveX(Rigidbody2D rb, float targetX, float duration,
//                               Ease ease, int loopCount, LoopType loopType)
//     {
//         Tween tween;
//         tween = rb.DOMoveX(targetX, duration);
//         tween.SetEase(ease);
//         if (loopCount != 0) tween.SetLoops(loopCount, loopType);
//         return tween;
//     }

//     // MOVE Y
//     public static Tween MoveY(Rigidbody2D rb, float targetY, float duration,
//                               Ease ease, int loopCount, LoopType loopType)
//     {
//         Tween tween;
//         tween = rb.DOMoveY(targetY, duration);
//         tween.SetEase(ease);
//         if (loopCount != 0) tween.SetLoops(loopCount, loopType);
//         return tween;
//     }

//     // JUMP
//     public static Tween Jump(Rigidbody2D rb, Vector3 targetPos, float jumpPower,
//                              int numJumps, float duration, Ease ease,
//                              int loopCount, LoopType loopType)
//     {
//         Tween tween;
//         tween = rb.DOJump(targetPos, jumpPower, numJumps, duration);
//         tween.SetEase(ease);
//         if (loopCount != 0) tween.SetLoops(loopCount, loopType);
//         return tween;
//     }

//     // ROTATE
//     public static Tween Rotate(Rigidbody2D rb, float toAngle, float duration,
//                                Ease ease, int loopCount, LoopType loopType)
//     {
//         Tween tween;
//         tween = rb.DORotate(toAngle, duration);
//         tween.SetEase(ease);
//         if (loopCount != 0) tween.SetLoops(loopCount, loopType);
//         return tween;
//     }

//     // PATH (world)
//     public static Tween Path(Rigidbody2D rb, Vector2[] path, float duration,
//                              PathType pathType, PathMode pathMode, Ease ease,
//                              int loopCount, LoopType loopType)
//     {
//         Tween tween = rb.DOPath(path, duration, pathType, pathMode);
//         tween.SetEase(ease);
//         if (loopCount != 0) tween.SetLoops(loopCount, loopType);
//         return tween;
//     }

//     // LOCAL PATH
//     public static Tween LocalPath(Rigidbody2D rb, Vector2[] path, float duration,
//                                   PathType pathType, PathMode pathMode, Ease ease,
//                                   int loopCount, LoopType loopType)
//     {
//         Tween tween = rb.DOLocalPath(path, duration, pathType, pathMode);
//         tween.SetEase(ease);
//         if (loopCount != 0) tween.SetLoops(loopCount, loopType);
//         return tween;
//     }



// }
