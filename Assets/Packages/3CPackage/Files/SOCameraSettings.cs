using System;
using UnityEngine;

// Author : Auguste Paccapelo

[Flags]
public enum AxisFlag
{
    None = 0,
    X = 2,
    Y = 4,
    Z = 8
};

public enum RelationType
{
    FreeCamera,
    CameraAdaptToTarget
}

[Flags]
public enum OffsetTypes
{
    None = 0,
    PositionOffset = 2,
    RotationOffset = 4,
    OrbitalCenterOffset = 8
}

[Flags]
public enum ClampParameter
{
    None = 0,
    Min = 2,
    Max = 4,
}

[CreateAssetMenu(fileName = "SOCameraSettings", menuName = "ScriptableObject/SOCameraSettings", order = 0)]
public class SOCameraSettings : ScriptableObject
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Events ----- \\
    
    // ----- Others ----- \\

    [Header("Behavior")]
    public AxisFlag followTargetOnAxis = AxisFlag.None;
    public AxisFlag lookAtAxis = AxisFlag.None;
    public RelationType linkMethod = RelationType.FreeCamera;

    [Header("Orbital")]
    public bool usePresetOrbital = true;

    public float orbitalRadius = 2.0f;
    public Vector2 startAngle = Vector2.zero;

    [Header("Clamp")]
    public AxisFlag clampOrbitalAngle = AxisFlag.None;

    public ClampParameter horizontalClamp = ClampParameter.None;
    public float horizontalMin = 0;
    public float horizontalMax = 0;

    public ClampParameter verticalClamp = ClampParameter.None;
    public float verticalMin = 0;
    public float verticalMax = 0;

    [Header("Offsets")]
    public OffsetTypes usePresetOffsets = OffsetTypes.None;

    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public Vector3 orbitalCenterOffset;

    // ---------- FUNCTIONS ---------- \\

    public bool HasPositionOffset()
    {
        return usePresetOffsets.HasFlag(OffsetTypes.PositionOffset);
    }

    public bool HasRotationOffset()
    {
        return usePresetOffsets.HasFlag(OffsetTypes.RotationOffset);
    }

    public bool HasOrbitalCenterOffset()
    {
        return usePresetOffsets.HasFlag(OffsetTypes.OrbitalCenterOffset);
    }

    public Vector3 GetPositionOffset()
    {
        return HasPositionOffset() ? positionOffset : Vector3.zero;
    }

    public Vector3 GetRotationOffset()
    {
        return HasRotationOffset() ? rotationOffset : Vector3.zero;
    }

    public Vector3 GetOrbitalCenterOffset()
    {
        return HasOrbitalCenterOffset() ? orbitalCenterOffset : Vector3.zero;
    }
}