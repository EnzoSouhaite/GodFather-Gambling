using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    }

    public static bool GetIsHorseSelected(HorseSelectable horse)
    {
        return _currentHorsesSelected.Contains(horse);
    }

    public static bool CanSelectNewHorse()
    {
        return _currentHorsesSelected.Count < 3;
    }

    public static void UnselectHorse(HorseSelectable horse)
    {
        if (!_currentHorsesSelected.Contains(horse)) return;

        _currentHorsesSelected.Remove(horse);
    }

    public static void SelectHorse(HorseSelectable horse)
    {
        if (_currentHorsesSelected.Count >= 3) return;

        _currentHorsesSelected.Add(horse);
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
        if (_bets.ContainsKey(player.Name) || _currentHorsesSelected.Count != 3) return;
        
        player.MakeTransaction(-bet);

        SOBetInfo soBet = ScriptableObject.CreateInstance<SOBetInfo>();
        soBet.Init(player.Name, bet, _currentHorsesSelected);

        _bets.Add(player.Name, soBet);
        OnNewBet?.Invoke(soBet);

        foreach (HorseSelectable horse in _currentHorsesSelected)
        {
            horse.UnSelect();
        }
        _currentHorsesSelected.Clear();
    }

    public static List<SOBetInfo> GetAllBets()
    {
        return _bets.Values.ToList();
    }

    public static void GameFinished(int[] winHorses)
    {
        List<SOBetInfo> winners = _bets.Values.Where(i => i.DoesWin(winHorses)).ToList();
        int totalPool = _bets.Values.Select(i => i.Bet).Sum();
        totalPool += (int)(totalPool * 0.1f);
        int winningStakes = winners.Select(i => i.Bet).Sum();

        SOPlayerInfo playerInfo;
        int amount;
        foreach (SOBetInfo betInfo in winners)
        {
            amount = winningStakes * (betInfo.Bet / totalPool);
            playerInfo = AccountsManager.GetPlayer(betInfo.Name);
            playerInfo.MakeTransaction(amount);
            betInfo.SetWonAmount(amount);
        }
    }
}