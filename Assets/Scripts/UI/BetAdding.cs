using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;

public class BetAdding : MonoBehaviour
{
    [SerializeField] private Text _bet;
    [SerializeField] private Text _name;

    public void AddBet()
    {
        string playerName = _name.text;

        SOPlayerInfo playerInfo = AccountsManager.AddPlayer(playerName);

        if (playerInfo == null) return;

        BetManager.AddBet(playerInfo, int.Parse(_bet.text));
        _bet.text = string.Empty;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("HorseRace");
    }
}