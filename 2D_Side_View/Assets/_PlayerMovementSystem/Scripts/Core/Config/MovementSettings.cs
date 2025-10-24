using UnityEngine;

namespace PlayerControlSystem
{
    [CreateAssetMenu(fileName = "MovementSettings", menuName = "PlayerControlSystem/Movement Settings")]
    public class MovementSettings : ScriptableObject
    {
        [Header("Ground Detection")]
        [Tooltip("Hangi layer'lar zemin sayılacak")]
        public LayerMask groundLayers = 1; // Default layer

        [Tooltip("Raycast mesafesi (karakter pivot'undan aşağı)")]
        public float groundCheckDistance = 0.1f;

        [Tooltip("Raycast başlangıç noktası offset (pivot'tan)")]
        public Vector2 groundCheckOffset = Vector2.zero;

        [Header("Coyote Time")]
        [Tooltip("Yerden ayrıldıktan sonra kısa süre jump yapabilme")]
        public bool useCoyoteTime = true;

        [Tooltip("Coyote time süresi (saniye)")]
        public float coyoteTimeDuration = 0.15f;

        [Header("Air Control")]
        [Tooltip("Havadayken yatay kontrole izin ver")]
        public bool allowAirControl = false;

        [Tooltip("Havada force çarpanı (0-1 arası)")]
        [Range(0f, 1f)]
        public float airControlMultiplier = 0.5f;

        [Header("Debug")]
        public bool showDebugLogs = false;
        public bool showGroundCheckGizmos = true;
        public Color gizmoColorGrounded = Color.green;
        public Color gizmoColorAirborne = Color.red;
    }
}


