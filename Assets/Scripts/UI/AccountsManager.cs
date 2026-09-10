using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AccountsManager
{
    static private Dictionary<string, SOPlayerInfo> _players = new Dictionary<string, SOPlayerInfo>();
    static public event Action<List<SOPlayerInfo>, SOPlayerInfo> UpdatePlayers;

    public static SOPlayerInfo AddPlayer(string name)
    {
        if (DoesPlayerExist(name)) return GetPlayer(name);

        SOPlayerInfo player = ScriptableObject.CreateInstance<SOPlayerInfo>();
        player.Init(name);
        _players.Add(name, player);
        
        InvokeUpdate(player);
        return player;
    }

    public static SOPlayerInfo AddPlayer(string name, int balance)
    {
        if (DoesPlayerExist(name)) return GetPlayer(name);

        SOPlayerInfo player = ScriptableObject.CreateInstance<SOPlayerInfo>();
        player.Init(name, balance);
        _players.Add(name, player);

        InvokeUpdate(player);
        return player;
    }

    public static bool DoesPlayerExist(string name)
    {
        return _players.ContainsKey(name);
    }

    private static void InvokeUpdate(SOPlayerInfo newPlayer)
    {
        List<SOPlayerInfo> so_players = _players.Values.ToList();

        so_players.Sort((a, b) => string.Compare(a.Name, b.Name));

        UpdatePlayers?.Invoke(so_players, newPlayer);
    }

    public static SOPlayerInfo GetPlayer(string name)
    {
        return DoesPlayerExist(name) ? _players[name] : null;
    }
}