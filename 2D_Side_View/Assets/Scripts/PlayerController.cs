using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private InputSystem_Actions _inputActions;
    private Vector2 _currentMoveInput;

    private Idle _idleState;
    private Walk _walkState;
    private Jump _jumpState;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _inputActions = new InputSystem_Actions();

        _idleState = new Idle();
        _walkState = new Walk();
        _jumpState = new Jump();

        // Rigidbody ve speed ayarları
        _idleState.SetRigidBody2D(rb);
        _walkState.SetRigidBody2D(rb);
        _walkState.SetSpeed(moveSpeed);
        _jumpState.SetRigidBody2D(rb);
        _jumpState.SetSpeed(jumpForce);

        // Jump state idle referansı
        _jumpState.SetIdleState(_idleState);

        MovementStateMachine.Instance.SetState(_idleState);
    }

    private void FixedUpdate()
    {
        MovementStateMachine.Instance.CurrentState?.HandleInput(_currentMoveInput);
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMove;
        _inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Move.canceled -= OnMove;
        _inputActions.Player.Jump.performed -= OnJump;
        _inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _currentMoveInput = context.ReadValue<Vector2>();

        if (_currentMoveInput.x == 0)
            MovementStateMachine.Instance.SetState(_idleState);
        else
            MovementStateMachine.Instance.SetState(_walkState);
    }
    
    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && MovementStateMachine.Instance.CurrentState is not Jump)
        {
            Debug.Log("Jump triggered");
            MovementStateMachine.Instance.SetState(_jumpState);
        }
    }

}


