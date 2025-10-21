using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControlSystem
{
    // public class PlayerInputManager : MonoBehaviour
    // {
    //     public List<PlayerInput> playerInputs;
    //     private Dictionary<(PlayerInput, UserInput), float> holdTimers = new();
    //     private Dictionary<(PlayerInput, UserInput), int> clickCounters = new();

    //     void Update()
    //     {
    //         foreach (var playerInputSO in playerInputs)
    //         {
    //             if (playerInputSO == null) continue;

    //             foreach (var userInput in playerInputSO.userInputs)
    //             {
    //                 bool triggered = false;

    //                 if (playerInputSO.DeviceType == InputDeviceType.Keyboard)
    //                     triggered = CheckKeyboardInput(playerInputSO, userInput);
    //                 else if (playerInputSO.DeviceType == InputDeviceType.Mouse)
    //                     triggered = CheckMouseInput(playerInputSO, userInput);

    //                 if (triggered)
    //                 {
    //                     var value = userInput.GetDirection();
    //                     // aksiyona uygula, test için debug
    //                     Debug.Log($"Triggered Value: {value}");
    //                 }
    //             }
    //         }
    //     }

    //     private bool CheckKeyboardInput(PlayerInput playerInputSO, UserInput userInput)
    //     {
    //         foreach (var key in userInput.keyCode)
    //         {
    //             var kbKey = Keyboard.current[key];
    //             if (kbKey == null) continue;

    //             if (CheckTrigger(playerInputSO, userInput, Keyboard.current[key].isPressed))
    //                 return true;
    //         }
    //         return false;
    //     }

    //     private bool CheckMouseInput(PlayerInput playerInputSO, UserInput userInput)
    //     {
    //         foreach (var btn in userInput.mouseButton)
    //         {
    //             var mouseBtn = Mouse.current[btn.ToString()];
    //             if (mouseBtn == null) continue;

    //             if (CheckTrigger(playerInputSO, userInput, mouseBtn.IsPressed()))
    //                 return true;
    //         }
    //         return false;
    //     }

    //     private bool CheckTrigger(PlayerInput playerInputSO, UserInput userInput, bool isDown)
    //     {
    //         var keyPair = (playerInputSO, userInput);

    //         switch (playerInputSO.TriggerType)
    //         {
    //             case InputTriggerType.Pressed:
    //                 if (isDown && !holdTimers.ContainsKey(keyPair))
    //                 {
    //                     holdTimers[keyPair] = 0f;
    //                     return true;
    //                 }    
    //                 else if (!isDown)
    //                 {
    //                     holdTimers.Remove(keyPair); // tuş bırakıldı, tekrar basışa izin ver
    //                 }
    //                 break;

    //             case InputTriggerType.Released:
    //                 if (holdTimers.ContainsKey(keyPair) && !isDown)
    //                 {
    //                     holdTimers.Remove(keyPair);
    //                     return true;
    //                 }
    //                 if (isDown && !holdTimers.ContainsKey(keyPair))
    //                     holdTimers[keyPair] = 0f;
    //                 break;

    //             case InputTriggerType.Held:
    //                 return isDown;

    //             case InputTriggerType.LongPress:
    //                 if (isDown)
    //                 {
    //                     if (!holdTimers.ContainsKey(keyPair)) holdTimers[keyPair] = 0f;
    //                     holdTimers[keyPair] += Time.deltaTime;
    //                     if (holdTimers[keyPair] >= playerInputSO.PressDuration)
    //                     {
    //                         holdTimers[keyPair] = 0f;
    //                         return true;
    //                     }
    //                 }
    //                 else holdTimers.Remove(keyPair);
    //                 break;

    //             case InputTriggerType.MultiClick:
    //                 if (isDown)
    //                 {
    //                     if (!clickCounters.ContainsKey(keyPair)) clickCounters[keyPair] = 1;
    //                     else clickCounters[keyPair] += 1;

    //                     if (clickCounters[keyPair] >= playerInputSO.ClickCount)
    //                     {
    //                         clickCounters[keyPair] = 0;
    //                         return true;
    //                     }
    //                 }
    //                 else if (!isDown) clickCounters.Remove(keyPair);
    //                 break;
    //         }

    //         return false;
    //     }
    // }

}

