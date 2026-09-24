using UnityEngine;
using UnityEngine.InputSystem;

public class BoxGridMovement : MonoBehaviour
{
    //This script provides BASIC grid movement functionality. There is a high probability this will need to be adjusted later to accomodate the rotating labyrinth cube orientation. 
    
    [SerializeField] private float moveSpeed = 4f;
    private Vector3 targetLocalPosition;
    private bool isMoving = false;
    private Transform cubeRef;
    private SwitchBehavior switchBelow = null;

    public LayerMask switchLayer;

    void Start()
    {
        cubeRef = transform.parent;
        targetLocalPosition = transform.localPosition;
    }
    void Update()
    {
         {
            if (LabyrinthCubeOrientation.Instance != null &&
                LabyrinthCubeOrientation.Instance.IsRotating)
            {
                return;
            }
        } 
        //Allows for a smooth player transition from one tile to the next. 
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetLocalPosition, moveSpeed * Time.deltaTime);
       
        //Checks if the player finished moving one tile space. 
        if (isMoving && Vector3.Distance(transform.localPosition, targetLocalPosition) < 0.01f)
        {
            transform.localPosition = targetLocalPosition; // Force snap to eliminate decimal drift
            isMoving = false;

            SwitchCollision.updateSwitch(
            transform.position,
            ref switchBelow,
            switchLayer);
        }
    }

    public void move(Vector3 worldDirection)
    {
        if (isMoving)
            return;

        Vector3 localDirection =
            transform.parent.InverseTransformDirection(worldDirection);

        localDirection = new Vector3(
            Mathf.Round(localDirection.x),
            Mathf.Round(localDirection.y),
            Mathf.Round(localDirection.z)
        );
        Debug.Log(
           "Local push direction: " +
            localDirection);
        targetLocalPosition += localDirection;

        isMoving = true;

        Debug.Log(
    gameObject.name +
    " Local Direction: " +
    localDirection
);
    }
    public bool getIsMoving()
    {
        return isMoving;
    }
    public void SnapToLocalPosition(Vector3 localPosition)
    {
        transform.localPosition = localPosition;
        targetLocalPosition = localPosition;
        isMoving = false;
    }

}