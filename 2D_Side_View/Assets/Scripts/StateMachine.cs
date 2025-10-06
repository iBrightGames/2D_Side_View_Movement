using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;


#region State Machine Core

public abstract class MovementState
{
    protected MovementStateMachine stateMachine;
    protected MovementController controller;

    public MovementState(MovementStateMachine sm, MovementController ctrl)
    {
        stateMachine = sm;
        controller = ctrl;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual bool CanTransitionTo(string newState) { return true; }
}

public class MovementStateMachine
{
    private Dictionary<string, MovementState> states = new Dictionary<string, MovementState>();
    private MovementState currentState;
    private MovementController controller;

    public MovementState CurrentState => currentState;

    public UnityEvent OnStateChanged = new UnityEvent();

    public MovementStateMachine(MovementController ctrl)
    {
        controller = ctrl;
    }

    public void RegisterState(MovementState state)
    {

    }

    public void ChangeState()
    {

    }

    public void Update()
    {
        currentState?.Update();
    }

    public void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }



    public T GetState<T>(string stateName) where T : MovementState
    {
        if (states.TryGetValue(stateName, out MovementState state))
        {
            return state as T;
        }
        return null;
    }
    
}

#endregion


public class IdleState : MovementState
{
    public IdleState(MovementStateMachine sm, MovementController controller) : base(sm, controller)
{
}


    public override void Enter() { }
    public override void Update() { }
    public override void Exit() { }
}
