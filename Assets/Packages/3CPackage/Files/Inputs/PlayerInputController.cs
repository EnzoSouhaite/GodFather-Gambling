using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Author : Auguste Paccapelo

public class PlayerInputController : MonoBehaviour, 
    IMoveController, ISprintController, ICrouchController, IJumpController, IInteractController
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    [SerializeField] private InputActionReference _inputActionMove;
    [SerializeField] private InputActionReference _inputActionSprint;
    [SerializeField] private InputActionReference _inputActionCrouch;
    [SerializeField] private InputActionReference _inputActionJump;
    [SerializeField] private InputActionReference _inputActionInteract;

    // ----- Events ----- \\

    public event Action<Vector2> onMoveValue;
    public event Action onMove;
    public event Action onSprintStart;
    public event Action onSprintEnd;
    public event Action onCrouchStart;
    public event Action onCrouchEnd;
    public event Action onJumpStart;
    public event Action onJumpPerformed;
    public event Action onJumpEnd;
    public event Action onInteract;

    // ----- Others ----- \\

    private bool _isSprintHold = true;
    private bool _isSprinting = false;

    private bool _isCrouchHold = true;
    private bool _isCrouched = false;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        ConnectInputActions();
    }

    private void OnDisable()
    {
        DisconnectInputActions();
    }

    private void Awake() { }

    private void Start() { }

    private void Update() { }

    // ----- My Functions ----- \\

    #region InputActions

    #region Connection

    private void ConnectInputActions()
    {
        ConnectMoveInputAction();
        ConnectSprintInputAction();
        ConnectCrouchInputAction();
        ConnectJumpInputAction();
        ConnectInteractInputAction();
    }

    private void ConnectMoveInputAction()
    {
        if (_inputActionMove == null || _inputActionMove.action == null) return;

        _inputActionMove.action.started += OnMoveStarted;
        _inputActionMove.action.performed += OnMovePerformed;
        _inputActionMove.action.canceled += OnMoveCanceled;
    }

    private void ConnectSprintInputAction()
    {
        if (_inputActionSprint == null || _inputActionSprint.action == null) return;

        _inputActionSprint.action.started += OnSprintStarted;
        _inputActionSprint.action.canceled += OnSprintCanceled;
    }

    private void ConnectCrouchInputAction()
    {
        if (_inputActionCrouch == null || _inputActionCrouch.action == null) return;

        _inputActionCrouch.action.started += OnCrouchStarted;
        _inputActionCrouch.action.canceled += OnCrouchCanceled;
    }

    private void ConnectJumpInputAction()
    {
        if (_inputActionJump == null || _inputActionJump.action == null) return;

        _inputActionJump.action.started += OnJumpStarted;
        _inputActionJump.action.performed += OnJumpPerformed;
        _inputActionJump.action.canceled += OnJumpCanceled;
    }

    private void ConnectInteractInputAction()
    {
        if (_inputActionInteract == null || _inputActionInteract.action == null) return;

        _inputActionInteract.action.started += OnInteractStarted;
    }

    #endregion

    #region Disconnection

    private void DisconnectInputActions()
    {
        DisconnectMoveInputAction();
        DisconnectSprintInputAction();
        DisconnectCrouchInputAction();
        DisconnectJumpInputAction();
        DisconnectInteractInputAction();
    }

    private void DisconnectMoveInputAction()
    {
        if (_inputActionMove == null || _inputActionMove.action == null) return;

        _inputActionMove.action.started -= OnMoveStarted;
        _inputActionMove.action.canceled -= OnMoveCanceled;
    }

    private void DisconnectSprintInputAction()
    {
        if (_inputActionSprint == null || _inputActionSprint.action == null) return;

        _inputActionSprint.action.started -= OnSprintStarted;
        _inputActionSprint.action.canceled -= OnSprintCanceled;
    }

    private void DisconnectCrouchInputAction()
    {
        if (_inputActionCrouch == null || _inputActionCrouch.action == null) return;

        _inputActionCrouch.action.started -= OnCrouchStarted;
        _inputActionCrouch.action.canceled -= OnCrouchCanceled;
    }

    private void DisconnectJumpInputAction()
    {
        if (_inputActionJump == null || _inputActionJump.action == null) return;

        _inputActionJump.action.started -= OnJumpStarted;
        _inputActionJump.action.performed -= OnJumpPerformed;
        _inputActionJump.action.canceled -= OnJumpCanceled;
    }

    private void DisconnectInteractInputAction()
    {
        if (_inputActionInteract == null || _inputActionInteract.action == null) return;

        _inputActionInteract.action.started -= OnInteractStarted;
    }

    #endregion

    #region CallBacks

    #region Move

    private void CallMoveEvents(Vector2 inputDir)
    {
        onMove?.Invoke();
        onMoveValue?.Invoke(inputDir);
    }

    private void OnMoveStarted(InputAction.CallbackContext obj)
    {
        CallMoveEvents(obj.ReadValue<Vector2>());
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        CallMoveEvents(obj.ReadValue<Vector2>());
    }

    private void OnMovePerformed(InputAction.CallbackContext obj)
    {
        CallMoveEvents(obj.ReadValue<Vector2>());
    }

    #endregion

    #region Sprint

    private void OnSprintStarted(InputAction.CallbackContext obj)
    {
        if (_isSprintHold)
        {
            _isSprinting = true;
            onSprintStart?.Invoke();
        }
        else
        {
            if (_isSprinting)
            {
                _isSprinting = false;
                onSprintEnd?.Invoke();
            }
            else
            {
                _isSprinting = true;
                onSprintStart?.Invoke();
            }
        }
    }

    private void OnSprintCanceled(InputAction.CallbackContext obj)
    {
        if (_isSprintHold)
        {
            _isSprinting = false;
            onSprintEnd?.Invoke();
        }
    }

    #endregion

    #region Crouch
    
    private void OnCrouchStarted(InputAction.CallbackContext obj)
    {
        if (_isCrouchHold)
        {
            _isCrouched = true;
            onCrouchStart?.Invoke();
        }
        else
        {
            if (_isCrouched)
            {
                _isCrouched = false;
                onCrouchEnd?.Invoke();
            }
            else
            {
                _isCrouched = true;
                onCrouchStart?.Invoke();
            }
        }
    }

    private void OnCrouchCanceled(InputAction.CallbackContext obj)
    {
        if (_isCrouchHold)
        {
            _isCrouched = false;
            onCrouchEnd?.Invoke();
        }
    }

    #endregion

    #region Jump

    private void OnJumpStarted(InputAction.CallbackContext obj)
    {
        onJumpStart?.Invoke();
    }

    private void OnJumpPerformed(InputAction.CallbackContext obj)
    {
        onJumpPerformed?.Invoke();
    }

    private void OnJumpCanceled(InputAction.CallbackContext obj)
    {
        onJumpEnd?.Invoke();
    }

    #endregion

    #region Interact

    private void OnInteractStarted(InputAction.CallbackContext obj)
    {
        onInteract?.Invoke();
    }

    #endregion

    #endregion

    #endregion

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}