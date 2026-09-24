using NUnit.Framework.Constraints;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BurrowTunnelBehavior : MonoBehaviour
{
    private GUIStyle textStyle;
    private Texture2D labelBackground;
    [SerializeField]
    public GameObject connectingTunnel;
    [SerializeField]
    public Vector3 rotation;
    [SerializeField]
    public float rotationDegrees;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textStyle = new GUIStyle();
        textStyle.normal.textColor = Color.white;
        textStyle.fontStyle = FontStyle.Bold;
        textStyle.alignment = TextAnchor.MiddleCenter;

        labelBackground = new Texture2D(1, 1);
        labelBackground.SetPixel(0, 0, new Color(0, 0, 0, 1.0f));
        labelBackground.Apply();
    }

    public IEnumerator rotateToConnectingTunnelAndSetPosition(GameObject player)
    {
        LabyrinthCubeOrientation instance = LabyrinthCubeOrientation.Instance;

        instance.RotateCube(rotation, rotationDegrees);
        while (instance.IsRotating)
        {
            yield return null;
        }

        player.transform.position = connectingTunnel.transform.position;
    }

    // Label must be drawn in OnGui function so this is needed
    private bool drawLabel = false;
    public void toggleLabel(bool toggle)
    {
        if (drawLabel == toggle) return;
        drawLabel = toggle;
    }

    // Renders a label above the burrow displaying that it can be entered.
    void OnGUI()
    {
        if (!drawLabel) return;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(
            transform.position + Vector3.up * 2f
         );

        screenPos.y = Screen.height - screenPos.y;

        Rect rect = new Rect(screenPos.x - 75, screenPos.y - 40, 150, 20);

        GUI.DrawTexture(rect, labelBackground);
        GUI.Label(
            rect,
            "Press 'E' to enter burrow!"
        );
    }
}
