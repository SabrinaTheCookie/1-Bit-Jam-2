using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Components")] [SerializeField]
    private CinemachineVirtualCamera vCam;

    private CinemachineBasicMultiChannelPerlin vCamNoise;

    [Header("Rotation")]
    public bool isRotating;

    private bool antiClockwise;
    public bool isHoldingRotate;
    public float rotationSteps = 90f;

    public float rotationDuration;
    public AnimationCurve rotationCurve;

    [Header("Camera Shake")] 
    public bool isShaking;
    public float shakeAmplitude;
    public float shakeFrequency;


    private void OnEnable()
    {
        FloorTraversal.OnTraversalStarted += StartCameraShake;
        FloorTraversal.OnTraversalEnded += EndCameraShake;
        InputManager.OnMovePressed += InputRotation;
        InputManager.OnMoveReleased += EndRotation;
    }
    
    private void OnDisable()
    {
        FloorTraversal.OnTraversalStarted -= StartCameraShake;
        FloorTraversal.OnTraversalEnded -= EndCameraShake;
        InputManager.OnMovePressed -= InputRotation;
        InputManager.OnMoveReleased -= EndRotation;
    }

    private void Awake()
    {
        vCamNoise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    [ContextMenu("RotateCamera")]
    public void RotateCamera() { RotateCamera(false);}
    public void RotateCamera(bool antiClockwise)
    {
        if (isRotating) return;
        StartCoroutine(RotateLerp(antiClockwise));
    }

    public void InputRotation(Vector2 input)
    {
        if(input.x == 0) return;

        isHoldingRotate = true;
        antiClockwise = input.x > 0;
        RotateCamera(antiClockwise);
    }

    public void EndRotation()
    {
        isHoldingRotate = false;
    }

    public IEnumerator RotateLerp(bool rotateAntiClockwise)
    {
        isRotating = true;
        StartCameraShake(0.5f);
        float t = 0;
        float startRotation = transform.rotation.eulerAngles.y;
        float endRotation = startRotation + rotationSteps * (rotateAntiClockwise ? -1 : 1);
        while (t < rotationDuration)
        {
            if(!isShaking) StartCameraShake(0.5f);
            transform.rotation = Quaternion.Euler(0, Mathf.Lerp(startRotation, endRotation, rotationCurve.Evaluate(t / rotationDuration)), 0);
            t += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, endRotation, 0);
        EndCameraShake();
        isRotating = false;
        if(isHoldingRotate) RotateCamera(antiClockwise);
        yield return null;
    }

    private void StartCameraShake() { StartCameraShake(1);}
    private void StartCameraShake(float intensityMultiplier)
    {
        isShaking = true;
        vCamNoise.m_AmplitudeGain = shakeAmplitude * intensityMultiplier;
        vCamNoise.m_FrequencyGain = shakeFrequency * intensityMultiplier;
    }
    
    private void EndCameraShake()
    {
        isShaking = false;
        vCamNoise.m_AmplitudeGain = 0;
        vCamNoise.m_FrequencyGain = 0; 
    }

    

}
