using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControlSystem
{
    public class PlayerStateMachine
    {
        public Rigidbody2D rb;
        public PlayerMovementController controller;

        private PlayerActionState currentState;
        public event Action<PlayerActionState> OnStateChanged;
        private readonly Dictionary<Type, PlayerActionState> availableStates;

        public PlayerStateMachine(PlayerMovementController ctrl)
        {
            controller = ctrl;
            rb = ctrl.rb;
            
            // State Havuzu (Pooling)
            availableStates = new Dictionary<Type, PlayerActionState>
            {
                { typeof(IdleState), new IdleState(this) },
                { typeof(WalkState), new WalkState(this) },
                { typeof(JumpState), new JumpState(this) },
                { typeof(SwimState), new SwimState(this) }
            };

            // Başlangıç State'i
            currentState = availableStates[typeof(IdleState)];
        }
        public virtual void Enter()
        {
            controller.OnCollided += HandleEnvironmentChanged;
        }

        public virtual void Exit()
        {
            controller.OnCollided -= HandleEnvironmentChanged;
        }


        protected virtual void HandleEnvironmentChanged(String env)
        {
            if (env != EnvironmentType.Water.ToString())
            currentState= new SwimState(this);
        }
        private PlayerActionState DetermineState()
        {

            if (Mathf.Abs(rb.linearVelocityY) > 0.01f)
                return new JumpState(this);
            else if (Mathf.Abs(rb.linearVelocityX) > 0.01f)
                return new WalkState(this);
            else
                return new IdleState(this);
        }

        private void ChangeState(PlayerActionState newState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();

            OnStateChanged?.Invoke(currentState);
            Debug.Log($"new state {newState}");
        }
        public void Update()
        {
            PlayerActionState nextState = DetermineState();

            if (nextState != null && nextState.GetType() != currentState.GetType())
                ChangeState(nextState);

            currentState.Update();
            if (nextState.GetType() != currentState.GetType())
            {
                currentState.Exit();
                currentState = nextState;
                currentState.Enter();
            }

            currentState.Update();
        }
    }


    public abstract class PlayerActionState
    {
        protected PlayerStateMachine stateMachine;

        public PlayerActionState(PlayerStateMachine machine)
        {
            stateMachine = machine;
        }

        public virtual void Enter()
        {
            // Ortam değişikliklerini dinle
            stateMachine.controller.OnCollided += HandleEnvironmentChanged;
        }

        public virtual void Exit()
        {
            stateMachine.controller.OnCollided -= HandleEnvironmentChanged;
        }

        public virtual void Update() { }

        // Ortam değiştiğinde ne olacağı
        protected virtual void HandleEnvironmentChanged(String env) { }

        // İsteğe bağlı: state geçişi belirlemek için
        public virtual PlayerActionState DetermineNextState() { return this; }
    }


    public class IdleState : PlayerActionState
    {
        public IdleState(PlayerStateMachine machine) : base(machine)
        {
        }
    }
    public class JumpState : PlayerActionState
    {
        public JumpState(PlayerStateMachine machine) : base(machine) { }

    }
    public class WalkState : PlayerActionState
    {
        public WalkState(PlayerStateMachine machine) : base(machine) { }

    }
    public class SwimState : PlayerActionState
    {
        public SwimState(PlayerStateMachine machine) : base(machine) { }

        protected override void HandleEnvironmentChanged(string env)
        {
        }
    }

    public enum EnvironmentType
    { Water, Land }


}
