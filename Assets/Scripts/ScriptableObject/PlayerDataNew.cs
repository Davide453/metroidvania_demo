using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataNew", menuName = "Scriptable Objects/PlayerDataNew")]
public class PlayerDataNew : ScriptableObject
{
    [Header("Run")]
    public float speed;

    [Header("Jump")]
    public float jumpForce; //The actual force applied (upwards) to the player when they jump.


    [Range(0.01f, 0.5f)] public float coyoteTime; //Grace period after falling off a platform, where you can still jump
}
