using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControlSystem
{
    public enum InputAxis { Horizontal, Vertical }
    public enum InputPolarity { Positive, Negative }

    [System.Serializable]
    public class ForceConfig
    {
        [Header("Force Type")]
        public ForceType forceType;

        [Header("Magnitude")]
        public float forceMagnitude = 10f;

        [Tooltip("Hangi eksen?")]
        public InputAxis axis;

        [Tooltip("Hangi yön?")]
        public InputPolarity polarity;

        public Vector2 GetLinearForceVector()
        {
            Vector2 dir = axis switch
            {
                InputAxis.Horizontal => Vector2.right,
                InputAxis.Vertical => Vector2.up,
                _ => Vector2.zero
            };

            float sign = polarity switch
            {
                InputPolarity.Positive => 1f,
                InputPolarity.Negative => -1f,
                _ => 0f
            };

            return dir * forceMagnitude * sign;
        }

        public float GetAngularForce()
        {
            switch (polarity)
            {
                case InputPolarity.Positive: return forceMagnitude;
                case InputPolarity.Negative: return -forceMagnitude;
            }
            return 0;
        }

    }
    [System.Serializable]
    public class UserInput
    {
        public InputActionReference action;

        public bool IsPressed => action != null && action.action.IsPressed();
        public bool WasPerformedThisFrame => action != null && action.action.triggered;

        public void Subscribe()
        {
            if (action == null) return;
            action.action.Enable();
        }

        public void Unsubscribe()
        {
            if (action == null) return;
            action.action.Disable();
        }
    }

    [System.Serializable]
    public class InputForceBridge
    {
        public UserInput playerInput;
        public ForceConfig forceConfig;
        public bool IsValid => playerInput != null && forceConfig != null;



    }

    public interface IPlayerActionState
    {
        void Enter();
        void Update();
        void Exit();
    }

    // ===================================
    // 1. STATE MACHINE SADECE CONTEXT TUTAR
    // ===================================
    public class PlayerStateMachine
    {
        public Rigidbody2D rb;
        public PlayerMovementController controller;
        private PlayerEnvironmentState currentState;
        public PlayerStateMachine(PlayerMovementController ctrl)
        {
            controller = ctrl;
            rb = ctrl.rb;

            // Başlangıç state'i
            currentState = new GroundedState(this);
            currentState.Enter();
        }

        public void Update()
        {
            // State logic'i çalıştır
            currentState.Update();

            // State geçişlerini kontrol et
            CheckTransitions();
        }

        public void FixedUpdate()
        {
            // State hangi force'lara izin veriyorsa onları çalıştır
            ExecuteAllowedForces();
        }

        private void CheckTransitions()
        {
            PlayerEnvironmentState nextState = currentState.CheckTransition();
            if (nextState != null && nextState != currentState)
            {
                currentState.Exit();
                currentState = nextState;
                currentState.Enter();
            }
        }

        private void ExecuteAllowedForces()
        {
            // Aktif inputlardaki force'ları kontrol et
            foreach (var bridge in controller.activeBridges)
            {
                // State bu force'a izin veriyor mu?
                if (currentState.IsForceAllowed(bridge))
                {
                    MovementExecuter.ExecuteMovement(rb, bridge, controller.showDebugLogs);
                }
            }
        }

        public void ChangeState(PlayerEnvironmentState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }

    // ===================================
    // 2. STATE'LER SADECE KURALLAR İÇERİR
    // ===================================
    public abstract class PlayerEnvironmentState
    {
        protected PlayerStateMachine player;
        
        public PlayerEnvironmentState(PlayerStateMachine player)
        {
            this.player = player;
        }
        
        public virtual void Enter() { }
        public virtual void Update() { }
        public virtual void Exit() { }
        
        // Bu state'te hangi force'lara izin var?
        public abstract bool IsForceAllowed(InputForceBridge bridge);
        
        // State geçişi gerekli mi?
        public abstract PlayerEnvironmentState CheckTransition();
    }

    // ===================================
    // 3. GROUNDED STATE
    // ===================================
    public class GroundedState : PlayerEnvironmentState
    {
        public GroundedState(PlayerStateMachine player) : base(player) { }
        
        public override void Enter()
        {
            Debug.Log("[State] Entered Grounded");
        }
        
        public override bool IsForceAllowed(InputForceBridge bridge)
        {
            // Yerdeyken tüm force'lara izin ver
            return true;
            
            // VEYA daha spesifik:
            // return bridge.forceConfig.forceType != ForceType.AddImpulse; // Impulse sadece havada
        }
        
        public override PlayerEnvironmentState CheckTransition()
        {
            // Yerden ayrıldı mı?
            if (player.rb.linearVelocity.y > 0.1f)
            {
                return new AirborneState(player);
            }
            return null;
        }
    }


    // ===================================
    // 4. AIRBORNE STATE
    // ===================================
    public class AirborneState : PlayerEnvironmentState
    {
        public AirborneState(PlayerStateMachine player) : base(player) { }

        public override void Enter()
        {
            Debug.Log("[State] Entered Airborne");
        }

        public override bool IsForceAllowed(InputForceBridge bridge)
        {
            // Havada sadece horizontal movement'e izin ver
            if (bridge.forceConfig.axis == InputAxis.Horizontal)
                return true;

            // Jump'a izin verme (zaten havadayız)
            if (bridge.forceConfig.axis == InputAxis.Vertical &&
                bridge.forceConfig.polarity == InputPolarity.Positive)
                return false;

            return true;
        }

        public override PlayerEnvironmentState CheckTransition()
        {
            return new GroundedState(player);
        }
    }



}

