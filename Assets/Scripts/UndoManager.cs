using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoManager : MonoBehaviour
{
    //Allows other scripts to easily access the UndoManager.
    public static UndoManager Instance;

    //Stores all previous game states.
    private Stack<GameState> undoStack = new Stack<GameState>();

    private void Awake()
    {
        //Creates a singleton instance.
        Instance = this;
    }

    //Saves the current player, box, and interconnected block positions before movement occurs.
    public void SaveState(PlayerGridMovement player)
    {
        GameState state = new GameState();

        //Stores current player world position.
        state.playerPosition = player.transform.position;

        BoxGridMovement[] boxes =
            FindObjectsByType<BoxGridMovement>(
                FindObjectsInactive.Exclude);

        //Stores references and local positions for normal boxes.
        state.boxes = boxes;
        state.boxLocalPositions = new Vector3[boxes.Length];

        for (int i = 0; i < boxes.Length; i++)
        {
            state.boxLocalPositions[i] = boxes[i].transform.localPosition;
        }

        InterconnectedBlockMovement[] interconnectedBlocks =
            FindObjectsByType<InterconnectedBlockMovement>(
                FindObjectsInactive.Exclude);

        //Stores references and local positions for interconnected blocks.
        state.interconnectedBlocks = interconnectedBlocks;
        state.interconnectedBlockLocalPositions = new Vector3[interconnectedBlocks.Length];

        for (int i = 0; i < interconnectedBlocks.Length; i++)
        {
            state.interconnectedBlockLocalPositions[i] =
                interconnectedBlocks[i].transform.localPosition;
        }

        //Stores the current cube rotation.
        if (LabyrinthCubeOrientation.Instance != null)
        {
            state.cubeRotation = LabyrinthCubeOrientation.Instance.transform.rotation;
        }

        //Pushes the state onto the stack.
        undoStack.Push(state);
    }

    //Restores the previous saved game state.
    public void Undo(PlayerGridMovement player)
    {
        //Prevents errors if there are no previous moves.
        if (undoStack.Count == 0)
            return;

        //Do not undo while the cube is already rotating.
        if (LabyrinthCubeOrientation.Instance != null &&
            LabyrinthCubeOrientation.Instance.IsRotating)
            return;

        //Gets the most recent saved state.
        GameState state = undoStack.Pop();

        //Prevents transition triggers from firing during undo.
        TransitionTrigger.TransitionLocked = true;

        //Restore cube rotation FIRST so positions line up correctly.
        if (LabyrinthCubeOrientation.Instance != null)
        {
            LabyrinthCubeOrientation.Instance.transform.rotation = state.cubeRotation;
        }

        //Moves the player back and resets movement data.
        player.SnapToPosition(state.playerPosition);

        //Restores each normal box local position and resets movement data.
        for (int i = 0; i < state.boxes.Length; i++)
        {
            if (state.boxes[i] != null)
            {
                state.boxes[i].SnapToLocalPosition(state.boxLocalPositions[i]);
            }
        }

        //Restores each interconnected block local position and resets movement data.
        for (int i = 0; i < state.interconnectedBlocks.Length; i++)
        {
            if (state.interconnectedBlocks[i] != null)
            {
                state.interconnectedBlocks[i].SnapToLocalPosition(
                    state.interconnectedBlockLocalPositions[i]);
            }
        }

        StartCoroutine(UnlockTransitionAfterUndo());
    }

    //Unlocks transitions shortly after an undo completes.
    private IEnumerator UnlockTransitionAfterUndo()
    {
        yield return new WaitForSeconds(0.25f);

        TransitionTrigger.TransitionLocked = false;
        TransitionTrigger.EnableAllTriggers();
    }
}

//Stores information required to restore a previous move.
public class GameState
{
    //Previous player world position.
    public Vector3 playerPosition;

    //References to all normal movable boxes.
    public BoxGridMovement[] boxes;

    //Previous local positions of all normal movable boxes.
    public Vector3[] boxLocalPositions;

    //References to all interconnected blocks.
    public InterconnectedBlockMovement[] interconnectedBlocks;

    //Previous local positions of all interconnected blocks.
    public Vector3[] interconnectedBlockLocalPositions;

    //Previous cube rotation.
    public Quaternion cubeRotation;
}