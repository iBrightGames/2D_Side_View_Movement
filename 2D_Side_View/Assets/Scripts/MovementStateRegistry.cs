// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.Events;
// using System;


// #region State Registry Component

// /// <summary>
// /// Component to easily register states in the inspector
// /// </summary>
// public class MovementStateRegistry : MonoBehaviour
// {
//     [SerializeField] private MovementController controller;
//     [SerializeField] private StateConfiguration[] stateConfigs;

//     private void Awake()
//     {
//         if (controller == null)
//             controller = GetComponent<MovementController>();

//         RegisterStates();
//     }

//     private void RegisterStates()
//     {
//         foreach (var config in stateConfigs)
//         {
//             MovementState state = CreateState(config);
//             if (state != null)
//             {
//                 controller.RegisterState(state);
//             }
//         }
//     }

//     private MovementState CreateState(StateConfiguration config)
//     {

//     }
// }

// [System.Serializable]
// public class StateConfiguration
// {
//     public string movementName = "Name";
    
// }


// #endregion

