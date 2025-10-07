using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;


#region Movement Controller

public class MovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Transform _transform;

    [Header("Events")]
    public UnityEvent OnMovementStarted = new UnityEvent();
    public UnityEvent OnMovementCompleted = new UnityEvent();

    private void Awake()
    {
        if (_transform == null) _transform = transform;
        if (_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void ApplyForce(MovementForceSO force, Vector3 inputDirection = default)
    {
        MovementExecutor.ApplyForce(force, inputDirection, _transform, _rigidbody2D);
    }

    public Coroutine StartTriggeredMovement(TriggeredRigidbodyMovement movement, Vector3 inputDirection)
    {
        return StartCoroutine(ExecuteTriggeredRigidbody(movement, inputDirection));
    }

    public Coroutine StartTriggeredMovement(TriggeredTransformMovement movement, Vector3 inputDirection)
    {
        return StartCoroutine(ExecuteTriggeredTransform(movement, inputDirection));
    }

    private IEnumerator ExecuteTriggeredRigidbody(TriggeredRigidbodyMovement movement, Vector3 inputDirection)
    {
        OnMovementStarted?.Invoke();
        
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        switch (movement.CompletionType)
        {
            case CompletionType.Time:
                while (elapsed < movement.CompletionValue)
                {
                    ApplyForce(movement, inputDirection);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                break;

            case CompletionType.Rotation:
                float targetAngle = movement.CompletionValue;
                float rotated = 0f;
                Quaternion lastRot = transform.rotation;
                
                while (Mathf.Abs(rotated) < Mathf.Abs(targetAngle))
                {
                    ApplyForce(movement, inputDirection);
                    float deltaAngle = Quaternion.Angle(lastRot, transform.rotation);
                    rotated += deltaAngle;
                    lastRot = transform.rotation;
                    yield return null;
                }
                break;

            case CompletionType.Distance:
                float targetDist = movement.CompletionValue;
                float traveled = 0f;
                
                while (traveled < targetDist)
                {
                    Vector3 lastPos = transform.position;
                    ApplyForce(movement, inputDirection);
                    traveled += Vector3.Distance(lastPos, transform.position);
                    yield return null;
                }
                break;

            default:
                ApplyForce(movement, inputDirection);
                break;
        }

        OnMovementCompleted?.Invoke();
    }

    private IEnumerator ExecuteTriggeredTransform(TriggeredTransformMovement movement, Vector3 inputDirection)
    {
        OnMovementStarted?.Invoke();
        
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        switch (movement.CompletionType)
        {
            case CompletionType.Time:
                while (elapsed < movement.CompletionValue)
                {
                    ApplyForce(movement, inputDirection);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
                break;

            case CompletionType.Rotation:
                float targetAngle = movement.CompletionValue;
                float rotated = 0f;
                Quaternion lastRot = transform.rotation;
                
                while (Mathf.Abs(rotated) < Mathf.Abs(targetAngle))
                {
                    ApplyForce(movement, inputDirection);
                    float deltaAngle = Quaternion.Angle(lastRot, transform.rotation);
                    rotated += deltaAngle;
                    lastRot = transform.rotation;
                    yield return null;
                }
                break;

            case CompletionType.Distance:
                float targetDist = movement.CompletionValue;
                while (Vector3.Distance(startPos, transform.position) < targetDist)
                {
                    ApplyForce(movement, inputDirection);
                    yield return null;
                }
                break;

            default:
                ApplyForce(movement, inputDirection);
                break;
        }

        OnMovementCompleted?.Invoke();
    }
}

#endregion





