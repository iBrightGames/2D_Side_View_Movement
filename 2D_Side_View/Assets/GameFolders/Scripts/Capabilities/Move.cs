using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private InputController _input = null;

    [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;

    private Vector2 _direction = Vector2.zero;
    private Vector2 _desiredVelocity = Vector2.zero;
    private Vector2 _velocity = Vector2.zero;

    private Rigidbody2D _body = null;
    private Ground _ground = null;

    private float _maxSpeedChange = 0.00f;
    private float _acceleration = 0.00f;
    private bool _onGround = false;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
    }

    private void Update()
    {
        _direction.x = _input.RetrieveMoveInput();
        _desiredVelocity = new Vector2(_direction.x, 0.00f) * Mathf.Max(_maxSpeed - _ground.GetFriction, 0.00f);
    }

    private void FixedUpdate()
    {
        _onGround = _ground.GetOnGround;
        _velocity = _body.linearVelocity;

        _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

        _body.linearVelocity = _velocity;
    }
}