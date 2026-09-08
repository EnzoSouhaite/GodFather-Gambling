using System.Collections.Generic;
using UnityEngine;

// Author : Auguste Paccapelo

[ExecuteAlways]
public class NewCameraController : MonoBehaviour
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    [SerializeField] private Transform _target;
    public Transform Target { get =>  _target; }

    [SerializeField] private SOCameraSettings _cameraSettings;
    public SOCameraSettings CameraSettings { get =>  _cameraSettings; }

    private IMouseXYAxisController _controller;
    public IMouseXYAxisController Controller { get => _controller; }

    private CameraState _cameraState;

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        // Will be removed & the function will have to be called another way (so not forced to use THIS inputs)
        _controller = GetComponentInChildren<IMouseXYAxisController>();

        _controller.onMouseValue += OnMouseMove;
    }

    private void OnDisable()
    {
        _controller.onMouseValue -= OnMouseMove;
    }

    private void Awake() { }

    private void Start()
    {
        SetStateMachine();
    }

    private void Update()
    {
        if (_target == null) return;
        if (_cameraSettings == null) return;

        if (_cameraState == null)
        {
            if (Application.isPlaying)
            {
                return;
            }
            else
            {
                SetStateMachine();
            }
        }

        _cameraState.Update();
    }

    private void LateUpdate()
    {
        if (_target == null) return;
        if (_cameraSettings == null) return;
        if (_cameraState == null) return;

        _cameraState.LateUpdate();
    }

    private void OnDrawGizmos()
    {
        if (_target == null || _cameraSettings == null) return;
        DrawGizmos();
    }

    // ----- My Functions ----- \\

    private void SetStateMachine()
    {
        // Will be reworked for transitions
        _cameraState = new FreeCameraState();
        _cameraState.Enter(transform, _target, null, _cameraSettings);
    }

    private void OnMouseMove(Vector2 obj)
    {
        if (_cameraState == null) return;

        _cameraState.OnMouseMove(obj);
    }

    private void DrawGizmos()
    {
        if (_cameraState == null) return;

        _cameraState.DrawGizmos();
    }

    // ----- Destructor ----- \\

    private void OnDestroy() { }
}