using UnityEngine;

public class BetList : MonoBehaviour
{
    [SerializeField] private GameObject _betPrefab;

    private void OnEnable()
    {
        BetManager.OnNewBet += OnNewBet;
    }

    private void OnDisable()
    {
        BetManager.OnNewBet -= OnNewBet;
    }

    private void OnNewBet(SOBetInfo obj)
    {
        GameObject newBet = Instantiate(_betPrefab, transform);
        SingleBet betComp = newBet.GetComponent<SingleBet>();
        betComp.Init(obj.Name, obj.Bet, obj.Horses);
    }
}