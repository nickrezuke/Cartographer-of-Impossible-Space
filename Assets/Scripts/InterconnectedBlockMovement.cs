using UnityEngine;
using static UnityEngine.Tilemaps.Tile;

public class InterconnectedBlockMovement : MonoBehaviour
{
    //This is a modified copy of the original BoxMovement script. This script 
    //is specifically for the 3D box movements, which determine if the player collides with
    //this object in any given area instead determining this only by its center. 

    [SerializeField] private float moveSpeed = 2f;
    private float originalPlayerSpeed;

    private Vector3 targetLocalPosition;
    private bool isMoving = false;
    private BoxCollider boxCollider;
    public PlayerGridMovement playerMovement;

    [SerializeField] private Transform colliderTop;
    [SerializeField] private Transform colliderBottom;

    void Start()
    {
        targetLocalPosition = transform.localPosition;
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (LabyrinthCubeOrientation.Instance != null &&
            LabyrinthCubeOrientation.Instance.IsRotating)
        {
            return;
        }
        transform.localPosition =
            Vector3.MoveTowards(
                transform.localPosition,
                targetLocalPosition,
                moveSpeed * Time.deltaTime);

        if (isMoving && Vector3.Distance(
                transform.localPosition,
                targetLocalPosition) < 0.01f)
        {
            transform.localPosition = targetLocalPosition;
            isMoving = false;
            CameraShake.Instance.toggleShake(false);
            if (playerMovement == null)
            {
                Debug.Log("Player movement is null could not restore speed");
            }
            playerMovement.moveSpeed = originalPlayerSpeed;
        }
        //transform.localPosition =
        //    Vector3.MoveTowards(
        //        transform.localPosition,
        //        targetLocalPosition,
        //        moveSpeed * Time.deltaTime);

        //if (Vector3.Distance(
        //        transform.localPosition,
        //        targetLocalPosition) < 0.01f)
        //{
        //    transform.localPosition = targetLocalPosition;
        //    isMoving = false;
        //}
    }

    public bool getIsMoving()
    {
        return isMoving;
    }

    public bool CanMove(
        Vector3 worldDirection,
        LayerMask wallLayer,
        LayerMask boxLayer)
    {
        // TOP checks
        bool hitTopWall = Physics.Raycast(
            colliderTop.position,
            worldDirection,
            1f,
            wallLayer);

        bool hitTopBox = Physics.Raycast(
            colliderTop.position,
            worldDirection,
            1f,
            boxLayer);

        // BOTTOM checks
        bool hitBottomWall = Physics.Raycast(
            colliderBottom.position,
            worldDirection,
            1f,
            wallLayer);

        bool hitBottomBox = Physics.Raycast(
            colliderBottom.position,
            worldDirection,
            1f,
            boxLayer);

        // Debug output
        Debug.Log(
            "Top Wall=" + hitTopWall +
            " Top Box=" + hitTopBox +
            " Bottom Wall=" + hitBottomWall +
            " Bottom Box=" + hitBottomBox);

        // Stop movement if ANY end hits something
        if (hitTopWall ||
            hitTopBox ||
            hitBottomWall ||
            hitBottomBox)
        {
            return false;
        }

        return true;
    }
    public void move(Vector3 worldDirection)
    {
        if (isMoving)
            return;

        Vector3 localDirection =
            transform.parent.InverseTransformDirection(
                worldDirection);

        localDirection = new Vector3(
            Mathf.Round(localDirection.x),
            Mathf.Round(localDirection.y),
            Mathf.Round(localDirection.z));

        targetLocalPosition += localDirection;
        if (this.playerMovement == null)
        {
            this.playerMovement = playerMovement;
        }
        originalPlayerSpeed = playerMovement.moveSpeed;
        playerMovement.moveSpeed = this.moveSpeed;
        CameraShake.Instance.toggleShake(true);
        isMoving = true;
    }
    public void SnapToLocalPosition(Vector3 localPosition)
    {
        transform.localPosition = localPosition;
        targetLocalPosition = localPosition;
        isMoving = false;
    }
}