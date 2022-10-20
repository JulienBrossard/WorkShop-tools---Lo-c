using System;
using UnityEngine;

public class Ball : MonoBehaviour, ITakeable
{
    private bool isTaken;
    private Transform parent;
    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        parent = transform.parent;
    }

    public void Take(Transform hand)
    {
        if (!isTaken)
        {
            transform.SetParent(hand);
            isTaken = true;
            rb.useGravity = false;
            rb.isKinematic = true;
            gameObject.layer = 7;
        }
    }

    public void LetGo()
    {
        isTaken = false;
        transform.parent = parent;
        rb.useGravity = true;
        rb.isKinematic = false;
        gameObject.layer = 0;
    }
}
