using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public PlayerInputs playerInputs;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Enable();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
