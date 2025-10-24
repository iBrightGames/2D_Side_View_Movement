using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControlSystem
{
    public enum EnvironmentType { Land, Water, Ice, Grass }

    // --------------------------------------
    // STATE MACHINE
    // --------------------------------------
    // STATE MACHINE - Sadece event hub
    public class PlayerStateMachine
    {
        public Rigidbody2D rb;
        public PlayerMovementController controller;
        private EnvironmentState currentEnvironmentState;
        
        // Events
        public event Action<EnvironmentState> OnEnvironmentChanged;
        public event Action<ActionState> OnActionChanged;

        // Event fırlatma metodları (EnvironmentState'ten çağrılacak)
        public void NotifyActionChanged(ActionState newAction)
        {
            OnActionChanged?.Invoke(newAction);
        }

        private readonly Dictionary<EnvironmentType, EnvironmentState> availableEnvironments;

        public PlayerStateMachine(PlayerMovementController ctrl)
        {
            controller = ctrl;
            rb = ctrl.rb;

            availableEnvironments = new Dictionary<EnvironmentType, EnvironmentState>
            {
                { EnvironmentType.Land, new LandState(this) },
                { EnvironmentType.Water, new WaterState(this) },
                { EnvironmentType.Ice, new IceState(this) },
                { EnvironmentType.Grass, new GrassState(this) }
            };

            currentEnvironmentState = availableEnvironments[EnvironmentType.Land];

            controller.OnCollided += HandleEnvironmentChanged;
            currentEnvironmentState.EnterEnvironment();
        }

        public void ExitStateMachine()
        {
            controller.OnCollided -= HandleEnvironmentChanged;
            currentEnvironmentState.ExitEnvironment();
        }

        private void HandleEnvironmentChanged(string envString)
        {
            if (Enum.TryParse<EnvironmentType>(envString, true, out var env))
            {
                if (availableEnvironments.TryGetValue(env, out var nextEnv))
                {
                    ChangeEnvironment(nextEnv);
                }
            }
            else
            {
                Debug.LogWarning($"Unknown environment: {envString}");
            }
        }

        private void ChangeEnvironment(EnvironmentState newEnv)
        {
            if (currentEnvironmentState == newEnv) return;

            currentEnvironmentState.ExitEnvironment();
            currentEnvironmentState = newEnv;
            currentEnvironmentState.EnterEnvironment();

            OnEnvironmentChanged?.Invoke(currentEnvironmentState);
            Debug.Log($"[StateMachine] Environment changed to: {newEnv.GetType().Name}");
        }

        public void Update()
        {
            currentEnvironmentState.Update();
        }

        // Query metodları (opsiyonel - dışarıdan state bilgisi almak için)
        public EnvironmentState GetCurrentEnvironment() => currentEnvironmentState;
        public ActionState GetCurrentAction() => currentEnvironmentState?.GetCurrentAction();
    }

    // --------------------------------------
    // ENVIRONMENT STATE
    // --------------------------------------
    
    public abstract class EnvironmentState
    {
        public PlayerStateMachine machine;
        protected ActionState currentActionState;
        protected Dictionary<Type, ActionState> availableActions;

        protected EnvironmentState(PlayerStateMachine machine)
        {
            this.machine = machine;
            availableActions = new Dictionary<Type, ActionState>();
        }

        public virtual void EnterEnvironment()
        {
            currentActionState = DetermineActionState();
            Debug.Log($"action is {currentActionState}");
            currentActionState?.EnterAction();
            machine.NotifyActionChanged(currentActionState);
        }

        public virtual void ExitEnvironment()
        {
            currentActionState?.ExitAction();
        }

        public virtual void Update()
        {
            ActionState next = DetermineActionState();
            if (next != null && next.GetType() != currentActionState?.GetType())
                ChangeActionState(next);

            currentActionState?.Update();
        }

        protected void ChangeActionState(ActionState newAction)
        {
            currentActionState?.ExitAction();
            currentActionState = newAction;
            currentActionState.EnterAction();

            // Event'i machine üzerinden fırlat
            machine.NotifyActionChanged(newAction);
            Debug.Log($"[EnvironmentState] Action changed to: {newAction.GetType().Name}");
        }

        protected abstract ActionState DetermineActionState();

        // Query metodu
        public ActionState GetCurrentAction() => currentActionState;
    }


    // --------------------------------------
    // ACTION STATE
    // --------------------------------------
    
    
    public abstract class ActionState
    {
        protected EnvironmentState envState;
        protected PlayerStateMachine machine;


        protected ActionState(EnvironmentState env)
        {
            envState = env;
            machine = env.machine;
        }

        public virtual void EnterAction() { Debug.Log($"{envState}"); }
        public virtual void ExitAction() { }
        public virtual void Update() { }
    }

    // --------------------------------------
    // SPECIFIC ENVIRONMENTS
    // --------------------------------------
    public class LandState : EnvironmentState
    {
        public LandState(PlayerStateMachine machine) : base(machine)
        {
            availableActions = new Dictionary<Type, ActionState>
            {
                { typeof(IdleState), new IdleState(this) },
                { typeof(WalkState), new WalkState(this) },
                { typeof(JumpState), new JumpState(this) }
            };
            currentActionState = availableActions[typeof(IdleState)];
        }

        protected override ActionState DetermineActionState()
        {
            if (Mathf.Abs(machine.rb.linearVelocityY) > 0.01f)
                return availableActions[typeof(JumpState)];
            else if (Mathf.Abs(machine.rb.linearVelocityX) > 0.01f)
                return availableActions[typeof(WalkState)];
            else
                return availableActions[typeof(IdleState)];
        }
    
    }

    public class WaterState : EnvironmentState
    {
        public WaterState(PlayerStateMachine machine) : base(machine)
        {
            availableActions = new Dictionary<Type, ActionState>
            {
                { typeof(SwimState), new SwimState(this) }
            };
            currentActionState = availableActions[typeof(SwimState)];
        }

        protected override ActionState DetermineActionState()
        {
            return availableActions[typeof(SwimState)]; // şimdilik tek action
        }
    }

    public class IceState : EnvironmentState
    {
        public IceState(PlayerStateMachine machine) : base(machine)
        {
            availableActions = new Dictionary<Type, ActionState>
            {
                { typeof(IdleState), new IdleState(this) },
                { typeof(WalkState), new WalkState(this) }
            };
            currentActionState = availableActions[typeof(IdleState)];
        }

        protected override ActionState DetermineActionState()
        {
            if (Mathf.Abs(machine.rb.linearVelocityX) > 0.01f)
                return availableActions[typeof(WalkState)];
            else
                return availableActions[typeof(IdleState)];
        }
    }

    public class GrassState : EnvironmentState
    {
        public GrassState(PlayerStateMachine machine) : base(machine)
        {
            availableActions = new Dictionary<Type, ActionState>
            {
                { typeof(IdleState), new IdleState(this) },
                { typeof(WalkState), new WalkState(this) }
            };
            currentActionState = availableActions[typeof(IdleState)];
        }

        protected override ActionState DetermineActionState()
        {
            if (Mathf.Abs(machine.rb.linearVelocityX) > 0.01f)
                return availableActions[typeof(WalkState)];
            else
                return availableActions[typeof(IdleState)];
        }
    }

    // --------------------------------------
    // SPECIFIC ACTIONS
    // --------------------------------------
    public class IdleState : ActionState
    {
        public IdleState(EnvironmentState env) : base(env) { }

    }

    public class WalkState : ActionState
    {
        public WalkState(EnvironmentState env) : base(env) { }
    }

    public class JumpState : ActionState
    {
        public JumpState(EnvironmentState env) : base(env) { }
    }

    public class SwimState : ActionState
    {
        public SwimState(EnvironmentState env) : base(env) { }
    }

}


// using System;
// using System.Collections.Generic;
// using UnityEngine;

// namespace PlayerControlSystem
// {
//     public enum EnvironmentType { Land, Water, Ice, Grass }

//     // --------------------------------------
//     // STATE MACHINE
//     // --------------------------------------
//     // STATE MACHINE - Sadece event hub
//     public class PlayerStateMachine
//     {
//         public Rigidbody2D rb;
//         public PlayerMovementController controller;
//         private EnvironmentState currentEnvironmentState;
        
//         // Events
//         public event Action<EnvironmentState> OnEnvironmentChanged;
//         public event Action<ActionState> OnActionChanged;

//         // Event fırlatma metodları (EnvironmentState'ten çağrılacak)
//         public void NotifyActionChanged(ActionState newAction)
//         {
//             OnActionChanged?.Invoke(newAction);
//         }

//         private readonly Dictionary<EnvironmentType, EnvironmentState> availableEnvironments;

//         public PlayerStateMachine(PlayerMovementController ctrl)
//         {
//             controller = ctrl;
//             rb = ctrl.rb;

//             availableEnvironments = new Dictionary<EnvironmentType, EnvironmentState>
//             {
//                 { EnvironmentType.Land, new LandState(this) },
//                 { EnvironmentType.Water, new WaterState(this) },
//                 { EnvironmentType.Ice, new IceState(this) },
//                 { EnvironmentType.Grass, new GrassState(this) }
//             };

//             currentEnvironmentState = availableEnvironments[EnvironmentType.Land];
//         }

//         public void Enter()
//         {
//             controller.OnCollided += HandleEnvironmentChanged;
//             currentEnvironmentState.Enter();
//         }

//         public void Exit()
//         {
//             controller.OnCollided -= HandleEnvironmentChanged;
//             currentEnvironmentState.Exit();
//         }

//         private void HandleEnvironmentChanged(string envString)
//         {
//             Debug.Log("handler");
//             if (Enum.TryParse<EnvironmentType>(envString, true, out var env))
//             {
//                 if (availableEnvironments.TryGetValue(env, out var nextEnv))
//                 {
//                     ChangeEnvironment(nextEnv);
//                     Debug.Log($"change to {envString}");
//                 }
//             }
//             else
//             {
//                 Debug.LogWarning($"Unknown environment: {envString}");
//             }
//         }

//         private void ChangeEnvironment(EnvironmentState newEnv)
//         {
//             if (currentEnvironmentState == newEnv) return;

//             currentEnvironmentState.Exit();
//             currentEnvironmentState = newEnv;
//             currentEnvironmentState.Enter();

//             OnEnvironmentChanged?.Invoke(currentEnvironmentState);
//             Debug.Log($"[StateMachine] Environment changed to: {newEnv.GetType().Name}");
//         }

//         public void Update()
//         {
//             currentEnvironmentState.Update();
//         }

//         // Query metodları (opsiyonel - dışarıdan state bilgisi almak için)
//         public EnvironmentState GetCurrentEnvironment() => currentEnvironmentState;
//         public ActionState GetCurrentAction() => currentEnvironmentState?.GetCurrentAction();
//     }

//     // --------------------------------------
//     // ENVIRONMENT STATE
//     // --------------------------------------
    
//     public abstract class EnvironmentState
//     {
//         public PlayerStateMachine machine;
//         protected ActionState currentActionState;
//         protected Dictionary<Type, ActionState> availableActions;

//         protected EnvironmentState(PlayerStateMachine machine)
//         {
//             this.machine = machine;
//             availableActions = new Dictionary<Type, ActionState>();
//         }

//         public virtual void Enter()
//         {
//             currentActionState?.Enter();
//             Debug.Log("action state entered");
//             machine.NotifyActionChanged(currentActionState);
//         }

//         public virtual void Exit()
//         {
//             currentActionState?.Exit();
//         }

//         public virtual void Update()
//         {
//             ActionState next = DetermineActionState();
//             if (next != null && next.GetType() != currentActionState?.GetType())
//                 ChangeActionState(next);

//             currentActionState?.Update();
//         }

//         protected void ChangeActionState(ActionState newAction)
//         {
//             currentActionState?.Exit();
//             currentActionState = newAction;
//             currentActionState.Enter();

//             // Event'i machine üzerinden fırlat
//             machine.NotifyActionChanged(newAction);
//             Debug.Log($"[EnvironmentState] Action changed to: {newAction.GetType().Name}");
//         }

//         protected abstract ActionState DetermineActionState();

//         // Query metodu
//         public ActionState GetCurrentAction() => currentActionState;
//     }


//     // --------------------------------------
//     // ACTION STATE
//     // --------------------------------------
    
    
//     public abstract class ActionState
//     {
//         protected EnvironmentState envState;
//         protected PlayerStateMachine machine;


//         protected ActionState(EnvironmentState env)
//         {
//             envState = env;
//             machine = env.machine;
//         }

//         public virtual void Enter() { Debug.Log($"{envState}"); }
//         public virtual void Exit() { }
//         public virtual void Update() { }
//     }

//     // --------------------------------------
//     // SPECIFIC ENVIRONMENTS
//     // --------------------------------------
//     public class LandState : EnvironmentState
//     {
//         public LandState(PlayerStateMachine machine) : base(machine)
//         {
//             availableActions = new Dictionary<Type, ActionState>
//             {
//                 { typeof(IdleState), new IdleState(this) },
//                 { typeof(WalkState), new WalkState(this) },
//                 { typeof(JumpState), new JumpState(this) }
//             };
//             currentActionState = availableActions[typeof(IdleState)];
//         }

//         protected override ActionState DetermineActionState()
//         {
//             if (Mathf.Abs(machine.rb.linearVelocityY) > 0.01f)
//                 return availableActions[typeof(JumpState)];
//             else if (Mathf.Abs(machine.rb.linearVelocityX) > 0.01f)
//                 return availableActions[typeof(WalkState)];
//             else
//                 return availableActions[typeof(IdleState)];
//         }
    
//     }

//     public class WaterState : EnvironmentState
//     {
//         public WaterState(PlayerStateMachine machine) : base(machine)
//         {
//             availableActions = new Dictionary<Type, ActionState>
//             {
//                 { typeof(SwimState), new SwimState(this) }
//             };
//             currentActionState = availableActions[typeof(SwimState)];
//         }

//         protected override ActionState DetermineActionState()
//         {
//             return availableActions[typeof(SwimState)]; // şimdilik tek action
//         }
//     }

//     public class IceState : EnvironmentState
//     {
//         public IceState(PlayerStateMachine machine) : base(machine)
//         {
//             availableActions = new Dictionary<Type, ActionState>
//             {
//                 { typeof(IdleState), new IdleState(this) },
//                 { typeof(WalkState), new WalkState(this) }
//             };
//             currentActionState = availableActions[typeof(IdleState)];
//         }

//         protected override ActionState DetermineActionState()
//         {
//             if (Mathf.Abs(machine.rb.linearVelocityX) > 0.01f)
//                 return availableActions[typeof(WalkState)];
//             else
//                 return availableActions[typeof(IdleState)];
//         }
//     }

//     public class GrassState : EnvironmentState
//     {
//         public GrassState(PlayerStateMachine machine) : base(machine)
//         {
//             availableActions = new Dictionary<Type, ActionState>
//             {
//                 { typeof(IdleState), new IdleState(this) },
//                 { typeof(WalkState), new WalkState(this) }
//             };
//             currentActionState = availableActions[typeof(IdleState)];
//         }

//         protected override ActionState DetermineActionState()
//         {
//             if (Mathf.Abs(machine.rb.linearVelocityX) > 0.01f)
//                 return availableActions[typeof(WalkState)];
//             else
//                 return availableActions[typeof(IdleState)];
//         }
//     }

//     // --------------------------------------
//     // SPECIFIC ACTIONS
//     // --------------------------------------
//     public class IdleState : ActionState
//     {
//         public IdleState(EnvironmentState env) : base(env) { }
//         public override void Enter() { /* animasyon vs */ }
//         public override void Update() { }
//     }

//     public class WalkState : ActionState
//     {
//         public WalkState(EnvironmentState env) : base(env) { }
//         public override void Enter() { /* animasyon vs */ }
//         public override void Update() { }
//     }

//     public class JumpState : ActionState
//     {
//         public JumpState(EnvironmentState env) : base(env) { }
//         public override void Enter() { /* animasyon vs */ }
//         public override void Update() { }
//     }

//     public class SwimState : ActionState
//     {
//         public SwimState(EnvironmentState env) : base(env) { }
//         public override void Enter() { /* animasyon vs */ }
//         public override void Update() { }
//     }

// }

