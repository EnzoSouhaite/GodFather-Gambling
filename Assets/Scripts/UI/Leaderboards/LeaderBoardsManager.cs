using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardsManager : MonoBehaviour
{
    [SerializeField] private PlayerGlobalLeader _prefabPlayerGlobal;
    [SerializeField] private Transform _globalContainer;

    [SerializeField] private PlayerLastGame _prefabPlayerLast;
    [SerializeField] private Transform _lastContainer;

    private void Start()
    {
        List<SOPlayerInfo> allPlayers = AccountsManager.GetAllPlayers();

        allPlayers.Sort((a, b) => b.Balance.CompareTo(a.Balance));
        allPlayers.Capacity = 10;

        PlayerGlobalLeader stat;
        foreach (SOPlayerInfo player in allPlayers)
        {
            stat = Instantiate(_prefabPlayerGlobal, _globalContainer);
            stat.Init(player.Name, player.Balance);
        }

        List<SOBetInfo> allBets = BetManager.GetAllBets();

        allBets.Sort((a, b) => b.LastAmountWon.CompareTo(a.LastAmountWon));
        allBets.Capacity = 10;

        PlayerLastGame lastGame;
        foreach (SOBetInfo bet in allBets)
        {
            lastGame = Instantiate(_prefabPlayerLast, _lastContainer);
            lastGame.Init(bet.Name, bet.LastAmountWon);
        }

        BetManager.ClearBets();
    }
}