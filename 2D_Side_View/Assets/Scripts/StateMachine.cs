using UnityEngine;
using UnityEngine.InputSystem;



public abstract class MovementStateBase : IMovementState
{
    protected Rigidbody2D _rigidbody2D;
    protected float _speed;

    public virtual void SetRigidBody2D(Rigidbody2D rb) => _rigidbody2D = rb;
    public virtual void SetSpeed(float speed) => _speed = speed;
    public virtual void HandleInput(Vector2 input) { }
    public virtual void Enter() { }
    public virtual void Exit() { }
}

public class Walk : MovementStateBase
{
    public override void HandleInput(Vector2 input)
    {
        if (_rigidbody2D != null)
            _rigidbody2D.linearVelocity = new Vector2(input.x * _speed, _rigidbody2D.linearVelocity.y);
    }
}

public class Idle : MovementStateBase
{
    public override void HandleInput(Vector2 input)
    {
        if (_rigidbody2D != null)
            _rigidbody2D.linearVelocity= new Vector2(0, _rigidbody2D.linearVelocity.y);
    }
}
public class Jump : MovementStateBase
{
    private float _timer;
    private float _duration = 1f;
    private Idle _idleState;

    public void SetIdleState(Idle idle) => _idleState = idle;

    public override void Enter()
    {
        _timer = 0f;
        if (_rigidbody2D != null)
        {
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, 0f);
            _rigidbody2D.AddForce(Vector2.up * _speed, ForceMode2D.Impulse);
        }
    }

    public override void HandleInput(Vector2 input)
    {
        _timer += Time.deltaTime;
        if (_timer >= _duration && _idleState != null)
        {
            MovementStateMachine.Instance.SetState(_idleState);
        }
    }
}

public interface IMovementState
{
    void Enter();
    void Exit();
    void HandleInput(Vector2 input);
}

public class MovementStateMachine
{
    private static MovementStateMachine _instance;
    public static MovementStateMachine Instance => _instance ??= new MovementStateMachine();

    private IMovementState _currentState;
    public IMovementState CurrentState => _currentState;

    public void SetState(IMovementState newState)
    {
        if (_currentState == newState) return;
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }
}

