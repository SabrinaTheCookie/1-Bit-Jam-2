using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSteps = 90f;

    public float rotationDuration;
    public AnimationCurve rotationCurve;

    [Header("Translation")]
    public float translationSteps = 10;
    public float translationDuration;
    public AnimationCurve translationCurve;
    public Vector2 heightRange;

    [ContextMenu("RotateCamera")]
    public void RotateCamera() { RotateCamera(false);}
    public void RotateCamera(bool antiClockwise)
    {
        StartCoroutine(RotateLerp(antiClockwise));
    }

    public IEnumerator RotateLerp(bool antiClockwise)
    {
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
        yield return null;
    }

    [ContextMenu("TranslateCamera")]
    public void TranslateCamera() { TranslateCamera(false); }

    public void TranslateCamera(bool moveUpwards)
    {
        StartCoroutine(TranslateLerp(moveUpwards));

    }

    public IEnumerator TranslateLerp(bool moveUpwards)
    {
        float t = 0;
        float startPosition = transform.position.y;
        float endPosition = startPosition + translationSteps * (moveUpwards ? -1 : 1);
        while (t < translationDuration)
        {
            transform.position = new Vector3(0, Mathf.Lerp(startPosition, endPosition, translationCurve.Evaluate(t / translationDuration)), 0);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(0, endPosition, 0);
        yield return null;
    }
}
