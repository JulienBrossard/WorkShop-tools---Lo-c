using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MyPlayerController : MonoBehaviour
{
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
    
    private PlayerInputs inputs;
    private Vector2 movement;
    private Vector3 targetVelocity;
    private Vector2 rotate;
    private float speed;
    private float currentSpeed;

    private void Awake()
    {
        inputs = new PlayerInputs();
        inputs.Enable();
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
        if (inputs.PlayerMovement.Move.ReadValue<Vector2>() != Vector2.zero && isGrounded)
        {
            animator.SetBool("isIdle", false);
            if (inputs.PlayerMovement.Run.IsPressed())
            {
                speed = playerData.runSpeed;
            }
            else
            {
                speed = playerData.walkSpeed;
            }
            movement = inputs.PlayerMovement.Move.ReadValue<Vector2>();
            targetVelocity = (transform.forward * movement.y + transform.right * movement.x) * speed *
                             Time.deltaTime + new Vector3(0, rb.velocity.y, 0);
            currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * playerData.acceleration);
            rb.velocity = (transform.forward * movement.y + transform.right * movement.x) * currentSpeed * Time.deltaTime + new Vector3(0, rb.velocity.y, 0);
            if (Physics.Raycast(foot, transform.forward, maxDistanceStairs, layer) && !Physics.Raycast(middle, transform.forward, maxDistanceStairs, layer))
            {
                transform.position += Vector3.up * 10;
            }
            
            animator.SetFloat("Speed", Mathf.SmoothDamp(animator.GetFloat("Speed"), currentSpeed, ref velocityAnim, playerData.accelerationAnim));
        }
        else if(rb.velocity.x != 0 || rb.velocity.z != 0 )
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * playerData.deceleration);
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero + new Vector3(0, rb.velocity.y, 0), Time.deltaTime * playerData.deceleration);
            animator.SetFloat("Speed", Mathf.SmoothDamp(animator.GetFloat("Speed"), currentSpeed, ref velocityAnim, playerData.accelerationAnim));
        }
        else if(isGrounded)
        {
            animator.SetBool("isIdle", true);
        }
    }

    void Rotate()
    {
        if (inputs.PlayerMovement.Look.ReadValue<Vector2>() != Vector2.zero)
        {
            rotate = inputs.PlayerMovement.Look.ReadValue<Vector2>();
            transform.Rotate(Vector3.up*playerData.angularSpeed*Time.deltaTime*Mathf.Sign(rotate.x));
        }
    }

    void Jump()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxDistance, layer))
        {
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            if (inputs.PlayerMovement.Jump.WasPressedThisFrame() )
            {
                rb.AddForce(transform.up * playerData.jumpForce, ForceMode.Impulse);
                animator.SetBool("isJumping", true);
                animator.SetBool("isIdle", false);
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


}
