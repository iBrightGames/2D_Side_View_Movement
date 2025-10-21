// ============================================
// FORCE CONFIGS
// ============================================

using UnityEngine;

public enum ForceType
{
    Linear
}
[CreateAssetMenu(fileName ="Force", menuName ="Force")]

public class ForceConfig : ScriptableObject
{
    [Header("Force Settings")]
    public ForceType forceType;
    public ForceMode2D forceMode2D = ForceMode2D.Force;
    public float forceMagnitude = 10f;
    

}
