
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControlSystem
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {
        public SpriteRenderer sr;
        public Rigidbody2D rb;
        public List<InputForceBridge> inputMovementBridges;

        public PlayerStateMachine stateMachine;

        // bridge tiplerine göre ayrılmış listeler
        private List<InputForceBridge> holdBridges = new();
        private List<InputForceBridge> triggerBridges = new();
        private HashSet<InputForceBridge> activeHoldBridges = new();

        public event Action<string> OnCollided;

        private void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();

            foreach (var bridge in inputMovementBridges)
            {
                if (!bridge.IsValid) continue;

                InputActionReference action = bridge.playerInput.action;
                action.action.Enable();


                bool requiresHold = action.action.interactions != null &&
                                    action.action.interactions.ToLower().Contains("hold");

                if (requiresHold)
                {
                    holdBridges.Add(bridge);

                    action.action.started += ctx => activeHoldBridges.Add(bridge);
                    action.action.canceled += ctx => activeHoldBridges.Remove(bridge);
                }
                else
                {
                    triggerBridges.Add(bridge);
                    action.action.performed += ctx => ApplyForce(bridge);
                }

            }

            stateMachine = new PlayerStateMachine(this);
            // stateMachine.Enter();
        }

        private void FixedUpdate()
        {
            // basılı tuşlar için sürekli kuvvet uygula
            foreach (var bridge in activeHoldBridges)
                ApplyForce(bridge);
            stateMachine.Update();
        }

        private void ApplyForce(InputForceBridge bridge)
        {
            MovementExecuter.ExecuteMovement(rb, bridge, false);
            if (bridge.forceConfig.axis == InputAxis.Horizontal)
            {
                sr.flipX = bridge.forceConfig.polarity == InputPolarity.Negative;
            }
            else if (bridge.forceConfig.axis == InputAxis.Vertical)
            {
                sr.flipY = bridge.forceConfig.polarity == InputPolarity.Negative;
            }

        }


        private void OnDisable()
        {
            foreach (var bridge in inputMovementBridges)
            {
                var action = bridge.playerInput.action.action;

                // 2. Eylemi devre dışı bırak
                action.Disable();
            }

            holdBridges.Clear();
            triggerBridges.Clear();
            activeHoldBridges.Clear();
        }


        public void OnCollisionEnter2D(Collision2D other)
        {
            OnCollided?.Invoke(other.collider.name);
            Debug.Log($"colliden {other.collider.name}");
        }


    }



}


