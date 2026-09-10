using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public System.Action<GameObject, int> OnHorseFinished;
 
    readonly List<GameObject> _arrivalOrder = new();
 
    public IReadOnlyList<GameObject> ArrivalOrder => _arrivalOrder;
 
    void OnTriggerEnter2D(Collider2D other)
    {
        HorsePhysicsWander horse = other.GetComponent<HorsePhysicsWander>();
        if (horse == null) return;
 
        if (_arrivalOrder.Contains(other.gameObject)) return; // déjà comptabilisé
 
        _arrivalOrder.Add(other.gameObject);
        int rank = _arrivalOrder.Count;
 
        horse.StopWandering();
        other.enabled = false;
        
        Debug.Log($"[FinishLine] {other.gameObject.name} arrive {rank}{RankSuffix(rank)}");
 
        OnHorseFinished?.Invoke(other.gameObject, rank);
    }
 
    string RankSuffix(int rank) => rank == 1 ? "er" : "ème";
    
    public void ResetRace()
    {
        _arrivalOrder.Clear();
    }
}
