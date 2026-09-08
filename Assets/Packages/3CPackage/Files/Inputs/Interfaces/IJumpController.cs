using System;
using UnityEngine;

// Author : Auguste Paccapelo

public interface IJumpController : IController
{
    // ---------- VARIABLES ---------- \\

// ----- Prefabs & Assets ----- \\

// ----- Objects ----- \\

// ----- Events ----- \\

    public event Action onJumpStart;
    public event Action onJumpPerformed;
    public event Action onJumpEnd;

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\
}