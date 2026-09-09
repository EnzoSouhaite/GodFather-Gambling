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

    public void Init(string name, int bet)
    {
        _name = name;
        _bet = bet;
    }
}