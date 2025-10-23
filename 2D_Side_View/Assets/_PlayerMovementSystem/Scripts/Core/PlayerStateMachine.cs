using System.Collections.Generic;
using UnityEngine;

namespace PlayerControlSystem
{
    // public interface IPlayerSubState
    // {
    //     void Enter();
    //     void Update();
    //     void Exit();
    //     Vector2 CalculateVelocity();
    // }
    public abstract class PlayerSuperState
    {
        protected PlayerStateMachine player;
        protected IPlayerSubState subState;

        public PlayerSuperState(PlayerStateMachine player)
        {
            this.player = player;
        }

        public virtual void Enter() => subState?.Enter();
        public virtual void Update()
        {
            subState?.Update();
        }
        public virtual void Exit() => subState?.Exit();
    }

    // -----------------------------
    // PLAYER STATE MACHINE
    // -----------------------------
    public class PlayerStateMachine
    {
        public Rigidbody2D rb;
        public PlayerMovementController movementController;

        // Super states
        public GroundedState GroundedState { get; private set; }
        public AirborneState AirborneState { get; private set; }

        private PlayerSuperState currentSuperState;

        public PlayerStateMachine(PlayerMovementController controller)
        {
            movementController = controller;
            rb = controller.rb;

            GroundedState = new GroundedState(this);
            AirborneState = new AirborneState(this);

            currentSuperState = GroundedState;
            currentSuperState.Enter();
        }

        public void Update()
        {
            // Input → state logic
            currentSuperState.Update();

            // Rigidbody velocity uygulama
            Vector2 velocity = (currentSuperState as dynamic).subState?.CalculateVelocity() ?? Vector2.zero;
            rb.linearVelocity = velocity;
        }

        public void ChangeSuperState(PlayerSuperState next)
        {
            if (currentSuperState == next) return;

            currentSuperState.Exit();
            currentSuperState = next;
            currentSuperState.Enter();
        }

        public bool IsGrounded()
        {
            // Basit örnek, zemine temas kontrolü
            return movementController.IsGrounded;
        }
    }

    // -----------------------------
    // SUPER STATES
    // -----------------------------
    public class GroundedState : PlayerSuperState
    {
        public MoveSubState MoveSub { get; private set; }
        public IdleSubState IdleSub { get; private set; }

        public GroundedState(PlayerStateMachine player) : base(player)
        {
            MoveSub = new MoveSubState(player, this);
            IdleSub = new IdleSubState(player, this);
            subState = IdleSub;
        }

        public override void Update()
        {
            // Alt state seçimi
            Vector2 moveInput = player.movementController.GetHorizontalInput();
            if (moveInput.sqrMagnitude > 0.01f)
                subState = MoveSub;
            else
                subState = IdleSub;

            // Zıplama tetik kontrolü
            if (player.movementController.JumpTriggered)
                player.ChangeSuperState(player.AirborneState);

            base.Update();
        }
    }

    public class AirborneState : PlayerSuperState
    {
        public JumpSubState JumpSub { get; private set; }
        public FallSubState FallSub { get; private set; }

        public AirborneState(PlayerStateMachine player) : base(player)
        {
            JumpSub = new JumpSubState(player, this);
            FallSub = new FallSubState(player, this);
            subState = JumpSub;
        }

        public override void Update()
        {
            // Hız kontrolü vs
            if (player.rb.linearVelocity.y < 0)
                subState = FallSub;

            if (player.IsGrounded())
                player.ChangeSuperState(player.GroundedState);

            base.Update();
        }
    }

    // -----------------------------
    // SUB STATES
    // -----------------------------
    public class IdleSubState : IPlayerSubState
    {
        private PlayerStateMachine player;
        private GroundedState parent;

        public IdleSubState(PlayerStateMachine player, GroundedState parent)
        {
            this.player = player;
            this.parent = parent;
        }

        public void Enter() { }
        public void Update() { }
        public void Exit() { }

        public Vector2 CalculateVelocity() => Vector2.zero;
    }

    public class MoveSubState : IPlayerSubState
    {
        private PlayerStateMachine player;
        private GroundedState parent;

        public MoveSubState(PlayerStateMachine player, GroundedState parent)
        {
            this.player = player;
            this.parent = parent;
        }

        public void Enter() { }
        public void Update() { }
        public void Exit() { }

        public Vector2 CalculateVelocity()
        {
            Vector2 total = Vector2.zero;
            foreach (var bridge in player.movementController.activeBridges)
            {
                total += bridge.CalculateVelocity(bridge.playerInput);
            }
            return total;
        }
    }

    public class JumpSubState : IPlayerSubState
    {
        private PlayerStateMachine player;
        private AirborneState parent;

        public JumpSubState(PlayerStateMachine player, AirborneState parent)
        {
            this.player = player;
            this.parent = parent;
        }

        public void Enter()
        {
            // Jump velocity
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.movementController.JumpForce);
        }

        public void Update() { }
        public void Exit() { }

        public Vector2 CalculateVelocity()
        {
            Vector2 total = Vector2.zero;
            foreach (var bridge in player.movementController.activeBridges)
            {
                total += bridge.CalculateVelocity(bridge.playerInput);
            }
            total.y = player.rb.linearVelocity.y;
            return total;
        }
    }

    public class FallSubState : IPlayerSubState
    {
        private PlayerStateMachine player;
        private AirborneState parent;

        public FallSubState(PlayerStateMachine player, AirborneState parent)
        {
            this.player = player;
            this.parent = parent;
        }

        public void Enter() { }
        public void Update() { }
        public void Exit() { }

        public Vector2 CalculateVelocity()
        {
            Vector2 total = Vector2.zero;
            foreach (var bridge in player.movementController.activeBridges)
            {
                total += bridge.CalculateVelocity(bridge.playerInput);
            }
            total.y = player.rb.linearVelocity.y;
            return total;
        }
    }
}

