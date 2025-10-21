using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerControlSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Movement Configuration")]
    [SerializeField] public List<InputMovementBridge> inputMovementBridges;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = false;

    // Input state
    private Dictionary<(InputMovementBridge, UserInput), float> holdTimers = new();
    private Dictionary<(InputMovementBridge, UserInput), int> clickCounters = new();

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        foreach (var bridge in inputMovementBridges)
        {
            if (bridge.playerInput == null) continue;

            foreach (var input in bridge.playerInput.userInputs)
            {
                bool isDown = CheckInputDown(bridge.playerInput, input);
                Vector2 dir;

                if (CheckTrigger(bridge, input, bridge.playerInput.TriggerType, isDown))
                {
                    dir = GetDirection(input);
                    MovementExecuter.ExecuteMovement(rb, bridge, dir, showDebugLogs);
                }

            }
        }
    }

    private bool CheckInputDown(PlayerControlSystem.PlayerInput playerInput, UserInput userInput)
    {
        if (playerInput.DeviceType == InputDeviceType.Keyboard)
        {
            foreach (var key in userInput.keyCode)
            {
                if (Keyboard.current[key]?.isPressed == true) return true;
            }
        }
        else if (playerInput.DeviceType == InputDeviceType.Mouse)
        {
            foreach (var btn in userInput.mouseButton)
            {
                if (Mouse.current[btn.ToString()]?.IsPressed() == true) return true;
            }
        }

        return false;
    }

    private Vector2 GetDirection(UserInput input)
    {
        float value = input.polarity == InputPolarity.Positive ? 1f : -1f;
        return input.axis == InputAxis.Horizontal ? new Vector2(value, 0) : new Vector2(0, value);
    }

    private bool CheckTrigger(InputMovementBridge bridge, UserInput input, InputTriggerType trigger, bool isDown)
    {
        var key = (bridge, input);

        switch (trigger)
        {
            case InputTriggerType.Pressed:
                if (isDown && !holdTimers.ContainsKey(key))
                {
                    holdTimers[key] = 0f;
                    return true;
                }
                if (!isDown) holdTimers.Remove(key);
                break;

            case InputTriggerType.Released:
                if (holdTimers.ContainsKey(key) && !isDown)
                {
                    holdTimers.Remove(key);
                    return true;
                }
                if (isDown && !holdTimers.ContainsKey(key)) holdTimers[key] = 0f;
                break;

            case InputTriggerType.Held:
                return isDown;

            case InputTriggerType.LongPress:
                if (isDown)
                {
                    if (!holdTimers.ContainsKey(key)) holdTimers[key] = 0f;
                    holdTimers[key] += Time.fixedDeltaTime;
                    if (holdTimers[key] >= bridge.playerInput.PressDuration)
                    {
                        holdTimers[key] = 0f;
                        return true;
                    }
                }
                else holdTimers.Remove(key);
                break;

            case InputTriggerType.MultiClick:
                if (isDown)
                {
                    if (!clickCounters.ContainsKey(key)) clickCounters[key] = 1;
                    else clickCounters[key] += 1;

                    if (clickCounters[key] >= bridge.playerInput.ClickCount)
                    {
                        clickCounters[key] = 0;
                        return true;
                    }
                }
                else clickCounters.Remove(key);
                break;
        }

        return false;
    }
}

// // ============================================
// // PLAYER MOVEMENT CONTROLLER
// // ============================================

// using System.Collections.Generic;
// using UnityEngine;
// using PlayerControlSystem;

// [RequireComponent(typeof(Rigidbody2D))]
// public class PlayerMovementController : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private Rigidbody2D rb;

//     [Header("Movement Configuration")]
//     [SerializeField] public List<InputMovementBridge> inputMovementBridges;


//     [Header("Debug")]
//     [SerializeField] private bool showDebugLogs = false;


//     void Awake()
//     {
//         if (rb == null)
//         {
//             rb = GetComponent<Rigidbody2D>();
//         }

//         if (showDebugLogs)
//         {
//             Debug.Log($"[Movement] Awake - Rigidbody2D: {rb != null}, Bridges: {inputMovementBridges.Count}");
//         }
//     }

//     private void FixedUpdate()
//     {
//         foreach (var bridge in inputMovementBridges)
//         {
//             if (bridge.playerInput == null) continue;
//             foreach (var input in bridge.playerInput.userInputs)
//             {
//                 Vector2 dir = input.GetDirection();
//                 MovementExecuter.ExecuteMovement(rb, bridge, dir, showDebugLogs);
//             }
//         }
//     }

// }

