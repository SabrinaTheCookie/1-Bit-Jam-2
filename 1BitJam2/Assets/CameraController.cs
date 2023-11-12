using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSteps = 90f;

    public float rotationDuration;
    public AnimationCurve rotationCurve;
    public bool isRotating;


    [ContextMenu("RotateCamera")]
    public void RotateCamera() { RotateCamera(false);}
    public void RotateCamera(bool antiClockwise)
    {
        if (isRotating) return;
        StartCoroutine(RotateLerp(antiClockwise));
    }

    public IEnumerator RotateLerp(bool antiClockwise)
    {
        isRotating = true;
        float t = 0;
        float startRotation = transform.rotation.eulerAngles.y;
        float endRotation = startRotation + rotationSteps * (antiClockwise ? -1 : 1);
        while (t < rotationDuration)
        {
            transform.rotation = Quaternion.Euler(0, Mathf.Lerp(startRotation, endRotation, rotationCurve.Evaluate(t / rotationDuration)), 0);
            t += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, endRotation, 0);
        isRotating = false;
        yield return null;
    }

    

}
