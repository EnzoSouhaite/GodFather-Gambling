using UnityEngine;

// Author : Auguste Paccapelo

public abstract class CameraState
{
    protected SOCameraSettings _settings;
    protected Transform _camera;
    protected Transform _target;

    protected Vector3 _orbitalCenterPoint;
    protected Vector3 _orbitalPosition;
    
    protected Vector3 _positionOffset { get => _settings.HasPositionOffset() ? _settings.positionOffset : Vector3.zero; }
    protected float _orbitalRadius { get => _settings.orbitalRadius; }

    protected Vector2 _angles;
    protected Quaternion _orbitalRotation = Quaternion.identity;
    protected Vector3 _directionFromTarget
    {
        get => _settings.linkMethod == RelationType.CameraAdaptToTarget ? -_target.forward : -Vector3.forward;
    }

    public virtual void Enter(Transform camera, Transform target, CameraState lastState = null, SOCameraSettings settings = null)
    {
        _settings = settings;
        _camera = camera;
        _target = target;

        ResetSettings();

        if (lastState != null)
        {
            GetLastStateValues(lastState);
        }
    }
    public virtual void Exit() { }

    protected virtual void GetLastStateValues(CameraState state)
    {
        _orbitalCenterPoint = state._orbitalCenterPoint;
        _orbitalPosition = state._orbitalPosition;
        _angles = state._angles;
        _orbitalRotation = state._orbitalRotation;
    }

    public virtual void ResetSettings(SOCameraSettings settings = null)
    {
        if (settings == null) settings = _settings;

        if (settings == null || _target == null) return;

        _orbitalPosition = Vector3.zero;

        _angles = Vector3.zero;

        _orbitalCenterPoint = _target.position;
    }

    protected virtual void FollowTarget(AxisFlag followTargetOnAxis)
    {
        Vector3 axisToFollow = AxisFlagToVector(followTargetOnAxis);
        Vector3 targetPos = MultVectors(axisToFollow, _target.position);
        Vector3 orbitalPos = MultVectors(NotBoolVector(axisToFollow), _orbitalCenterPoint);
        _orbitalCenterPoint = targetPos + orbitalPos;
        if (_settings.HasOrbitalCenterOffset()) _orbitalCenterPoint += _settings.orbitalCenterOffset;
    }

    protected virtual void LookAtTarget(AxisFlag lookAtAxis)
    {
        Vector3 direction = _target.position - _camera.position;

        direction = MultVectors(direction, AxisFlagToVector(lookAtAxis));

        if (direction == Vector3.zero) return;

        Quaternion rotation = direction == Vector3.zero ?
            Quaternion.identity : // True
            Quaternion.LookRotation(direction); // False

        _camera.rotation = rotation;
    }

    protected virtual void LookAtTarget(AxisFlag lookAtAxis, Vector3 rotationOffset)
    {
        LookAtTarget(lookAtAxis);

        _camera.eulerAngles += rotationOffset;
    }

    protected virtual void UpdatePosition()
    {
        if (_orbitalPosition == Vector3.zero) return;

        Vector3 direction = -_orbitalPosition;

        Quaternion toTargetRotation = Quaternion.LookRotation(direction);
        Vector3 relativePosOffset = toTargetRotation * _positionOffset;

        _camera.position = _orbitalCenterPoint + _orbitalPosition + relativePosOffset;
    }

    public virtual void OnMouseMove(Vector2 obj) { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }
    public virtual void DrawGizmos() { }

    protected Vector3 AxisFlagToVector(AxisFlag axisFlag)
    {
        return new(
            axisFlag.HasFlag(AxisFlag.X) ? 1 : 0,
            axisFlag.HasFlag(AxisFlag.Y) ? 1 : 0,
            axisFlag.HasFlag(AxisFlag.Z) ? 1 : 0
            );
    }

    protected Vector3 MultVectors(Vector3 a, Vector3 b)
    {
        return new(
            a.x * b.x,
            a.y * b.y,
            a.z * b.z
            );
    }

    protected Vector3 NotBoolVector(Vector3 vec)
    {
        vec -= Vector3.one;
        return MultVectors(vec, vec);
    }
}