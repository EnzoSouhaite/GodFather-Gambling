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

    public void Init(string name, int bet, List<HorseSelectable> horses)
    {
        _name = name;
        _bet = bet;

        _horses = horses.Select(horse => horse.Number).ToArray();
    }

    public bool DoesWin(int[] horses)
    {
        foreach (int num in horses)
        {
            if (!_horses.Contains(num)) return false;
        }

        return true;
    }
}