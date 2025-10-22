
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControlSystem
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D rb;

        [Header("Movement Configuration")]
        [SerializeField] public List<InputMovementBridge> inputMovementBridges;

        [Header("Debug")]
        [SerializeField] private bool showDebugLogs = false;

        // Event handler tracking
        private Dictionary<UserInput, System.Action<InputAction.CallbackContext>> eventHandlers
            = new Dictionary<UserInput, System.Action<InputAction.CallbackContext>>();

        void Awake()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }

            InitializeInputs();
        }

        private void InitializeInputs()
        {
            foreach (var bridge in inputMovementBridges)
            {
                if (bridge.playerInput == null) continue;


                if (bridge.playerInput.action == null) continue;

                var action = bridge.playerInput.action.action;

                // Enable action
                if (!action.enabled)
                {
                    action.Enable();
                }

                // Handler oluştur (bridge'i closure ile yakala)
                System.Action<InputAction.CallbackContext> handler = ctx =>
                {
                    OnInputEvent(bridge, bridge.playerInput, ctx);
                };

                eventHandlers[bridge.playerInput] = handler;

                // Tüm event'lere subscribe ol
                action.started += handler;
                action.performed += handler;
                action.canceled += handler;

                if (showDebugLogs)
                {
                    Debug.Log($"[Input] Subscribed: {action.name}");
                }
            }
        }



        private void OnInputEvent(InputMovementBridge bridge, UserInput userInput, InputAction.CallbackContext ctx)
        {
            if (showDebugLogs)
            {
                Debug.Log($"[Input] Event: {ctx.phase}, Action: {ctx.action.name}");
            }

            // Movement uygula
            MovementExecuter.ExecuteMovement(rb, userInput, bridge, showDebugLogs);
        }

        private void OnDisable()
        {
            foreach (var pair in eventHandlers)
            {
                var userInput = pair.Key;
                var handler = pair.Value;

                if (userInput.action == null) continue;
                var action = userInput.action.action;

                // Unsubscribe
                action.started -= handler;
                action.performed -= handler;
                action.canceled -= handler;

                action.Disable();
            }

            eventHandlers.Clear();
        }


    }


}