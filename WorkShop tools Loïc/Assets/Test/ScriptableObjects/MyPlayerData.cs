using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Player Data", menuName = "My Player Data")]
public class MyPlayerData : ScriptableObject
{
    [Header("Speed")]
    public float walkSpeed;
    public float runSpeed;
    public float armAngularSpeed;
    public float angularSpeedCamera;
    [Header("Acceleration")]
    public float angularSpeed;
    public float acceleration;
    public float deceleration;
    [Header("Force")]
    public float jumpForce;
    [Header("Animation")]
    public float accelerationAnim;
    [Header("Arms")]
    public float activeArmAcceleration;
    [Header("Rotation")] 
    public float maxAngle;
}
