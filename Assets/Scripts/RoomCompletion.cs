using UnityEngine;

public class RoomCompletion : MonoBehaviour
{
    [SerializeField]
    private Torches[] torches;

    [SerializeField]
    private SwitchBehavior[] requiredSwitches;

    private bool completed = false;

    private void Update()
    {
        if (completed)
            return;

        if (AllRequiredSwitchesPressed())
        {
            CompleteRoom();
        }
    }

    private bool AllRequiredSwitchesPressed()
    {
        foreach (SwitchBehavior s in requiredSwitches)
        {
            if (s == null)
                continue;

            if (!s.IsPressed)
                return false;
        }

        return true;
    }

    public void CompleteRoom()
    {
        if (completed)
            return;

        completed = true;

        foreach (Torches torch in torches)
        {
            torch.Ignite();
        }
    }
}