using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

#region Movement Controller with State Machine


public class MovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MovementConfigSO defaultConfig;
    
    [Header("State Machine")]
    [SerializeField] private bool useStateMachine = true;
    [SerializeField] private string initialState = "Idle";
    
    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = false;
    
    private Transform _transform;
    private Rigidbody2D _rigidbody2D;

    private MovementStateMachine stateMachine;
    public MovementStateMachine StateMachine => stateMachine;
    
    // Events
    public UnityEvent OnMovementStarted = new UnityEvent();
    public UnityEvent OnMovementCompleted = new UnityEvent();
    public UnityEvent OnStateChanged = new UnityEvent();
    
    // Public properties
    public Transform Transform => _transform;
    public Rigidbody2D Rigidbody2D => _rigidbody2D;
    public MovementConfigSO DefaultConfig => defaultConfig;
    
    private void Awake()
    {
        _transform = transform;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
        if (useStateMachine)
        {
            InitializeStateMachine();
        }
    }

    private void Start()
    {
        if (useStateMachine && stateMachine != null)
        {
            stateMachine.ChangeState();
        }
    }

    private void Update()
    {
        stateMachine?.Update();
    }

    private void FixedUpdate()
    {
        stateMachine?.FixedUpdate();
    }

    #region State Machine Setup

    protected virtual void InitializeStateMachine()
    {
        stateMachine = new MovementStateMachine(this);
        
        // Register default states
        RegisterDefaultStates();
    }

    protected virtual void RegisterDefaultStates()
    {
        RegisterState(new IdleState(stateMachine, this));
    }



    public void RegisterState(MovementState state)
    {
        stateMachine?.RegisterState(state);
    }

    public void ChangeState(MovementState state)
    {
        if (useStateMachine)
        {

        }
    }





    private void OnStateChangedHandler(MovementState state)
    {

    }

    #endregion

    #region Public API - Execute Movements

    public void ExecuteMovement(string movementName)
    {
        if (defaultConfig == null)
        {
            Debug.LogError($"[MovementController] No default config assigned on {gameObject.name}");
            return;
        }

    }

    public void ExecuteMovement(MovementConfigSO config, string movementName)
    {
        if (config == null)
        {
            Debug.LogError($"[MovementController] Config is null");
            return;
        }


    }

    public void ExecuteMovementWithDirection(string movementName, Vector3 direction)
    {
        if (defaultConfig == null) return;


    }

    public void ExecuteMovementWithMagnitude(string movementName, float magnitude)
    {
        if (defaultConfig == null) return;

    }

    public void ExecuteCustomForce(MovementForce force, string identifier = "custom")
    {

    }

    #endregion

    #region Movement Execution

    private void ExecuteMovementForce(MovementForce force, string identifier)
    {
        StopMovement(identifier);

   
        if (showDebugLogs)
            Debug.Log($"[MovementController] Executing '{identifier}' on {gameObject.name}");
    }

    private IEnumerator ExecuteTimedMovement(MovementForce force, string identifier)
    {
        float elapsed = 0f;

        while (elapsed < force.duration)
        {
            ApplyForce(force);
            elapsed += Time.deltaTime;
            yield return null;
        }

    }

    private IEnumerator ExecuteContinuousMovement(MovementForce force, string identifier)
    {
        while (true)
        {
            ApplyForce(force);
            yield return null;
        }
    }

    public void ApplyForce(MovementForce force)
    {
        MovementExecutor.ApplyForce(force, _transform, _rigidbody2D);
    }

    #endregion

    #region Control Methods

    public void StopMovement(string identifier)
    {

    }

    public void StopAllMovements()
    {

        
    }





    #endregion

    private void OnDestroy()
    {
        StopAllMovements();
    }
}

#endregion

