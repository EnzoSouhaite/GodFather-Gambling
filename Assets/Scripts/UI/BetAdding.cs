using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class BetAdding : MonoBehaviour
{
    [SerializeField] private Dropdown _dropdown;
    [SerializeField] private Text _bet;

    private void OnEnable()
    {
        AccountsManager.UpdatePlayers += UpdatePlayers;
    }    

    private void OnDisable()
    {
        AccountsManager.UpdatePlayers -= UpdatePlayers;
    }

    private void UpdatePlayers(List<SOPlayerInfo> allPlayers, SOPlayerInfo newPlayer)
    {
        _dropdown.ClearOptions();
        List<string> names = allPlayers.Select(player => player.Name).ToList();
        _dropdown.AddOptions(names);
        _dropdown.value = allPlayers.IndexOf(newPlayer);
    }

    public void AddBet()
    {
        string playerName = _dropdown.options[_dropdown.value].text;
        SOPlayerInfo playerInfo = AccountsManager.GetPlayer(playerName);

        if (playerInfo == null) return;

        BetManager.AddBet(playerInfo, int.Parse(_bet.text));
        _bet.text = string.Empty;
    }
}