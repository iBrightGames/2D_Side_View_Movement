// using UnityEngine;

// #region Demo Player Controller with State Machine

// /// <summary>
// /// Example player controller using state machine
// /// </summary>
// public class PlayerMovementDemo : MonoBehaviour
// {
//     [SerializeField] private MovementController controller;
    
//     [Header("Input")]
//     [SerializeField] private KeyCode jumpKey = KeyCode.Space;
//     [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;
//     [SerializeField] private KeyCode runKey = KeyCode.LeftControl;
    
//     [Header("Ground Check")]
//     [SerializeField] private Transform groundCheck;
//     [SerializeField] private float groundCheckRadius = 0.2f;
//     [SerializeField] private LayerMask groundLayer;
    

//     private bool isGrounded;
    
//     private void Start()
//     {
//         SetupStates();
//     }
    
//     private void SetupStates()
//     {

//     }
    
//     private void Update()
//     {
//         CheckGround();
//         HandleInput();
//     }
    
//     private void CheckGround()
//     {
//         if (groundCheck != null)
//         {
//             isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
            

//         }
//     }
    
//     private void HandleInput()
//     {
//         float h = Input.GetAxis("Horizontal");
//         Vector3 moveDir = new Vector3(h, 0, 0);
        
//         // Jump
//         if (Input.GetKeyDown(jumpKey) && isGrounded)
//         {
            
//             controller.ChangeState("Jump");
//             return;
//         }
        
//         // Dash
//         if (Input.GetKeyDown(dashKey))
//         {
//             controller.ChangeState("Dash");
//             return;
//         }
        

//     }
    
//     private void OnDrawGizmosSelected()
//     {
//         if (groundCheck != null)
//         {
//             Gizmos.color = isGrounded ? Color.green : Color.red;
//             Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
//         }
//     }
// }

// #endregion

