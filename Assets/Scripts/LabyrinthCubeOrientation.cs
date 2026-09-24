using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LabyrinthCubeOrientation : MonoBehaviour
{
    public static LabyrinthCubeOrientation Instance;
    [SerializeField] private float rotateSpeed = 180f;
    private bool rotating;
    public bool IsRotating => rotating;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
    //    if (Keyboard.current.rKey.wasPressedThisFrame)
    //        RotateCube(Vector3.right);

    //    if (Keyboard.current.tKey.wasPressedThisFrame)
    //        RotateCube(Vector3.left);

    //    if (Keyboard.current.fKey.wasPressedThisFrame)
    //        RotateCube(Vector3.forward);

    //    if (Keyboard.current.gKey.wasPressedThisFrame)
    //        RotateCube(Vector3.back);
    }
    public void RotateCube(Vector3 axis, float degrees = 90f)
    {
        if (!rotating)
        {
            StartCoroutine(RotateRoutine(axis, degrees));
        }

    }
    private IEnumerator RotateRoutine(Vector3 axis, float degrees)
    {
        rotating = true;
        Quaternion startRot = transform.rotation;
        Quaternion targetRot =
            Quaternion.AngleAxis(degrees, axis) * startRot;
       
        float angle = 0;
        while (angle < degrees)
        {
            float step = rotateSpeed * Time.deltaTime;
            transform.rotation =
                Quaternion.RotateTowards(
                    transform.rotation,
                    targetRot,
                    step);
            angle += step;
            yield return null;
        }

        transform.rotation = targetRot;

        rotating = false;
       
    }

}