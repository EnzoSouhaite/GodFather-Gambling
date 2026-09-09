using UnityEngine;
using UnityEngine.UI;

public class SingleBet : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Text _bet;
    [SerializeField] private Text _horses;

    public void Init(string name, int bet, int[] horses)
    {
        _name.text = name;
        _bet.text = bet.ToString();

        string horsesText = "";

        int length = horses.Length;
        for (int i = 0;  i < length; i++)
        {
            horsesText += horses[i].ToString();
            if (i != length - 1) horsesText += "-";
        }

        _horses.text = horsesText;
    }
}
