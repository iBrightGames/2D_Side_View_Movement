
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControlSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {
        public Rigidbody2D rb;
        public List<InputForceBridge> inputMovementBridges;

        public bool showDebugLogs = true;

        [HideInInspector] public List<InputForceBridge> activeBridges = new();

        private Dictionary<UserInput, System.Action<InputAction.CallbackContext>> eventHandlers
            = new Dictionary<UserInput, System.Action<InputAction.CallbackContext>>();

        private PlayerStateMachine stateMachine;

        void Awake()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            foreach(var bridge in inputMovementBridges)
            {
                bridge.playerInput.Subscribe();
            }

            // State machine oluştur
            stateMachine = new PlayerStateMachine(this);
        }
        void Update()
        {
            stateMachine.Update();
        }
        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        private void OnDisable()
        {
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            foreach(var bridge in inputMovementBridges)
            {
                bridge.playerInput.Unsubscribe();
            }

            eventHandlers.Clear();
        }

    }
}


