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

    public int traversalInputDirection;
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
        InputManager.OnTraversePressed += StartTraversal;
        InputManager.OnTraverseReleased += EndTraversal;
    }

    void OnDisable()
    {
        InputManager.OnTraversePressed -= StartTraversal;
        InputManager.OnTraverseReleased -= EndTraversal;
    }

    void Start()
    {
        UpdateFloorScale(_manager.Floors);
    }

    void Update()
    {
        //Only update timer if holding key
        if (traversalInputDirection == 0) return;
        timeHoldingTraversal += Time.deltaTime;
        //If its already fast forwarding, return.
        if (!fastForwardActive & timeHoldingTraversal > holdTimeForFastTraversal)
        {
            fastForwardActive = true;
        }
    }

    void StartTraversal(int input)
    {
        if(input == 0) return;

        Debug.Log("Start");

        traversalInputDirection = input;
        if (input > 0) TraverseUpwards();
        else TraverseDownwards();
    }

    void EndTraversal()
    {
        Debug.Log("End");
        traversalInputDirection = 0;
        timeHoldingTraversal = 0;
        fastForwardActive = false;
    }


    [ContextMenu("Traverse Upwards")]
    public void TraverseUpwards()
    {
        //Cannot traverse upwards on the top floor.
        if (currentFloor == 0) return;
        Traverse(-1);

    }
    [ContextMenu("Traverse Downwards")]
    public void TraverseDownwards()
    {
        //Cannot traverse downwards on bottom floor.
        if (currentFloor == _manager.Floors.Count - 1) return;
        Traverse(1);
    }

    private void Traverse(int direction)
    {
        if (isTraversing) return;
        isTraversing = true;
        OnTraversalStarted?.Invoke();
        int nextFloor = currentFloor + Mathf.RoundToInt(direction);
        List<Floor> floors = _manager.Floors;
        Debug.Log("Traverse called");
        StartCoroutine(TraverseOverTime(floors, direction, nextFloor));
    }

    private void TraversalComplete()
    {
        if (!isTraversing) return;
        isTraversing = false;
        OnTraversalEnded?.Invoke();

        if (traversalInputDirection != 0)
        {
            StartTraversal(traversalInputDirection);
        }
    }
    private IEnumerator TraverseOverTime(List<Floor> targets, int traversal, int nextFloor)
    {
        Debug.Log("Traverse ie");
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
            endPos.y += (translationDistance * traversal);
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
        currentFloor = nextFloor;
        Debug.Log("Traversal Complete");
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
