using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BetManager : MonoBehaviour
{
    private static BetManager _instance;
    public static BetManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("BetManager");
                _instance = obj.AddComponent<BetManager>();
            }

            return _instance;
        }
        private set => _instance = value;
    }

    private static InputAction _click;

    private static List<HorseSelectable> _currentHorsesSelected = new List<HorseSelectable>();

    private static Dictionary<string, SOBetInfo> _bets = new Dictionary<string, SOBetInfo>();

    public static event Action<SOBetInfo> OnNewBet;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        CreateBinding();
    }

    private void OnEnable()
    {
        _click.started += OnClick;
        _click.Enable();
    }

    private void OnDisable()
    {
        if (_click == null) return;

        _click.started -= OnClick;
        _click.Disable();
    }

    private static void CreateBinding()
    {
        if (_click != null) return;

        _click = new InputAction(
            name: "Click",
            type: InputActionType.Button,
            binding: "<Mouse>/leftButton"
        );
    }

    private static void OnClick(InputAction.CallbackContext obj)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, 0f);
        
        HorseSelectable horse;
        if (hit.collider != null && hit.collider.TryGetComponent<HorseSelectable>(out horse))
        {
            if (horse.IsSelected && _currentHorsesSelected.Contains(horse))
            {
                horse.UnSelect();
                _currentHorsesSelected.Remove(horse);
            }
            else if (!horse.IsSelected && _currentHorsesSelected.Count < 3 && !_currentHorsesSelected.Contains(horse))
            {
                horse.Select();
                _currentHorsesSelected.Add(horse);
            }
        }
    }

    public static void ClearSelection()
    {
        for (int i = _currentHorsesSelected.Count - 1; i >= 0; i--)
        {
            _currentHorsesSelected[i].UnSelect();
            _currentHorsesSelected.RemoveAt(i);
        }
    }
    
    public static void AddBet(SOPlayerInfo player, int bet)
    {
        if (_bets.ContainsKey(player.Name)) return;
        
        player.MakeTransaction(-bet);

        SOBetInfo soBet = ScriptableObject.CreateInstance<SOBetInfo>();
        soBet.Init(player.Name, bet, _currentHorsesSelected);

        _bets.Add(player.Name, soBet);
        OnNewBet?.Invoke(soBet);
    }
}