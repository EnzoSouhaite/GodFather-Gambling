using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_BetInfo", menuName = "Scriptable Objects/SO_BetInfo")]
public class SOBetInfo : ScriptableObject
{
    private string _name = "";
    public string Name => _name;

    private int _bet = 0;
    public int Bet => _bet;

    private int[] _horses = new int[0];
    public int[] Horses => _horses;

    private int _lastAmountWon;
    public int LastAmountWon => _lastAmountWon;

    public void Init(string name, int bet, List<HorseSelectable> horses)
    {
        _name = name;
        _bet = bet;

        _horses = horses.Select(horse => horse.Number).ToArray();
    }

    public int GetNumHorseWellPlaced(int[] winHorses)
    {
        int num = 0;
        int length = Mathf.Min(winHorses.Length, _horses.Length);

        for (int i = 0; i < length; i++)
        {
            if (winHorses[i] == _horses[i])
            {
                num++;
            }
        }

        return num;
    }

    public void SetWonAmount(int amount)
    {
        _lastAmountWon = amount;
    }
}