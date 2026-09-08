using UnityEditor;

// Author : Auguste Paccapelo

[CustomEditor(typeof(SOCameraSettings))]
public class SOCameraSettingsEditor : Editor
{
    // ---------- VARIABLES ---------- \\
    
    // NAMES

    // Behavior
    private const string _followTargetOnAxisName = "followTargetOnAxis";
    private const string _lookAtAxisName = "lookAtAxis";
    private const string _linkMethodName = "linkMethod";

    // Orbital
    private const string _usePresetOrbitalName = "usePresetOrbital";
    private const string _orbitalRadiusName = "orbitalRadius";
    private const string _startAngleName = "startAngle";

    // Clamps
    private const string _clampOrbitalAngleName = "clampOrbitalAngle";

    private const string _horizontalClampName = "horizontalClamp";
    private const string _horizontalMinName = "horizontalMin";
    private const string _horizontalMaxName = "horizontalMax";

    private const string _verticalClampName = "verticalClamp";
    private const string _verticalMinName = "verticalMin";
    private const string _verticalMaxName = "verticalMax";

    // Offsets
    private const string _usePresetOffsetsName = "usePresetOffsets";
    private const string _positionOffsetName = "positionOffset";
    private const string _rotationOffsetName = "rotationOffset";
    private const string _orbitalCenterOffsetName = "orbitalCenterOffset";

    // SERIALIZED PROPERTIES

    // Behavior
    private SerializedProperty _followTargetOnAxis;
    private SerializedProperty _lookAtAxis;
    private SerializedProperty _linkMethod;

    // Orbital
    private SerializedProperty _usePresetOrbital;
    private SerializedProperty _orbitalRadius;
    private SerializedProperty _startAngle;

    // Clamps
    private SerializedProperty _clampOrbitalAngle;

    private SerializedProperty _horizontalClamp;
    private SerializedProperty _horizontalMin;
    private SerializedProperty _horizontalMax;

    private SerializedProperty _verticalClamp;
    private SerializedProperty _verticalMin;
    private SerializedProperty _verticalMax;

    // Offsets
    private SerializedProperty _usePresetOffsets;
    private SerializedProperty _positionOffset;
    private SerializedProperty _rotationOffset;
    private SerializedProperty _orbitalCenterOffset;

    // ---------- FUNCTIONS ---------- \\

    // ----- Buil-in ----- \\

    private void OnEnable()
    {
        _followTargetOnAxis = serializedObject.FindProperty(_followTargetOnAxisName);
        _lookAtAxis = serializedObject.FindProperty(_lookAtAxisName);
        _linkMethod = serializedObject.FindProperty(_linkMethodName);

        _usePresetOrbital = serializedObject.FindProperty(_usePresetOrbitalName);
        _orbitalRadius = serializedObject.FindProperty(_orbitalRadiusName);
        _startAngle = serializedObject.FindProperty(_startAngleName);

        _clampOrbitalAngle = serializedObject.FindProperty(_clampOrbitalAngleName);

        _horizontalClamp = serializedObject.FindProperty(_horizontalClampName);
        _horizontalMin = serializedObject.FindProperty(_horizontalMinName);
        _horizontalMax = serializedObject.FindProperty(_horizontalMaxName);

        _verticalClamp = serializedObject.FindProperty(_verticalClampName);
        _verticalMin = serializedObject.FindProperty(_verticalMinName);
        _verticalMax = serializedObject.FindProperty(_verticalMaxName);

        _usePresetOffsets = serializedObject.FindProperty(_usePresetOffsetsName);
        _positionOffset = serializedObject.FindProperty(_positionOffsetName);
        _rotationOffset = serializedObject.FindProperty(_rotationOffsetName);
        _orbitalCenterOffset = serializedObject.FindProperty(_orbitalCenterOffsetName);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawBehaviorProperties();

        EditorGUILayout.PropertyField(_usePresetOrbital);
        if (_usePresetOrbital.boolValue)
        {
            DrawOrbitalProperties();
        }

        DrawClampsProperties();

        DrawOffsetsProperties();

        serializedObject.ApplyModifiedProperties();
    }

    // ----- My Functions ----- \\

    private void DrawBehaviorProperties()
    {
        EditorGUILayout.PropertyField(_followTargetOnAxis);
        EditorGUILayout.PropertyField(_lookAtAxis);
        EditorGUILayout.PropertyField(_linkMethod);
    }

    private void DrawOrbitalProperties()
    {
        EditorGUILayout.PropertyField(_orbitalRadius);
        EditorGUILayout.PropertyField(_startAngle);
    }

    private void DrawClampsProperties()
    {
        EditorGUILayout.PropertyField(_clampOrbitalAngle);

        if (((AxisFlag)_clampOrbitalAngle.enumValueFlag).HasFlag(AxisFlag.X))
        {
            DrawHorizontalClamps();
        }

        if (((AxisFlag)_clampOrbitalAngle.enumValueFlag).HasFlag(AxisFlag.Y))
        {
            DrawVerticalClamps();
        }
    }

    private void DrawHorizontalClamps()
    {
        EditorGUILayout.PropertyField(_horizontalClamp);

        if (((ClampParameter)_horizontalClamp.enumValueFlag).HasFlag(ClampParameter.Min))
        {
            EditorGUILayout.PropertyField(_horizontalMin);
        }

        if (((ClampParameter)_horizontalClamp.enumValueFlag).HasFlag(ClampParameter.Max))
        {
            EditorGUILayout.PropertyField(_horizontalMax);
        }
    }

    private void DrawVerticalClamps()
    {
        EditorGUILayout.PropertyField(_verticalClamp);

        if (((ClampParameter)_verticalClamp.enumValueFlag).HasFlag(ClampParameter.Min))
        {
            EditorGUILayout.PropertyField(_verticalMin);
        }

        if (((ClampParameter)_verticalClamp.enumValueFlag).HasFlag(ClampParameter.Max))
        {
            EditorGUILayout.PropertyField(_verticalMax);
        }
    }

    private void DrawOffsetsProperties()
    {
        EditorGUILayout.PropertyField(_usePresetOffsets);

        if ((target as SOCameraSettings).HasPositionOffset())
        {
            EditorGUILayout.PropertyField(_positionOffset);
        }

        if ((target as SOCameraSettings).HasRotationOffset())
        {
            EditorGUILayout.PropertyField(_rotationOffset);
        }

        if ((target as SOCameraSettings).HasOrbitalCenterOffset())
        {
            EditorGUILayout.PropertyField(_orbitalCenterOffset);
        }
    }
}