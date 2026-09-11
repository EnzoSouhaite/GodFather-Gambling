using UnityEngine;
using UnityEngine.UI;

public class PlayerLastGame : MonoBehaviour
{
    [SerializeField] private Text _playerName;
    [SerializeField] private Text _playerBalance;

    public void Init(string name, int balance)
    {
        _playerName.text = name;
        _playerBalance.text = balance.ToString() + " $";
    }
}
