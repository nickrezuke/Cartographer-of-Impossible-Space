using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

public class PlayerGridMovement : MonoBehaviour
{
    //This script provides BASIC grid movement functionality. There is a high probability this will need to be adjusted later to accomodate the rotating labyrinth cube orientation. 
    [SerializeField] public float moveSpeed = 4f;
    private Vector3 targetPosition;
    private bool playerMoving = false;
    private bool transitioning = false;
    public LayerMask wallLayer;
    public LayerMask boxLayer;
    public LayerMask doorLayer;
    public LayerMask switchLayer;
    public LayerMask burrowLayer; //NEW 6/23
    public LayerMask spikeLayer; //NEW 6/25
    private SwitchBehavior switchBelow = null;
  
    [SerializeField] private AntController antVisual;
    private BurrowTunnelBehavior burrowBelow = null;
    private Collider playerCollider; //NEW 6/23
    void Start()
    {
        targetPosition = transform.position;
        playerCollider = GetComponent<Collider>(); //NEW 6/23
    }
    void Update()
    {
        if (!playerCollider.enabled) return;

        if (Keyboard.current.zKey.wasPressedThisFrame &&
           !playerMoving &&
           !TransitionTrigger.TransitionLocked &&
           (LabyrinthCubeOrientation.Instance == null ||
            !LabyrinthCubeOrientation.Instance.IsRotating))
        {
            UndoManager.Instance.Undo(this);
        }

        //Allows for a smooth player transition from one tile to the next. 
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        //Checks if the player finished moving one tile space. 
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition; // Force snap to eliminate decimal drift
            playerMoving = false;

            SwitchCollision.updateSwitch(
            transform.position,
            ref switchBelow,
            switchLayer);

            checkForBurrow();
        }
        if (burrowBelow != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            UndoManager.Instance.SaveState(this);
            StartCoroutine(enterBurrow(burrowBelow));
            burrowBelow.toggleLabel(false);
            burrowBelow = null;
        }
        //Player moves only when stationary/no user input. 
        if (!playerMoving && Keyboard.current != null)
        {
            Vector3 inputDirection = Vector3.zero;

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) inputDirection = Vector3.forward;
            else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) inputDirection = Vector3.back;
            else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) inputDirection = Vector3.left;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) inputDirection = Vector3.right;

            if (inputDirection != Vector3.zero)
            {
                //Uses raycast to determine distance between center points of the player and the next tile. 
                // Uses 1.0f distance (equivalent to 1 Unity Grid Unit)

                // Gets the hit object as a raycast to see if it's a box or if a wall, otherwise moves
                RaycastHit hit;
                if (Physics.Raycast(transform.position, inputDirection, out hit, 1.0f, boxLayer))
                {
                    GameObject box = hit.collider.gameObject;
                    // check if new potentional box position is a wall or another box (moves with ant if neither)
                    bool hitWall = Physics.Raycast(box.transform.position, inputDirection, 1.0f, wallLayer);
                    bool hitBox = Physics.Raycast(box.transform.position, inputDirection, 1.0f, boxLayer);
                    bool hitDoor =
                            !canMoveThroughDoor(
                                box.transform.position,
                                inputDirection);

                    if (!hitWall && !hitBox && !hitDoor)
                    {
                        InterconnectedBlockMovement specialBlock =box.GetComponent<InterconnectedBlockMovement>();
    

                        if (specialBlock != null)
                        {
                            if (specialBlock.getIsMoving())
                                return;

                            if (!specialBlock.CanMove(
                                    inputDirection,
                                    wallLayer,
                                    boxLayer))
                            {
                                return;
                            }
                            UndoManager.Instance.SaveState(this);

                            specialBlock.move(inputDirection);
                            antVisual.FaceDirection(inputDirection);
                            setMoving(inputDirection);
                            return;
                        }

                        BoxGridMovement boxMovement =
                            box.GetComponent<BoxGridMovement>();

                        if (boxMovement == null)
                            return;

                        if (boxMovement.getIsMoving())
                            return;

                        UndoManager.Instance.SaveState(this);

                        boxMovement.move(inputDirection);
                        antVisual.FaceDirection(inputDirection);
                        setMoving(inputDirection);
                    } 
                    
                }
                else if (
                        !Physics.Raycast(
                            transform.position,
                            inputDirection,
                            1.0f,
                            wallLayer)
                            &&
                            !SpikeIsBlocking(inputDirection)
                            &&
                            canMoveThroughDoor(
                                transform.position,
                                inputDirection))
                {
                    UndoManager.Instance.SaveState(this);
                    antVisual.FaceDirection(inputDirection);
                    setMoving(inputDirection);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (TransitionTrigger.TransitionLocked)
            return;

        TransitionTrigger trigger =
            other.GetComponent<TransitionTrigger>();

        if (trigger == null)
            return;

        if (!trigger.IsActive)
            return;

        TransitionTrigger.TransitionLocked = true;
        StartCoroutine(
            Transition(trigger));
    }
    private IEnumerator Transition(
    TransitionTrigger trigger)
    {
        trigger.DisableTrigger();

        LabyrinthCubeOrientation.Instance.RotateCube(
            trigger.rotationAxis);

        while (LabyrinthCubeOrientation.Instance.IsRotating)
        {
            yield return null;
        }

        transform.position =
            trigger.SpawnDestination.position;

        targetPosition =
            transform.position;

        if (trigger.oppositeTrigger != null)
            trigger.oppositeTrigger.EnableTrigger();

        // Prevent immediate retriggering
        yield return new WaitForSeconds(0.25f);

        TransitionTrigger.TransitionLocked = false;
    }
    bool canMoveThroughDoor(Vector3 pos, Vector3 inputDirection)
    {
        RaycastHit hit;

        if (Physics.Raycast(
                pos,
                inputDirection,
                out hit,
                1.0f,
                doorLayer))
        {
            DoorBehavior door =
                hit.collider.GetComponent<DoorBehavior>();

            if (door != null)
            {
                return door.isOpen;
            }
        }

        return true;
    }
    void setMoving(Vector3 inputDirection)
    {
        targetPosition = transform.position + inputDirection;
        playerMoving = true;
    }
    void checkForBurrow()
    {
        Collider[] hits = Physics.OverlapBox(
                transform.position,
                new Vector3(0.45f, 1.0f, 0.45f),
                Quaternion.identity,
                burrowLayer);
        if (hits.Length > 0)
        {
            burrowBelow = hits[0].GetComponent<BurrowTunnelBehavior>();
            burrowBelow.toggleLabel(true);
        }
        else
        {
            if (burrowBelow != null)
            {
                burrowBelow.toggleLabel(false);
            }
            burrowBelow = null;
        }
    }

    IEnumerator enterBurrow(BurrowTunnelBehavior burrow)
    {
        playerCollider.enabled = false;
        yield return burrow.rotateToConnectingTunnelAndSetPosition(gameObject);
        TransitionTrigger.EnableAllTriggers();

        targetPosition = transform.position;
        playerCollider.enabled = true;
    }

    bool SpikeIsBlocking(Vector3 inputDirection)
    {
        RaycastHit hit;

        if (Physics.Raycast(
            transform.position,
            inputDirection,
            out hit,
            1.0f,
            spikeLayer))
        {
            SpikeTrap spike = hit.collider.GetComponent<SpikeTrap>();

            if (spike != null)
            {
                Debug.Log(
                    "Hit: " +
                    hit.collider.gameObject.name +
                    " | Instance: " +
                    hit.collider.GetInstanceID() +
                    " | IsUp: " +
                    spike.IsUp());

                return spike.IsUp();
            }
        }

        return false;
    }

    public void SnapToPosition(Vector3 position)
    {
        transform.position = position;
        targetPosition = position;
        playerMoving = false;
    }
}