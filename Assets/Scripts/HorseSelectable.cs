using System;
using UnityEngine;

public class HorseSelectable : MonoBehaviour
{
    [SerializeField] private int _number = 0;
    public int Number => _number;

    bool _isSelected;
    public bool IsSelected => _isSelected;

    [SerializeField] private SpriteRenderer _outline;

    public void Select()
    {
        _isSelected = true;
        _outline.gameObject.SetActive(true);
    }

    public void UnSelect()
    {
        _isSelected = false;
        _outline.gameObject.SetActive(false);
    }
}