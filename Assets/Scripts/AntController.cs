using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class AntController : MonoBehaviour
{
    //private float yRotation = 0f;
    private float xRotation;
    private float heading = 90; 

    [SerializeField]
    private Vector3 modelOffset;
    public void FaceDirection(Vector3 direction)
    {
        if (direction == Vector3.forward)
            heading = 90;
        else if (direction == Vector3.right)
            heading = 180;
        else if (direction == Vector3.back)
            heading = 270;
        else if (direction == Vector3.left)
            heading = 0;
    }
    void LateUpdate()
    {
        // Keep ant upright while preserving its heading
        transform.localRotation =
     Quaternion.Euler(
         modelOffset.x,
         modelOffset.y + heading,
         modelOffset.z);
    }
}
