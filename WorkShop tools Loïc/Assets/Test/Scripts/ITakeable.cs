using UnityEngine;

public interface ITakeable
{
    public void Take(Rigidbody hand);
    public void LetGo();
}
