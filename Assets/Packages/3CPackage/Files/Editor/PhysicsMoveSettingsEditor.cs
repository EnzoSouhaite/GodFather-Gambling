using UnityEditor;

// Author : Auguste Paccapelo

[CustomEditor(typeof(PhysicsMoveSettings))]
public class PhysicsMoveSettingsEditor : Editor
{
    private const string _canSprintName = "canSprint";
    private const string _canSlowName = "canSlow";
    private const string _canCrouchName = "canCrouch";

    private const string _walkSpeedName = "walkSpeed";
    private const string _runSpeedName = "runSpeed";
    private const string _slowSpeedName = "slowSpeed";
    private const string _crouchSpeedName = "crouchSpeed";

    // Jump Settings
    private const string _canJumpName = "canJump";
    private const string _canMultiJumpName = "canMultiJump";
    private const string _numJumpsName = "numJumps";
    private const string _jumpForceName = "jumpForce";
    private const string _differentJumpForcesName = "differentJumpForces";
    private const string _decreaseJumpForceName = "decreaseJumpForce";
    private const string _jumpForceLostName = "jumpForceLost";
    private const string _presetJumpForcesName = "presetJumpForces";

    private SerializedProperty _canSprint;
    private SerializedProperty _canSlow;
    private SerializedProperty _canCrouch;

    private SerializedProperty _walkSpeed;
    private SerializedProperty _runSpeed;
    private SerializedProperty _slowSpeed;
    private SerializedProperty _crouchSpeed;

    // Jump Settings
    private SerializedProperty _canJump;
    private SerializedProperty _canMultiJump;
    private SerializedProperty _numJumps;
    private SerializedProperty _jumpForce;
    private SerializedProperty _differentJumpForces;
    private SerializedProperty _decreaseJumpForce;
    private SerializedProperty _jumpForceLost;
    private SerializedProperty _presetJumpForces;

    private void OnEnable()
    {
        _canSprint = serializedObject.FindProperty(_canSprintName);
        _canSlow = serializedObject.FindProperty(_canSlowName);
        _canCrouch = serializedObject.FindProperty(_canCrouchName);

        _walkSpeed = serializedObject.FindProperty(_walkSpeedName);
        _runSpeed = serializedObject.FindProperty(_runSpeedName);
        _slowSpeed = serializedObject.FindProperty(_slowSpeedName);
        _crouchSpeed = serializedObject.FindProperty(_crouchSpeedName);

        // Jump Settings
        _canJump = serializedObject.FindProperty(_canJumpName);
        _jumpForce = serializedObject.FindProperty(_jumpForceName);
        _canMultiJump = serializedObject.FindProperty(_canMultiJumpName);
        _numJumps = serializedObject.FindProperty(_numJumpsName);
        _differentJumpForces = serializedObject.FindProperty(_differentJumpForcesName);
        _decreaseJumpForce = serializedObject.FindProperty(_decreaseJumpForceName);
        _jumpForceLost = serializedObject.FindProperty(_jumpForceLostName);
        _presetJumpForces = serializedObject.FindProperty(_presetJumpForcesName);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(_walkSpeed);

        EditorGUILayout.PropertyField(_canSprint);
        if (_canSprint.boolValue)
        {
            EditorGUILayout.PropertyField(_runSpeed);
        }

        EditorGUILayout.PropertyField(_canSlow);
        if (_canSlow.boolValue)
        {
            EditorGUILayout.PropertyField(_slowSpeed);
        }

        EditorGUILayout.PropertyField(_canCrouch);
        if (_canCrouch.boolValue)
        {
            EditorGUILayout.PropertyField(_crouchSpeed);
        }

        // Jump Settings
        EditorGUILayout.PropertyField(_canJump);
        if (_canJump.boolValue)
        {
            DrawJumpSettings();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawJumpSettings()
    {
        EditorGUILayout.PropertyField(_jumpForce);
        EditorGUILayout.PropertyField(_canMultiJump);

        if (_canMultiJump.boolValue)
        {
            DrawMultiJump();
        }
    }

    private void DrawMultiJump()
    {
        EditorGUILayout.PropertyField(_numJumps);
        EditorGUILayout.PropertyField(_differentJumpForces);

        if (_differentJumpForces.boolValue)
        {
            DrawDiffJumpForces();
        }
    }

    private void DrawDiffJumpForces()
    {
        EditorGUILayout.PropertyField(_decreaseJumpForce);

        if (_decreaseJumpForce.boolValue)
        {
            EditorGUILayout.PropertyField(_jumpForceLost);
        }
        else
        {
            EditorGUILayout.PropertyField(_presetJumpForces);
        }
    }
}