using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private delegate void OnCollisionDetectHandler(Collision2D collision);

    private OnCollisionDetectHandler OnCollisionDetectCallback;

    public bool GetOnGround => _onGround;
    private bool _onGround = false;

    public float GetFriction => _friction;
    private float _friction = 0.00f;

    private void OnEnable()
    {
        OnCollisionDetectCallback += EvaluateCollision;
        OnCollisionDetectCallback += RetrieveFriction;

    }
    private void OnDisable()
    {
        OnCollisionDetectCallback -= EvaluateCollision;
        OnCollisionDetectCallback -= RetrieveFriction;
    }

    private void OnCollisionEnter2D(Collision2D collision) => InvokeOnCollisionDetectCallback(collision);
    private void OnCollisionStay2D(Collision2D collision) => InvokeOnCollisionDetectCallback(collision);
    private void OnCollisionExit2D(Collision2D collision)
    {
        _onGround = false;
        _friction = 0.00f;
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;

            _onGround |= normal.y >= 0.90f;
        }
    }
    private void RetrieveFriction(Collision2D collision)
    {
        PhysicsMaterial2D material = collision.rigidbody.sharedMaterial;

        _friction = 0.00f;

        if (material)
            _friction = material.friction;
    }

    private void InvokeOnCollisionDetectCallback(Collision2D collision) => OnCollisionDetectCallback?.Invoke(collision);
}