using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;

public class BetAdding : MonoBehaviour
{
    [SerializeField] private InputField _bet;
    [SerializeField] private InputField _name;

    public void AddBet()
    {
        string playerName = _name.text;

        SOPlayerInfo playerInfo = AccountsManager.AddPlayer(playerName);

        BetManager.AddBet(playerInfo, int.Parse(_bet.text));
        _bet.text = "";
        _name.text = "";

        SoundManager.Instance.PlaySound(SoundEnum.ButtonGamble);
    }

    public void StartGame()
    {
        SoundManager.Instance.PlaySound(SoundEnum.ButtonRace);
        SceneManager.LoadScene("HorseRace");
    }
}