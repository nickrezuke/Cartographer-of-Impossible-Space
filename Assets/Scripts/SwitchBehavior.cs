using UnityEngine;

public class SwitchBehavior : MonoBehaviour
{
    [SerializeField]
    public GameObject door;
    public bool isPressed = false;
    public bool IsPressed => isPressed;
    public void setPressed(bool pressed)
    {
        if (isPressed == pressed) return;

        isPressed = pressed;

        DoorBehavior doorBehavior = door.GetComponent<DoorBehavior>();
        if (doorBehavior != null)
        {
            doorBehavior.updateDoor();
        }
        else
        {
            Debug.Log("Door missing DoorBehavior script. This shouldn't happen.");
        }
    }
}
