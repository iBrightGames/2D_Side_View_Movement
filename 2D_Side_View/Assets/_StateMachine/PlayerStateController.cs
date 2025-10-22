using UnityEngine;
using PlayerControlSystem;
using SpriteDatabaseAnimation;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] SpriteDatabaseAnimator sprite;
    [SerializeField] ForceToCategoryBridge[] forceToCategoryBridges;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] string defaultCategory;

        // Event handler tracking
        private Dictionary<UserInput, System.Action<InputAction.CallbackContext>> eventHandlers
            = new Dictionary<UserInput, System.Action<InputAction.CallbackContext>>();

        void Awake()
        {
            
            InitializeInputs();
        }

        private void InitializeInputs()
        {
            foreach (var bridge in forceToCategoryBridges)
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
                    OnInputEvent(bridge, ctx);
                };

                eventHandlers[bridge.playerInput] = handler;

                // Tüm event'lere subscribe ol
                action.started += handler;
                action.performed += handler;
                action.canceled += handler;
            }
        }


    private void OnInputEvent(ForceToCategoryBridge bridge, InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
            sprite.SetCategory(defaultCategory);
        else
            sprite.SetCategory(bridge.categoryName);

        if (ctx.performed || ctx.started)
            UpdateFlip(bridge.playerInput.GetDirection());
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



    void UpdateFlip(Vector2 inputDir)
    {
        if (inputDir.x > 0.01f) spriteRenderer.flipX = false;
        else if (inputDir.x < -0.01f) spriteRenderer.flipX = true;

        if (inputDir.y > 0.01f) spriteRenderer.flipY = false;
        else if (inputDir.y < -0.01f) spriteRenderer.flipY = true;
    }


}
