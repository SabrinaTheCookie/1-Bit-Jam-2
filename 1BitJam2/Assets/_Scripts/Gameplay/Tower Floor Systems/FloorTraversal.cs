using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTraversal : MonoBehaviour
{
    private FloorManager _manager;
    public int currentFloor;
    private float _traversalDuration;
    public float traversalDuration;

    public AnimationCurve translateCurve;
    public Vector3 focusedFloorScale;
    public Vector3 unfocusedFloorScale;

    public Vector2 traversalInputDirection;
    public float timeHoldingTraversal;
    public float holdTimeForFastTraversal;
    public float fastTraversalSpeedMultiplier;
    public bool fastForwardActive;
    public bool isTraversing;
    public static event Action OnTraversalStarted;
    public static event Action OnTraversalEnded;

    private void Awake()
    {
        _manager = GetComponent<FloorManager>();
        _traversalDuration = traversalDuration;

    }

    void OnEnable()
    {
        InputManager.OnMovePressed += StartTraversal;
        InputManager.OnMoveReleased += EndTraversal;
    }

    void OnDisable()
    {
        InputManager.OnMovePressed -= StartTraversal;
        InputManager.OnMoveReleased -= EndTraversal;

    }

    void Start()
    {
        UpdateFloorScale(_manager.Floors);
    }

    void Update()
    {
        //Only update timer if holding key
        if (traversalInputDirection == Vector2.zero) return;
        timeHoldingTraversal += Time.deltaTime;
        //If its already fast forwarding, return.
        if (fastForwardActive) return;

        if (timeHoldingTraversal > holdTimeForFastTraversal)
        {
            fastForwardActive = true;
        }
    }

    void StartTraversal(Vector2 input)
    {
        traversalInputDirection = input;
        if (input.y > 0) TraverseUpwards();
        else if(input.y < 0) TraverseDownwards();
    }

    void EndTraversal()
    {
        traversalInputDirection = Vector2.zero;
        timeHoldingTraversal = 0;
        _traversalDuration = traversalDuration;
        fastForwardActive = false;
    }


    [ContextMenu("Traverse Upwards")]
    public void TraverseUpwards()
    {
        //Cannot traverse upwards on the top floor.
        if (currentFloor == 0) return;
        Traverse(Vector3.down);

    }
    [ContextMenu("Traverse Downwards")]
    public void TraverseDownwards()
    {
        //Cannot traverse downwards on bottom floor.
        if (currentFloor == _manager.Floors.Count - 1) return;
        Traverse(Vector3.up);
    }

    private void Traverse(Vector3 direction)
    {
        if (isTraversing) return;
        OnTraversalStarted?.Invoke();
        isTraversing = true;
        int nextFloor = currentFloor + Mathf.RoundToInt(direction.y);
        List<Floor> floors = _manager.Floors;

        StartCoroutine(TraverseOverTime(floors, direction, nextFloor));

        currentFloor = nextFloor;
    }

    private void TraversalComplete()
    {
        if (!isTraversing) return;
        isTraversing = false;
        OnTraversalEnded?.Invoke();

        if (traversalInputDirection != Vector2.zero)
        {
            StartTraversal(traversalInputDirection);
        }
    }
    private IEnumerator TraverseOverTime(List<Floor> targets, Vector3 traversal, int nextFloor)
    {
        //Generate start and end scale/positions
        List<Vector3> startScales = new List<Vector3>();
        List<Vector3> startPositions = new List<Vector3>();
        List<Vector3> endScales = new List<Vector3>();
        List<Vector3> endPositions = new List<Vector3>();
        for (int i = 0; i < targets.Count; i++)
        {
            bool isNextFloor = nextFloor == i;
            float translationDistance = _manager.YSpaceBetweenFloors * (isNextFloor || currentFloor == i ? 1.5f : 1);

            //Start scale/position
            startScales.Add(targets[i].transform.localScale);
            startPositions.Add(targets[i].transform.position);
            //End Scale
            endScales.Add(isNextFloor ? focusedFloorScale : unfocusedFloorScale);
            //Calculate end position
            Vector3 endPos = startPositions[i];
            endPos.y += (translationDistance * traversal.y);
            endPositions.Add(Vector3Int.FloorToInt(endPos));
        }

        //Iterate and translate/scale all floors
        float t = 0;
        while (t < _traversalDuration)
        {
            float curveT = translateCurve.Evaluate(t / _traversalDuration);
            for (int i = 0; i < targets.Count; i++)
            {
                targets[i].transform.localScale = Vector3.Lerp(startScales[i], endScales[i], curveT);
                targets[i].transform.position = Vector3.Lerp(startPositions[i], endPositions[i], curveT);
            }
            t += Time.deltaTime * (fastForwardActive ? fastTraversalSpeedMultiplier : 1);

            yield return null;
        }

        //Set to end value
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].transform.localScale = endScales[i];
            targets[i].transform.position = endPositions[i];
        }
        TraversalComplete();

        yield return null;
    }

    void UpdateFloorScale(List<Floor> floors)
    {
        for (int i = 0; i < floors.Count; i++)
        {
            floors[i].transform.localScale = (i != currentFloor) ? unfocusedFloorScale : focusedFloorScale;
        }
    }
}
