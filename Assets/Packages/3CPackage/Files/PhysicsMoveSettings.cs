using UnityEngine;

// Author : Auguste Paccapelo

[CreateAssetMenu(fileName = "PhysicsMoveSettings", menuName = "ScriptableObject/PhysicsMoveSettings", order = 0)]
public class PhysicsMoveSettings : ScriptableObject
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Others ----- \\

    public float walkSpeed;

    public bool canSprint;
    public float runSpeed;

    public bool canSlow;
    public float slowSpeed;

    public bool canCrouch;
    public float crouchSpeed;    
    

    [Header("Jump Settings")]
    public bool canJump;
    public bool canMultiJump;
    public int numJumps;
    public float jumpForce;
    public bool differentJumpForces;
    public bool decreaseJumpForce;
    public float jumpForceLost;
    public int[] presetJumpForces;

    // ---------- FUNCTIONS ---------- \\

    public float GetJumpForce(int jumpDone = 0)
    {
        if (!canJump) return 0;

        float force;

        if (canMultiJump && differentJumpForces)
        {
            if (decreaseJumpForce)
            {
                force = jumpForce - jumpForceLost * jumpDone;
            }
            else
            {
                force = presetJumpForces[jumpDone];
            }
        }
        else
        {
            force = jumpForce;
        }

        return force;
    }
}