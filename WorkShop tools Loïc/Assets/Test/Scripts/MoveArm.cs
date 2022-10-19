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

        if ((inputs.PlayerAction.ActiveRightArm.IsPressed() || inputs.PlayerAction.ActiveLeftArm.IsPressed()))
        {
            if (rotate != Vector2.zero)
            {
                leftArmTarget.RotateAround(leftArmTargetRotationCenter.position, -transform.right, playerData.armAngularSpeed*Time.deltaTime*Mathf.Sign(rotate.y)); 
                rightArmTarget.RotateAround(rightArmTargetRotationCenter.position, -transform.right, playerData.armAngularSpeed*Time.deltaTime*Mathf.Sign(rotate.y));   
            }
        }
        
        else
        {
            rightArmTarget.localPosition = rightArmTargetSpawn;
            
            leftArmTarget.localPosition = leftArmTargetSpawn;
        }
    }
    
}
