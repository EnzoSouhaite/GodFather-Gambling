using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AccountsManager
{
    static private Dictionary<string, SOPlayerInfo> _players = new Dictionary<string, SOPlayerInfo>();
    static public event Action<List<SOPlayerInfo>, SOPlayerInfo> UpdatePlayers;

    private static string NormalizeName(string name)
    {
        return name.Trim();
    }

    public static List<SOPlayerInfo> GetAllPlayers()
    {
        return _players.Values.ToList();
    }

    public static SOPlayerInfo AddPlayer(string name)
    {
        name = NormalizeName(name);
        if (DoesPlayerExist(name)) return GetPlayer(name);

        SOPlayerInfo player = ScriptableObject.CreateInstance<SOPlayerInfo>();
        player.Init(name);
        _players.Add(name, player);
        
        InvokeUpdate(player);
        return player;
    }

    public static SOPlayerInfo AddPlayer(string name, int balance)
    {
        name = NormalizeName(name);
        if (DoesPlayerExist(name)) return GetPlayer(name);

        SOPlayerInfo player = ScriptableObject.CreateInstance<SOPlayerInfo>();
        player.Init(name, balance);
        _players.Add(name, player);

        InvokeUpdate(player);
        return player;
    }

    public static bool DoesPlayerExist(string name)
    {
        name = NormalizeName(name);
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
        name = NormalizeName(name);
        if (DoesPlayerExist(name)) return _players[name];
        else return null;
    }

    public static void LoadPlayers(List<SOPlayerInfo> players)
    {
        _players.Clear();
        
        foreach (SOPlayerInfo player in players)
        {
            _players.Add(player.Name, player);
        }
    }
}