using UnityEngine;
using UnityEngine.UI;

public class SingleBet : MonoBehaviour
{
    [SerializeField] private Text _name;
    [SerializeField] private Text _bet;
    [SerializeField] private Image[] _horses;

    public void Init(string name, int bet, int[] horses)
    {
        _name.text = name;
        _bet.text = bet.ToString();

        int length = horses.Length;
        for (int i = 0;  i < length; i++)
        {
            _horses[i].sprite = HorseSelectable.GetSprite(horses[i]);
        }
    }
}
