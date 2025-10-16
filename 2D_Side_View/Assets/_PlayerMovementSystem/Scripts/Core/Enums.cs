using UnityEngine;

public enum ForceType { Linear, Angular }

public enum InputType
{
    Continuous,  // Held input, sürekli hareket
    Triggered    // Tek sefer tetiklenen input
}

public enum DirectionSource
{
    FromInput,        // Input'tan direction oku
    Manual,           // Sabit direction kullan (jump için Vector2.up gibi)
    CurrentVelocity   // Rigidbody'nin velocity direction'ı (dash için)
}