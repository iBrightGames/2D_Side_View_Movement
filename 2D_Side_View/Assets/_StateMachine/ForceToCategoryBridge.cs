
using PlayerControlSystem;
using UnityEngine;

[CreateAssetMenu(menuName="Character/ForceToCategoryBridge")]
public class ForceToCategoryBridge : ScriptableObject
{
    public UserInput playerInput;
    public string categoryName; // SpriteDatabase category ile eşleşecek
}
