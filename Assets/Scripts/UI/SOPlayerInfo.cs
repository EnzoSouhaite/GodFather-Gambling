using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerInfo", menuName = "Scriptable Objects/SO_PlayerInfo")]
public class SOPlayerInfo : ScriptableObject
{
    private string _name;
    private long _balance;
    public string Name => _name;
    public long Balance => _balance;

    private int _defaultBalance = 1000;

    public void Init(string name)
    {
        _name = name;
        _balance = _defaultBalance;
    }

    public void Init(string name, int balance)
    {
        _name = name;
        _balance = balance;
    }

    public void MakeTransaction(long amount)
    {
        _balance += amount;
    }
}