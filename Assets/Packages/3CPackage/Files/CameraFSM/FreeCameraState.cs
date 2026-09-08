using UnityEngine;

// Author : Auguste Paccapelo

public class FreeCameraState : CameraState
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\

    // ----- FSM ----- \\

    public override void LateUpdate()
    {
        if (!Application.isPlaying)
        {           
            ResetSettings();
            OnMouseMove(Vector2.zero);
        }

        MoveOrbitalCamera(Vector2.zero);
        FollowTarget(_settings.followTargetOnAxis);
        UpdatePosition();
        LookAtTarget(_settings.lookAtAxis, _settings.HasRotationOffset() ? _settings.rotationOffset : Vector3.zero);
    }

    public override void OnMouseMove(Vector2 obj)
    {
        // Will be then removed when inputs will be seperated
        if (_settings.linkMethod == RelationType.CameraAdaptToTarget) return;

        MoveOrbitalCamera(obj);
    }

    public override void DrawGizmos()
    {
        base.DrawGizmos();

        if (_settings.usePresetOrbital)
        {
            Gizmos.DrawWireSphere(_orbitalCenterPoint, _settings.orbitalRadius);

            Vector3 previewPos = _orbitalPosition + _orbitalCenterPoint;

            Gizmos.DrawSphere(previewPos, 0.25f);
        }
    }

    // ----- My Functions ----- \\

    protected virtual void MoveOrbitalCamera(Vector2 mouseMove)
    {
        _angles += mouseMove * Time.deltaTime * Mathf.Rad2Deg;

        ClampAngle();

        Vector2 angle = (_angles + _settings.startAngle);

        _orbitalRotation = Quaternion.AngleAxis(-angle.x, Vector3.up);

        _orbitalRotation *= Quaternion.AngleAxis(-angle.y, Vector3.right);

        _orbitalPosition = _orbitalRotation * _directionFromTarget * _orbitalRadius;
    }

    protected virtual void ClampAngle()
    {
        if (_settings.clampOrbitalAngle.HasFlag(AxisFlag.X))
        {
            if (_settings.horizontalClamp.HasFlag(ClampParameter.Min)
                && _angles.x - _settings.startAngle.x < _settings.horizontalMin)
            {
                _angles.x = _settings.horizontalMin + _settings.startAngle.x;
            }

            if (_settings.horizontalClamp.HasFlag(ClampParameter.Max)
                && _angles.x + _settings.startAngle.x > _settings.horizontalMax)
            {
                _angles.x = _settings.horizontalMax - _settings.startAngle.x;
            }
        }

        if (_settings.clampOrbitalAngle.HasFlag(AxisFlag.Y))
        {
            if (_settings.verticalClamp.HasFlag(ClampParameter.Min)
                && _angles.y - _settings.startAngle.y < _settings.verticalMin)
            {
                _angles.y = _settings.verticalMin + _settings.startAngle.y;
            }

            if (_settings.verticalClamp.HasFlag(ClampParameter.Max)
                && _angles.y + _settings.startAngle.y > _settings.verticalMax)
            {
                _angles.y = _settings.verticalMax - _settings.startAngle.y;
            }
        }
    }

    // ----- Destructor ----- \\
}