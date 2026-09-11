using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public static int[] winningHorses;

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
        switch (_currentHorsesSelected.Count)
        {
            case 1:
                SoundManager.Instance.PlaySound(SoundEnum.SelectHorseFirst);
                break;
            case 2:
                SoundManager.Instance.PlaySound(SoundEnum.SelectHorseSecond);
                break;
            case 3:
                SoundManager.Instance.PlaySound(SoundEnum.SelectHorseThird);
                break;
        }
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
        winningHorses = winHorses;

        Debug.Log($"[BetManager] L'ordre officiel d'arrivée est : {string.Join(" - ", winHorses)}");
        
        List<SOBetInfo> losers = new List<SOBetInfo>();
        Dictionary<SOBetInfo, int> winners = new Dictionary<SOBetInfo, int>();
        int num;

        foreach (SOBetInfo bet in _bets.Values)
        {
            num = bet.GetNumHorseWellPlaced(winHorses);
            if (num != 0)
            {
                winners.Add(bet, num);
            }
            else
            {
                losers.Add(bet);
            }
        }

        if (winners.Count > 0)
        {
            // Affiche le nom et le nombre de chevaux trouvés pour chaque gagnant
            string winnerNames = string.Join(", ", winners.Select(kvp => $"{kvp.Key.Name} ({kvp.Value} bon(s) cheval/chevaux)"));
            Debug.Log($"[BetManager] 🎉 Joueur(s) gagnant(s) : {winnerNames}");
        }
        else
        {
            Debug.Log("[BetManager] ❌ Aucun joueur n'a trouvé de cheval gagnant.");
        }

        float totalPool = _bets.Values.Select(i => i.Bet).Sum();
        totalPool *= 1f;
        float winningStakes = winners.Keys.Select(i => i.Bet).Sum();

        SOPlayerInfo playerInfo;
        float amount;
        float realAmount;
        float wonAmount;
        float numHorses = winHorses.Length;

        foreach (SOBetInfo bet in losers)
        {
            bet.SetWonAmount(-bet.Bet);
        }

        foreach (SOBetInfo betInfo in winners.Keys)
        {
            amount = totalPool * (betInfo.Bet / winningStakes);
            realAmount = betInfo.Bet + (amount - betInfo.Bet) * (winners[betInfo] / numHorses);
            if (realAmount < betInfo.Bet) realAmount = betInfo.Bet;
            wonAmount = realAmount - betInfo.Bet;

            playerInfo = AccountsManager.GetPlayer(betInfo.Name);
            playerInfo.MakeTransaction((int)realAmount);
            betInfo.SetWonAmount((int)wonAmount);
            
            Debug.Log($"[BetManager] 💰 {betInfo.Name} remporte {realAmount} jetons ! (Gain partiel : {winners[betInfo]}/{numHorses} chevaux trouvés)");
        }

        SceneManager.LoadScene(3);
    }

    public static void ClearBets()
    {
        _bets.Clear();
    }
}