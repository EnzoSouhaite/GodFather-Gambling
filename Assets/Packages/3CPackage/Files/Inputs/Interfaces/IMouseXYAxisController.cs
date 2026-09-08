using System;
using UnityEngine;

// Author : Auguste Paccapelo

public interface IMouseXYAxisController : IController
{
    // ---------- VARIABLES ---------- \\

    // ----- Events ----- \\

    public event Action<Vector2> onMouseValue;
    public event Action onMouse;
}