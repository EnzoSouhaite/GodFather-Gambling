using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HorseSelectable : MonoBehaviour
{
    private static Dictionary<int, Sprite> _sprites = new Dictionary<int, Sprite>();

    [SerializeField] private int _number = 0;
    public int Number => _number;

    bool _isSelected;
    public bool IsSelected => _isSelected;

    [SerializeField] private Sprite _image;
    [SerializeField] private Image _sprite;
    [SerializeField] private Image _outline;

    [SerializeField] private GameObject[] _trophies;

    private void Start()
    {
        _sprite.sprite = _image;
        _outline.sprite = _image;

        if (_sprites.ContainsKey(_number)) return;
        
        _sprites.Add(_number, _sprite.sprite);
    }

    public void Clicked()
    {
        if (_isSelected)
        {
            UnSelect();
            BetManager.UnselectHorse(this);
        }
        else if (BetManager.CanSelectNewHorse() && !BetManager.GetIsHorseSelected(this))
        {
            Select();
            BetManager.SelectHorse(this);
        }
    }

    public void Select()
    {
        _isSelected = true;
        _outline.gameObject.SetActive(true);
    }

    public void UnSelect()
    {
        _isSelected = false;
        _outline.gameObject.SetActive(false);
        RemoveTrophy();
    }

    public void SetTrophy(int placement)
    {
        _trophies[placement].SetActive(true);
    }

    public void RemoveTrophy()
    {
        foreach (GameObject trophy in _trophies)
        {
            trophy.SetActive(false);
        }
    }

    public static Sprite GetSprite(int num)
    {
        return _sprites.ContainsKey(num) ? _sprites[num] : null;
    }
}