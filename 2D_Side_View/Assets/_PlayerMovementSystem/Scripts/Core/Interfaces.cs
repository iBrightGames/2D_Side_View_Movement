using System.Collections.Generic;
using UnityEngine;

namespace PlayerControlSystem
{
    public interface IPlayerSubState
    {
        void Enter();
        void Update();
        void Exit();
        Vector2 CalculateVelocity();
    }
    
}