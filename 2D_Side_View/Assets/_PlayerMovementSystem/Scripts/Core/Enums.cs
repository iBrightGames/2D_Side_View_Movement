


namespace PlayerControlSystem
{
    public enum InputAxis { Horizontal, Vertical }
    public enum InputPolarity { Positive, Negative }
    public enum ForceType
    {
        AddForce,          // Continuous force
        AddImpulse,        // One-shot force
        AddTorque,         // Continuous rotation
        AddAngularImpulse, // One-shot rotation
        SetVelocity        // Direct velocity
    }


}