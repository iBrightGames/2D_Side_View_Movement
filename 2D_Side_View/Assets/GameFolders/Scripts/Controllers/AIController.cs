using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/InputController/AIController")]
public class AIController : InputController
{
    public override bool RetrieveJumpInput() => true;

    public override float RetrieveMoveInput() => 1f;
}