using System;
using UnityEngine;

public class ActivateRagdoll : MonoBehaviour
{
    private PlayerInputs inputs;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private CapsuleCollider collider;
    private bool isRagdoll;

    private void Start()
    {
        inputs = InputManager.instance.playerInputs;
    }

    private void Update()
    {
        Enable();
    }

    void Enable()
    {
        if (inputs.PlayerAction.Ragdoll.WasPressedThisFrame() && !isRagdoll)
        {
            animator.enabled = false;
            rb.isKinematic = true;
            collider.enabled = false;
            isRagdoll = true;
        }
        else if(inputs.PlayerAction.Ragdoll.WasPressedThisFrame())
        {
            animator.enabled = false;
            rb.isKinematic = true;
            collider.enabled = false;
            isRagdoll = false;
        }
    }

}
