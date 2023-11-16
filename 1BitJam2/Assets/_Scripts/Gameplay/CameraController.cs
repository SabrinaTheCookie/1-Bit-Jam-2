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
    public Transform BossFace;

    [Header("Rotation")]
    public bool isRotating;

    private bool antiClockwise;
    public bool isHoldingRotate;
    public float timeHoldingRotate;
    public float holdTimeForFastRotate;
    public float fastRotateSpeedMultiplier;
    public bool fastForwardActive;
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
        InputManager.OnRotatePressed += InputRotation;
        InputManager.OnRotateReleased += EndRotation;
    }
    
    private void OnDisable()
    {
        FloorTraversal.OnTraversalStarted -= StartCameraShake;
        FloorTraversal.OnTraversalEnded -= EndCameraShake;
        InputManager.OnRotatePressed -= InputRotation;
        InputManager.OnRotateReleased -= EndRotation;
    }

    private void Awake()
    {
        vCamNoise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (!isHoldingRotate) return;
        timeHoldingRotate += Time.deltaTime;
        if (!fastForwardActive && timeHoldingRotate > holdTimeForFastRotate)
        {
            fastForwardActive = true;
        }


    }

    [ContextMenu("RotateCamera")]
    public void RotateCamera() { RotateCamera(false);}
    public void RotateCamera(bool antiClockwise)
    {
        if (isRotating) return;
        StartCoroutine(RotateLerp(antiClockwise));
    }

    public void InputRotation(int input)
    {
        if(input == 0) return;

        isHoldingRotate = true;
        antiClockwise = input > 0;
        RotateCamera(antiClockwise);
    }

    public void EndRotation()
    {
        
        timeHoldingRotate = 0;
        fastForwardActive = false;
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
            Quaternion newRot = Quaternion.Euler(0, Mathf.Lerp(startRotation, endRotation, rotationCurve.Evaluate(t / rotationDuration)), 0);
            transform.rotation = newRot;
            BossFace.localRotation = newRot;
            t += Time.deltaTime * (fastForwardActive ? fastRotateSpeedMultiplier : 1);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, endRotation, 0);
        BossFace.localRotation = Quaternion.Euler(0, endRotation, 0);
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
