using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/InputController/PlayerController")]
public class PlayerController : InputController
{
    public override bool RetrieveJumpInput() => Input.GetKeyDown(KeyCode.Space);

    public override float RetrieveMoveInput() => Input.GetAxisRaw("Horizontal");
}