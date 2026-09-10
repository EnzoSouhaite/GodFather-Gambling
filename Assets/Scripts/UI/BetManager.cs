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

        RemoveHorseTrophies();

        int length = _currentHorsesSelected.Count;
        for (int i = 0; i < length; i++)
        {
            _currentHorsesSelected[i].SetTrophy(i);
        }
    }

    public static void SelectHorse(HorseSelectable horse)
    {
        if (_currentHorsesSelected.Count >= 3) return;

        horse.SetTrophy(_currentHorsesSelected.Count);
        _currentHorsesSelected.Add(horse);
    }

    private static void RemoveHorseTrophies()
    {
        foreach (HorseSelectable horse in _currentHorsesSelected)
        {
            horse.RemoveTrophy();
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
        Dictionary<SOBetInfo, int> winners = new Dictionary<SOBetInfo, int>();
        int num;

        foreach (SOBetInfo bet in _bets.Values)
        {
            num = bet.GetNumHorseWellPlaced(winHorses);
            if (num != 0)
            {
                winners.Add(bet, num);
            }
        }

        int totalPool = _bets.Values.Select(i => i.Bet).Sum();
        totalPool += (int)(totalPool * 0.1f);
        int winningStakes = winners.Keys.Select(i => i.Bet).Sum();

        SOPlayerInfo playerInfo;
        int amount;
        int realAmount;
        int numHorses = winHorses.Length;
        foreach (SOBetInfo betInfo in winners.Keys)
        {
            amount = winningStakes * (betInfo.Bet / totalPool);
            realAmount = betInfo.Bet + (amount - betInfo.Bet) * (winners[betInfo] / numHorses);
            if (realAmount < betInfo.Bet) realAmount = betInfo.Bet;

            playerInfo = AccountsManager.GetPlayer(betInfo.Name);
            playerInfo.MakeTransaction(realAmount);
            betInfo.SetWonAmount(realAmount);
        }
    }

    public static void ClearBets()
    {
        _bets.Clear();
    }
}