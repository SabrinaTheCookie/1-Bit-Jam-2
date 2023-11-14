using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private PlayerInput playerInput;
    [SerializeField] private Vector2 pointerPositionScreenSpace;
    [SerializeField] private Vector3 lookDelta;
    [SerializeField] private int traverseDelta;
    [SerializeField] private int rotateDelta;

    public static event Action OnPrimaryUpdated;
    public static event Action<Vector2> OnLookUpdated;
    public static event Action<int> OnTraversePressed;
    public static event Action OnTraverseReleased;
    public static event Action<int> OnRotatePressed;
    public static event Action OnRotateReleased;

    void Awake()
    {
        if(playerInput == null) playerInput = GetComponent<PlayerInput>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!playerInput.camera)
            playerInput.camera = Camera.main;
    }

    public void OnTraverse(InputAction.CallbackContext context)
    {
        traverseDelta = Mathf.RoundToInt(context.ReadValue<float>());

        if (context.performed)
        {
            OnTraversePressed?.Invoke(traverseDelta);
        }
        else if (context.canceled)
        {
            OnTraverseReleased?.Invoke();
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        rotateDelta = Mathf.RoundToInt(context.ReadValue<float>());
        if (context.performed)
        {
            OnRotatePressed?.Invoke(rotateDelta);
        }
        else if (context.canceled)
        {
            OnRotateReleased?.Invoke();
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 pos = context.ReadValue<Vector2>();
        lookDelta = pos;
        OnLookUpdated?.Invoke(lookDelta);

    }

    public void OnPrimary(InputAction.CallbackContext context)
    {
        OnPrimaryUpdated?.Invoke();
    }

    public void OnPointerPosition(InputAction.CallbackContext context)
    {
        Vector2 pos = context.ReadValue<Vector2>();
        pos = new Vector2(Mathf.Clamp(pos.x / Screen.width, 0f, 1f), Mathf.Clamp(pos.y / Screen.height, 0f, 1f));
        pointerPositionScreenSpace = pos;

    }


}
