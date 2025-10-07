using UnityEngine;
using UnityEngine.Events;




public class MovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Transform _transform;


    public UnityEvent OnMovementStarted = new UnityEvent();
    public UnityEvent OnMovementCompleted = new UnityEvent();

    private void Awake()
    {
        if (_transform == null) _transform = transform;
        if (_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
    }


    public void ApplyForce(MovementForceSO[] force)
    {
        foreach (var f in force)
        {
            MovementExecutor.ApplyForce(f, _transform, _rigidbody2D);
        }
        
    }

    public void ApplyForce(MovementForceSO force)
    {
        MovementExecutor.ApplyForce(force, _transform, _rigidbody2D);
    }
        
        

}





