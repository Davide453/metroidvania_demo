using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataNew", menuName = "Scriptable Objects/PlayerDataNew")]
public class PlayerDataNew : ScriptableObject
{
    [Header("Run")]
    public float speed;

    [Header("Jump")]
    public float jumpForce; //The actual force applied (upwards) to the player when they jump.
    public Vector2 wallJumpForce;
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime; //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.
    [Range(0.01f, 0.5f)] public float coyoteTime; //Grace period after falling off a platform, where you can still jump
}
