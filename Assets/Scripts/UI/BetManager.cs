using System;
using System.Collections.Generic;
using UnityEngine;

public static class BetManager
{
    private static Dictionary<string, SOBetInfo> _bets = new Dictionary<string, SOBetInfo>();

    public static event Action<SOBetInfo> OnNewBet;

    public static void AddBet(SOPlayerInfo player, int bet)
    {
        if (_bets.ContainsKey(player.Name)) return;
        
        player.MakeTransaction(-bet);

        SOBetInfo soBet = ScriptableObject.CreateInstance<SOBetInfo>();
        soBet.Init(player.Name, bet);

        _bets.Add(player.Name, soBet);
        OnNewBet?.Invoke(soBet);
    }
}