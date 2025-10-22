using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private InputController _input = null;

    [SerializeField, Range(0.00f, 10.00f)] private float _jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int _maxAirJumps = 0;
    [SerializeField, Range(0.00f, 5.00f)] private float _downwardMovementMultiplier = 3f;
    [SerializeField, Range(0.00f, 5.00f)] private float _upwardMovementMultiplier = 1.7f;

    private Rigidbody2D _body = null;
    private Ground _ground = null;
    private Vector2 _velocity = Vector2.zero;

    private int _jumpPhase = 0;
    private float _defaultGravityScale = 0.00f, _jumpSpeed = 0.00f;

    private bool _desiredJump = false, _onGround = false;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();

        _defaultGravityScale = 1f;
    }

    private void Update() => _desiredJump |= _input.RetrieveJumpInput();
    private void FixedUpdate()
    {
        _onGround = _ground.GetOnGround;
        _velocity = _body.linearVelocity;

        _jumpPhase = _onGround ? 0 : _jumpPhase;

        if (_desiredJump)
        {
            _desiredJump = false;
            JumpAction();
        }

        _body.gravityScale = _body.linearVelocity.y != 0 ? (_body.linearVelocity.y > 0 ? _body.gravityScale = _upwardMovementMultiplier : _downwardMovementMultiplier) : _defaultGravityScale;

        _body.linearVelocity = _velocity;
    }

    private void JumpAction()
    {
        if (_onGround | _jumpPhase <= _maxAirJumps)
        {
            _jumpPhase++;

            _jumpSpeed = _velocity.y != 0 ? (_velocity.y > 0 ? Mathf.Max(_jumpSpeed - _velocity.y, 0f) : Mathf.Abs(_body.linearVelocity.y)) : Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight);

            _velocity.y += _jumpSpeed;
        }
    }
}