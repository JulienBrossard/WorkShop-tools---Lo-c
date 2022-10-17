using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [FormerlySerializedAs("HorizontalSpeed")] public float horizontalSpeed = 6f;
    public AnimationCurve acceleration;
    public float jumpForce = 5f;
    public float angularSpeed = 0.1f;
    public float turnSmoohtTime;
    public float jumpHeight = 5;
    public float gravity = -0.81f;
}
