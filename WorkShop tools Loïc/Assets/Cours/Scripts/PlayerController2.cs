using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController2 : MonoBehaviour
{
    [Header("PlayerData")]
    [SerializeField] PlayerData playerData;
    [Header("Character Controller")]
    [SerializeField] private CharacterController characterController;
    private PlayerInputs inputs;
    
    [Header("Camera")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] float turnSmoothVelocity;
    private float targetAngle;
    private float angle;
    private Vector3 moveDir;
    

    private Vector2 move;
    private Vector3 direction;

    [Header("Jump")]
    [SerializeField] LayerMask layer;
    bool isGrounded;
    private float groundDistance;
    private Vector3 velocity;
    

    private void Start()
    {
        inputs = new PlayerInputs();
        inputs.Enable();
    }

    private void Update()
    {
        move = inputs.PlayerMovement.Move.ReadValue<Vector2>();
        direction = new Vector3(move.x, 0, move.y);

        if (direction.magnitude > 0.1f)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, playerData.turnSmoohtTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * playerData.horizontalSpeed * Time.deltaTime);

        }
        Jump();
    }

    void Jump()
    {
        if (inputs.PlayerMovement.Jump.WasPressedThisFrame() && Physics.Raycast(transform.position, Vector3.down, 2f, layer))
        {
            velocity.y = Mathf.Sqrt(playerData.jumpHeight * -2f * playerData.gravity);
        }

        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        velocity.y += playerData.gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
