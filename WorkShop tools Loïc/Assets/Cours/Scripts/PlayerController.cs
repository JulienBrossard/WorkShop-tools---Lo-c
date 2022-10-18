using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    PlayerInputs inputs;
    private Vector2 move;
    [Header("Rigidbody")]
    [SerializeField] Rigidbody rb;
    [Header("Player Data")]
    [SerializeField] PlayerData playerData;
    [Header("Speed")]
    [SerializeField] private Vector3 targetSpeed;
    [SerializeField] private Vector3 currentSpeed;
    private float elapsedTime = 0;
    private Quaternion targetRotation;
    private RaycastHit hit;
    [Header("Jump")]
    [SerializeField] private LayerMask layer;
    
    private void Awake()
    {
        inputs = new PlayerInputs();
        inputs.Enable();
    }

    void Update()
    {
        move.x = inputs.PlayerMovement.Move.ReadValue<Vector2>().normalized.x;
        move.y = inputs.PlayerMovement.Move.ReadValue<Vector2>().normalized.y;
        HorizontalMovement();
        Rotate();
        Jump();
    }

    void HorizontalMovement()
    {
        if (inputs.PlayerMovement.Move.ReadValue<Vector2>() != Vector2.zero)
        {
            targetSpeed = transform.forward * move.y * playerData.horizontalSpeed * Time.deltaTime;
            currentSpeed = rb.velocity;
            targetSpeed.y = currentSpeed.y;
            elapsedTime += Time.deltaTime;
            rb.velocity = Vector3.Lerp(currentSpeed, targetSpeed, playerData.acceleration.Evaluate(elapsedTime));
        }
        else
        {
            elapsedTime = 0;
        }
    }

    void Rotate()
    {
        if (inputs.PlayerMovement.Move.ReadValue<Vector2>().x != 0 )
        {
            targetRotation = transform.rotation * Quaternion.Euler(Vector3.up * Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg);
        
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, playerData.angularSpeed * Time.deltaTime);
        }
    }

    void Jump()
    {
        if (inputs.PlayerMovement.Jump.WasPressedThisFrame() && Physics.Raycast(transform.position, Vector3.down, out hit, 2f, layer))
        {
            rb.AddForce(rb.velocity + new Vector3(0, playerData.jumpForce, 0), ForceMode.Impulse);
        }
    }
}
