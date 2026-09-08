using System;
using UnityEngine;

// Author : Auguste Paccapelo

public interface IMoveController : IController
{
    // ---------- VARIABLES ---------- \\

    // ----- Prefabs & Assets ----- \\

    // ----- Objects ----- \\

    // ----- Events ----- \\

    public event Action<Vector2> onMoveValue;
    public event Action onMove;

    // ----- Others ----- \\

    // ---------- FUNCTIONS ---------- \\
}