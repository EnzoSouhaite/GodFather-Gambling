using System;
using UnityEngine;

// Author : Auguste Paccapelo

public interface ICrouchController : IController
{
    // ---------- VARIABLES ---------- \\

// ----- Prefabs & Assets ----- \\

// ----- Objects ----- \\

// ----- Events ----- \\

    public event Action onCrouchStart;
    public event Action onCrouchEnd;

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\
}