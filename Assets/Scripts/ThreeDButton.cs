using System;
using UnityEngine;
using UnityEngine.Events;

public class ThreeDButton : MonoBehaviour, ITouchableOnDown
{
    [SerializeField] UnityEvent _onInteract;

    public void OnTouchedDown()
    {
        _onInteract?.Invoke();
    }
}
