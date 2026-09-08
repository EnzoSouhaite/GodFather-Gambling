using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Author : Auguste Paccapelo

public class CameraLookInput : MonoBehaviour, IMouseXYAxisController
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    [SerializeField] private InputActionReference _inputActionLook;

    // ----- Others ----- \\

    // ----- Events ----- \\

    public event Action<Vector2> onMouseValue;
    public event Action onMouse;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        ConnectInputs();
    }

    private void OnDisable()
    {
        DisconnectInputs();
    }

    private void Awake() { }

    private void Start() { }

    private void Update() { }

    // ----- My Functions ----- \\

    private void OnMouseMoveStart(InputAction.CallbackContext obj)
    {
        onMouse?.Invoke();
        onMouseValue?.Invoke(obj.ReadValue<Vector2>());
    }

    private void OnMouseMovePerformed(InputAction.CallbackContext obj)
    {
        onMouse?.Invoke();
        onMouseValue?.Invoke(obj.ReadValue<Vector2>());
    }

    private void OnMouseMoveCanceled(InputAction.CallbackContext obj)
    {
        onMouse?.Invoke();
        onMouseValue?.Invoke(obj.ReadValue<Vector2>());
    }

    private void ConnectInputs()
    {
        _inputActionLook.action.started += OnMouseMoveStart;
        _inputActionLook.action.performed += OnMouseMovePerformed;
        _inputActionLook.action.canceled += OnMouseMoveCanceled;
    }

    private void DisconnectInputs()
    {
        _inputActionLook.action.started -= OnMouseMoveStart;
        _inputActionLook.action.performed -= OnMouseMovePerformed;
        _inputActionLook.action.canceled -= OnMouseMoveCanceled;
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}