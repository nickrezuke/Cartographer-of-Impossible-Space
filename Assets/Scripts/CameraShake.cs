using System;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 originalPosition;
    private Vector3 shakeOffset;
    private bool isShaking = false;
    private Camera cam;

    [SerializeField]
    private float strength = 0.05f;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        cam = Camera.main;
    }

    public void toggleShake(bool toggle)
    {
        isShaking = toggle;
        if (isShaking)
        {
            originalPosition = cam.transform.localPosition;
        } else
        {
            cam.transform.localPosition = originalPosition;
        }
    }


    void Update()
    {
        if (!isShaking) return;

        cam.transform.localPosition -= shakeOffset;

        Vector2 offset = UnityEngine.Random.insideUnitCircle * strength;
        shakeOffset = new Vector3(offset.x, offset.y, 0f);
        cam.transform.localPosition += shakeOffset;
    }
}
