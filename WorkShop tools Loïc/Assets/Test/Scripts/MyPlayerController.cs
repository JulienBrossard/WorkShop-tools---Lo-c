using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class MyPlayerController : MonoBehaviour
{
    public static MyPlayerController instance;
    
    [Header("PlayerData")] 
    [SerializeField] MyPlayerData playerData;

    [Header("Rigidbody")] 
    [SerializeField] private Rigidbody rb;

    [Header("Jump")]
    RaycastHit hit;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float maxDistance = 2f;
    private bool isGrounded;

    [Header("Stairs")] 
    [SerializeField] private Vector3 foot;
    [SerializeField] private Vector3 middle;
    [SerializeField] private float maxDistanceStairs = 1;

    [Header("Animations")] 
    [SerializeField]
    private Animator animator;
    [SerializeField]
    float velocityAnim;

    [Header("Camera")] 
    [SerializeField] private Transform cameraRotationCenter;
    [SerializeField] Transform cameraRotationInitialisation;

    [Header("Spin")] 
    [SerializeField] private Rig spinRig;
    
    private PlayerInputs inputs;
    private Vector2 movement;
    private Vector3 targetVelocity;
    private Vector2 rotate;
    private float speedY;
    private float speedX;
    private float currentSpeedY;
    private float currentSpeedX;

    private bool canMove = true;
    private bool canJump = true;
    private bool canRotate = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inputs = InputManager.instance.playerInputs;
        cameraRotationCenter.eulerAngles = Vector3.zero;
    }

    private void Update()
    {
        Movement();   
        Rotate();
        Jump();

        if (Input.GetKeyDown(KeyCode.V))
        {
            animator.SetBool("HandRaising", !animator.GetBool("HandRaising"));
        }
    }

    void Movement()
    {
        if (inputs.PlayerMovement.Move.ReadValue<Vector2>() != Vector2.zero && canMove)
        {
            movement = inputs.PlayerMovement.Move.ReadValue<Vector2>();
            animator.SetBool("isIdle", false);
            if (inputs.PlayerMovement.Run.IsPressed())
            {
                if (movement.y < 0)
                {
                    speedY = playerData.walkSpeed;
                }
                else
                {
                    speedY = playerData.runSpeed;
                }
                if (movement.y > 0)
                {
                    speedX = playerData.runSpeed;
                }
                else
                {
                    speedX = playerData.walkSpeed;
                }
            }
            else
            {
                speedX = playerData.walkSpeed;
                speedY = playerData.walkSpeed;
            }
            if (inputs.PlayerMovement.Move.ReadValue<Vector2>().y != 0)
            {
                currentSpeedY = Mathf.Lerp(currentSpeedY, speedY * movement.y, Time.deltaTime * playerData.acceleration);
            }
            else
            {
                speedY = 0;
                currentSpeedY = Mathf.Lerp(currentSpeedY, speedY, Time.deltaTime * playerData.deceleration);
            }

            if (inputs.PlayerMovement.Move.ReadValue<Vector2>().x != 0)
            {
                currentSpeedX = Mathf.Lerp(currentSpeedX, speedX  * movement.x, Time.deltaTime * playerData.acceleration);
            }
            else
            {
                speedX = 0;
                currentSpeedX = Mathf.Lerp(currentSpeedX, speedX, Time.deltaTime * playerData.deceleration);
            }
            
            
            targetVelocity = (transform.forward * movement.y * speedY + transform.right * movement.x * speedX) *
                Time.deltaTime + new Vector3(0, rb.velocity.y, 0);
            rb.velocity = (transform.forward * currentSpeedY + transform.right * currentSpeedX) * Time.deltaTime + new Vector3(0, rb.velocity.y, 0);
            
            /*if (Physics.Raycast(foot, transform.forward, maxDistanceStairs, layer) && !Physics.Raycast(middle, transform.forward, maxDistanceStairs, layer))
            {
                transform.position += Vector3.up * 10;
            }*/

            if (movement.y != 0)
            {
                animator.SetFloat("Speed", Mathf.SmoothDamp(animator.GetFloat("Speed"), currentSpeedY, ref velocityAnim, playerData.accelerationAnim));
            }
            else
            {
                animator.SetFloat("Speed", Mathf.SmoothDamp(animator.GetFloat("Speed"), currentSpeedY, ref velocityAnim, playerData.accelerationAnim));
            }
            if (movement.x != 0)
            {
                animator.SetFloat("XSpeed", Mathf.SmoothDamp(animator.GetFloat("XSpeed"), currentSpeedX, ref velocityAnim, playerData.accelerationAnim));
            }
            else
            {
                animator.SetFloat("XSpeed", Mathf.SmoothDamp(animator.GetFloat("XSpeed"), currentSpeedX, ref velocityAnim, playerData.accelerationAnim));
            }
        }
        else if(rb.velocity.x != 0 || rb.velocity.z != 0 )
        {
            if (rb.velocity.x != 0)
            {
                currentSpeedX = Mathf.Lerp(currentSpeedX, 0, Time.deltaTime * playerData.deceleration);
            }

            if (rb.velocity.y != 0)
            {
                currentSpeedY = Mathf.Lerp(currentSpeedY, 0, Time.deltaTime * playerData.deceleration);
            }
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero + new Vector3(0, rb.velocity.y, 0), Time.deltaTime * playerData.deceleration);
            animator.SetFloat("Speed", Mathf.SmoothDamp(animator.GetFloat("Speed"), currentSpeedY, ref velocityAnim, playerData.accelerationAnim));
            animator.SetFloat("XSpeed", Mathf.SmoothDamp(animator.GetFloat("XSpeed"), currentSpeedX, ref velocityAnim, playerData.accelerationAnim));
            speedX = 0;
            speedY = 0;
        }
        else if(isGrounded)
        {
            animator.SetBool("isIdle", true);
        }
    }

    void Rotate()
    {
        if (inputs.PlayerMovement.Look.ReadValue<Vector2>().x != 0 && canRotate)
        {
            rotate = inputs.PlayerMovement.Look.ReadValue<Vector2>();
            transform.Rotate(Vector3.up*playerData.angularSpeed*Time.deltaTime*Mathf.Sign(rotate.x));
        }

        if (inputs.PlayerMovement.Look.ReadValue<Vector2>().y != 0 && canRotate)
        {
            rotate = inputs.PlayerMovement.Look.ReadValue<Vector2>();
            if ((cameraRotationCenter.eulerAngles.x > -0.1f && cameraRotationCenter.eulerAngles.x < playerData.maxAngle) || (cameraRotationCenter.eulerAngles.x > 350 && cameraRotationCenter.eulerAngles.x < 360))
            {
                cameraRotationCenter.Rotate(Vector3.right*playerData.angularSpeedCamera*Time.deltaTime*Mathf.Sign(rotate.y));
            }

            if ((cameraRotationCenter.eulerAngles.x > -1 && cameraRotationCenter.eulerAngles.x < playerData.maxAngle))
            {
                spinRig.weight = cameraRotationCenter.eulerAngles.x / playerData.maxAngle;
            }
        }
        else
        {
            cameraRotationCenter.rotation = Quaternion.Lerp(cameraRotationCenter.rotation, cameraRotationInitialisation.rotation, Time.deltaTime);
            spinRig.weight = Mathf.Lerp(spinRig.weight, 0, Time.deltaTime);
        }
    }

    void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance, layer))
        {
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            if (inputs.PlayerMovement.Jump.WasPressedThisFrame() && canJump)
            {
                rb.AddForce(transform.up * playerData.jumpForce, ForceMode.Impulse);
                animator.SetBool("isJumping", true);
                animator.SetBool("isIdle", false);
            }

            if (rb.velocity.y < -6)
            {
                Fall();
            }
        }
        else
        {
            isGrounded = false;
            animator.SetBool("isJumping", false);
            animator.SetBool("isGrounded", false);
            animator.SetBool("isIdle", false);
        }
    }

    private void Fall()
    {
        CantControl();
        canRotate = false;
        animator.SetBool("isFall", true);
    }

    public void StandUp()
    {
        CanControl();
        canRotate = true;
        animator.SetBool("isFall", false);
    }

    public void CantControl()
    {
        canJump = false;
        canMove = false;
    }

    public void CanControl()
    {
        canJump = true;
        canMove = true;
    }
}
