using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    private List<SwitchBehavior> switches = new List<SwitchBehavior>();
    public Boolean isOpen = false;
    public LayerMask switchLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Gets all game objects in the scene
        var gameObjects = FindObjectsByType<GameObject>();
        // Iterates through game objects to check if each objects layer mask matches switches   
        foreach (GameObject obj in gameObjects)
        {
            if (((1 << obj.layer) & switchLayer.value) != 0)
            {
                var switchBehavior = obj.GetComponent<SwitchBehavior>();
                if (switchBehavior != null && switchBehavior.door == this.gameObject)
                {
                    switches.Add(switchBehavior);
                    Debug.Log(
                    gameObject.name +
                    gameObject.name +
                    " found switch " +
                    switchBehavior.gameObject.name);
                }
            }
        }
    }

    // Called when a switch is pressed or unpressed
    public void updateDoor()
    {
        foreach(SwitchBehavior s in switches) {
            if (!s.isPressed)
            {
                setDoor(false);
                return;
            }
        }
        setDoor(true);
    }

    // Sets door to open and derenders it if it is open or does the opposite if closed again
    private void setDoor(bool open)
    {
        if (isOpen == open)
            return;

        isOpen = open;

        MeshRenderer renderer =
            GetComponent<MeshRenderer>();

        Collider collider =
            GetComponent<Collider>();

        if (renderer != null)
            renderer.enabled = !isOpen;

        if (collider != null)
            collider.enabled = !isOpen;
    }
}
