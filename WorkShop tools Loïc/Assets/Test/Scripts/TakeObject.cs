using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakeObject : MonoBehaviour
{
    private PlayerInputs inputs;
    private InputAction action;
    [SerializeField] private Rigidbody hand;

    enum Arms
    {
        Left,
        Right
    }

    [Header("Choose Arm")] 
    [SerializeField] private Arms arm;

    private Transform currentObject;
    private Transform previousParent;

    private void Start()
    {
        inputs = InputManager.instance.playerInputs;
        if (arm == Arms.Left)
        {
            action = inputs.PlayerAction.ActiveLeftArm;
        }
        else
        {
            action = inputs.PlayerAction.ActiveRightArm;
        }
    }

    private void Update()
    {
        LetGoObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ITakeable>() != null && action.IsPressed() && currentObject == null)
        {
            other.GetComponent<ITakeable>().Take(hand);
            currentObject = other.transform;
            previousParent = other.transform.parent;
        }
    }

    void LetGoObject()
    {
        if (action.WasReleasedThisFrame())
        {
            if (currentObject != null)
            {
                currentObject.GetComponent<ITakeable>().LetGo();
                currentObject = null;
            }
        }
    }
    
    
}
