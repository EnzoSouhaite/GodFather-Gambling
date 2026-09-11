using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BetAdding : MonoBehaviour
{
    [SerializeField] private InputField _bet;
    [SerializeField] private InputField _name;

    public void AddBet()
    {
        string playerName = _name.text;

        SOPlayerInfo playerInfo = AccountsManager.AddPlayer(playerName);
        int bet = int.Parse(_bet.text);
        if (bet < 0) bet *= -1;
        BetManager.AddBet(playerInfo, bet);
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