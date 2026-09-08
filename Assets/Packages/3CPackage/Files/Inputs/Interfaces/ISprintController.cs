using System;
using UnityEngine;

// Author : Auguste Paccapelo

public interface ISprintController : IController
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Events ----- \\

    public event Action onSprintStart;
    public event Action onSprintEnd;

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\
}