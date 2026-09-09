using UnityEngine;
using UnityEngine.UI;

public class AddPlayerButton : MonoBehaviour
{
    [SerializeField] private Text _name;

    public void AddPlayer()
    {
        if (string.IsNullOrEmpty(_name.text)) return;

        AccountsManager.AddPlayer(_name.text);
        _name.text = string.Empty;
    }
}
