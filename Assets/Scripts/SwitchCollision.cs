using UnityEngine;
using UnityEngine.UIElements;

public static class SwitchCollision
{
    // Gets all objects overlapping
    private static SwitchBehavior checkSwitches(Vector3 pos, LayerMask switchLayer)
    {
        Debug.DrawRay(
    pos,
    Vector3.down,
    Color.red,
    3f);

        Debug.Log("Checking switches from " + pos);
        RaycastHit hit;
        if (Physics.Raycast(pos, Vector3.down, out hit, 3.0f, switchLayer))
        {
            // Gets collided with switches script to update the switch is being pressed
            SwitchBehavior switchBehavior = hit.collider.GetComponent<SwitchBehavior>();
            if (switchBehavior != null)
            {
                return switchBehavior;
            }
        }

        return null;
    }

    // Used to prevent too much repeat code between objects that can activate a switch
    public static SwitchBehavior updateSwitch(Vector3 pos, ref SwitchBehavior switchBelow, LayerMask switchLayer)
    {
        if (switchBelow != null)
        {
            switchBelow.setPressed(false);
            switchBelow = null;
        }

        switchBelow = checkSwitches(pos, switchLayer);

        if (switchBelow != null)
        {
            switchBelow.setPressed(true);
        }

        return switchBelow;
    }
}
