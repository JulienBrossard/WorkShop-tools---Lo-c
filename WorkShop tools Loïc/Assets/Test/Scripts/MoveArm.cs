using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class MoveArm : MonoBehaviour
{
    PlayerInputs inputs;
    [Header("Rigs")]
    [SerializeField] private Rig leftArmRig;
    [SerializeField] private Rig rightArmRig;
    
    [Header("Targets")]
    [SerializeField] private Transform leftArmTarget;
    [SerializeField] private Transform rightArmTarget;
    [SerializeField] private Transform leftArmTargetRotationCenter;
    [SerializeField] private Transform rightArmTargetRotationCenter;
    [SerializeField] private Transform nextTargetPos;
    private Vector3 leftArmTargetSpawn;
    private Vector3 rightArmTargetSpawn;

    [Header("Player Data")] [SerializeField]
    private MyPlayerData playerData;

    private Vector2 rotate;

    private void Start()
    {
        inputs = InputManager.instance.playerInputs;
        leftArmTargetSpawn = leftArmTarget.localPosition;
        rightArmTargetSpawn = rightArmTarget.localPosition;
    }

    private void Update()
    {
        ActiveArm();
    }

    void ActiveArm()
    {
        rotate = inputs.PlayerAction.MoveArm.ReadValue<Vector2>();
        if (inputs.PlayerAction.ActiveLeftArm.IsPressed())
        {
            leftArmRig.weight = Mathf.Lerp(leftArmRig.weight, 1,  playerData.activeArmAcceleration * Time.deltaTime);
        }
        else
        {
            leftArmRig.weight = Mathf.Lerp(leftArmRig.weight, 0, playerData.activeArmAcceleration * Time.deltaTime);
        }
        
        if(inputs.PlayerAction.ActiveRightArm.IsPressed())
        {
            rightArmRig.weight = Mathf.Lerp(rightArmRig.weight, 1, playerData.activeArmAcceleration * Time.deltaTime);
        }
        else
        {
            rightArmRig.weight = Mathf.Lerp(rightArmRig.weight, 0, playerData.activeArmAcceleration * Time.deltaTime);
        }

        if ((inputs.PlayerAction.ActiveRightArm.IsPressed() || inputs.PlayerAction.ActiveLeftArm.IsPressed()) && rotate != Vector2.zero)
        {
            if (rotate != Vector2.zero)
            {
                nextTargetPos.position = leftArmTarget.position;
                RotateAround(nextTargetPos, leftArmTargetRotationCenter.position, -transform.right, playerData.armAngularSpeed*Time.deltaTime*Mathf.Sign(rotate.y));
                if ((nextTargetPos.localPosition.y<0.5f && nextTargetPos.localPosition.y>-0.1f) || (nextTargetPos.localPosition.y>0.5f && rotate.y>0) || (nextTargetPos.localPosition.y<-0.1f && rotate.y<0))
                {
                    RotateAround(leftArmTarget, leftArmTargetRotationCenter.position, transform.right, playerData.armAngularSpeed*Time.deltaTime*Mathf.Sign(rotate.y));
                    RotateAround(rightArmTarget, rightArmTargetRotationCenter.position, transform.right, playerData.armAngularSpeed*Time.deltaTime*Mathf.Sign(rotate.y));
                }
            }
        }
        
        else
        {
            rightArmTarget.localPosition = Vector3.Lerp(rightArmTarget.localPosition, rightArmTargetSpawn, Time.deltaTime);
            leftArmTarget.localPosition = Vector3.Lerp(leftArmTarget.localPosition, leftArmTargetSpawn, Time.deltaTime);
            rightArmTarget.eulerAngles = Vector3.zero;
            leftArmTarget.eulerAngles = Vector3.zero;
        }
    }

    void RotateAround(Transform go, Vector3 point, Vector3 axis, float angle)
    {
        Vector3 position = go.position;
        Vector3 vector3 = Quaternion.AngleAxis(angle, axis) * (position - point);
        go.position = point + vector3;
    }

}
