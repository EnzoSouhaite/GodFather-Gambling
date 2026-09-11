using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string _keyNumPlayer = "numPlayers";
    private static string _keyPlayerName = "playerName";
    private static string _keyPlayerBalance = "playerBalance";

    private void Start()
    {
        LoadGame();
    }

    public static void SaveGame()
    {
        PlayerPrefs.DeleteAll();
        List<SOPlayerInfo> players = AccountsManager.GetAllPlayers();

        int length = players.Count;
        PlayerPrefs.SetInt(_keyNumPlayer, length);

        for (int i = 0; i < length; i++)
        {
            PlayerPrefs.SetString(_keyPlayerName + i, players[i].name);
            PlayerPrefs.SetInt(_keyPlayerBalance + i, players[i].Balance);
        }
        
        PlayerPrefs.Save();
    }

    public static void LoadGame()
    {
        List<SOPlayerInfo> players = new List<SOPlayerInfo>();

        int length = PlayerPrefs.GetInt(_keyNumPlayer);

        SOPlayerInfo player;
        string name;
        int balance;
        for (int i = 0; i < length; i++)
        {
            player = ScriptableObject.CreateInstance<SOPlayerInfo>();
            name = PlayerPrefs.GetString(_keyPlayerName + i);
            balance = PlayerPrefs.GetInt(_keyPlayerBalance + i);
            player.Init(name, balance);
            players.Add(player);
        }

        AccountsManager.LoadPlayers(players);
    }
}
