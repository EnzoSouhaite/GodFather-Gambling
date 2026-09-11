using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardsManager : MonoBehaviour
{
    [SerializeField] private PlayerGlobalLeader _prefabPlayerGlobal;
    [SerializeField] private Transform _globalContainer;

    [SerializeField] private PlayerLastGame _prefabPlayerLast;
    [SerializeField] private Transform _lastContainer;

    [SerializeField] private Transform _worstContainer;

    [SerializeField] private Image[] _podium;
    [SerializeField] private List<Sprite> _horsesSprites = new List<Sprite>();

    private int playerCapGlob = 10;
    private int playerCapLast = 10;
    private int playerCapWorst = 5;

    private void Start()
    {
        List<SOPlayerInfo> allPlayers = AccountsManager.GetAllPlayers();

        allPlayers.Sort((a, b) => b.Balance.CompareTo(a.Balance));
        allPlayers.RemoveRange(playerCapGlob - 1, allPlayers.Count - playerCapGlob);

        PlayerGlobalLeader stat;
        foreach (SOPlayerInfo player in allPlayers)
        {
            stat = Instantiate(_prefabPlayerGlobal, _globalContainer);
            stat.Init(player.Name, player.Balance);
        }

        List<SOBetInfo> allBets = BetManager.GetAllBets();

        allBets.Sort((a, b) => b.LastAmountWon.CompareTo(a.LastAmountWon));
        allBets.RemoveRange(playerCapLast - 1, allBets.Count - playerCapLast);

        PlayerLastGame lastGame;
        foreach (SOBetInfo bet in allBets)
        {
            lastGame = Instantiate(_prefabPlayerLast, _lastContainer);
            lastGame.Init(bet.Name, bet.LastAmountWon);
        }

        List<SOPlayerInfo> worstPlayers = AccountsManager.GetAllPlayers();
        worstPlayers.Sort((a, b) => a.Balance.CompareTo(b.Balance));
        worstPlayers.RemoveRange(playerCapWorst - 1, worstPlayers.Count - playerCapWorst);

        PlayerGlobalLeader worstStat;
        foreach (SOPlayerInfo player in worstPlayers)
        {
            worstStat = Instantiate(_prefabPlayerGlobal, _worstContainer);
            worstStat.Init(player.Name, player.Balance);
        }

        BetManager.ClearBets();

        int length = Mathf.Min(_podium.Length, BetManager.winningHorses.Length);
        for (int i = 0; i < length; i++)
        {
            _podium[i].sprite = _horsesSprites[BetManager.winningHorses[i] - 1];
        }
    }
}