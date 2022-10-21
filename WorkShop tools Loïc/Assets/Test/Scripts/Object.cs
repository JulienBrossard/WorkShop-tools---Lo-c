using UnityEngine;

public class Object : MonoBehaviour, ITakeable
{
    private bool isTaken;
    private FixedJoint fixedJoint;

    public void Take(Rigidbody hand)
    {
        if (!isTaken)
        {
            fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = hand;
            isTaken = true;
            gameObject.layer = 7;
        }
    }

    public void LetGo()
    {
        isTaken = false;
        if (fixedJoint != null)
        {
            fixedJoint.breakForce = 0;
        }
        gameObject.layer = 3;
    }
}
