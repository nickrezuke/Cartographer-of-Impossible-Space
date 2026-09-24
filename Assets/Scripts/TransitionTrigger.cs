using System;
using UnityEngine;

public class TransitionTrigger : MonoBehaviour
{
    public Transform SpawnDestination;
    public TransitionTrigger oppositeTrigger;
    public Vector3 rotationAxis;
    public static bool TransitionLocked = false;

    [SerializeField]
    public RoomCompletion roomManager;

    private bool activeTrigger = true;
    public bool IsActive => activeTrigger;
    private bool completesRoom = false;
    public bool CompletesRoom => completesRoom;
    public void DisableTrigger()
    {
        activeTrigger = false;
    }

    public void EnableTrigger()
    {
        activeTrigger = true;
    }
    public static void EnableAllTriggers()
    {
        foreach (TransitionTrigger trigger in
                 FindObjectsByType<TransitionTrigger>(FindObjectsInactive.Exclude))
        {
            trigger.EnableTrigger();
        }
    }
}