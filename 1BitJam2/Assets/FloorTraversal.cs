using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTraversal : MonoBehaviour
{
    [SerializeField] private FloorManager _manager;
    public int currentFloor;
    public float traversalDuration;
    public AnimationCurve translateCurve;
    public Vector3 focusedFloorScale;
    public Vector3 unfocusedFloorScale;
    
    public bool isTraversing;
    public static event Action OnTraversalStarted;
    public static event Action OnTraversalEnded;

    private void Awake()
    {
        _manager = GetComponent<FloorManager>();
    }

    void Start()
    {
        UpdateFloorScale(_manager.Floors);
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
        List<GameObject> floors = _manager.Floors;

        for (int i = 0; i < floors.Count; i++)
        {
            float translationDistance = _manager.YSpaceBetweenFloors * (i == nextFloor || i == currentFloor ? 1.5f : 1);
            StartCoroutine(TraverseOverTime(floors[i].transform, direction * translationDistance));
            StartCoroutine(UpdateFloorScaleOverTime(floors[i].transform, (i != nextFloor) ? unfocusedFloorScale : focusedFloorScale));
        }
        Invoke(nameof(TraversalComplete), traversalDuration);
        currentFloor = nextFloor;
    }

    private void TraversalComplete()
    {
        if (!isTraversing) return;
        OnTraversalEnded?.Invoke();
        isTraversing = false;
    }
    private IEnumerator TraverseOverTime(Transform target, Vector3 traversal)
    {
        float t = 0;
        Vector3 startPos = target.position;
        Vector3 endPos = startPos + traversal;
        while (t < traversalDuration)
        {
            target.position = Vector3.Lerp(startPos, endPos ,translateCurve.Evaluate(t / traversalDuration) );
            t += Time.deltaTime;
            yield return null;
        }

        target.position = endPos;
        yield return null;
    }

    void UpdateFloorScale(List<GameObject> gameObjects)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].transform.localScale = (i != currentFloor) ? unfocusedFloorScale : focusedFloorScale;
        }
    }
    IEnumerator UpdateFloorScaleOverTime(Transform target, Vector3 endScale)
    {
        float t = 0;
        Vector3 startScale = target.localScale;
        while (t < traversalDuration)
        {
            target.localScale = Vector3.Lerp(startScale, endScale , translateCurve.Evaluate(t / traversalDuration));
            t += Time.deltaTime;
            yield return null;
        }

        target.localScale = endScale;
        yield return null;

    }
}
