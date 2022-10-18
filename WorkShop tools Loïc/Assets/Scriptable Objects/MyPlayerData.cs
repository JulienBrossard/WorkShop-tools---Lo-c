using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Player Data", menuName = "My Player Data")]
public class MyPlayerData : ScriptableObject
{
    public float walkSpeed;
    public float runSpeed;
    public float acceleration;
    public float deceleration;
    public float angularSpeed;
    public float jumpForce;
    public float accelerationAnim;
}
