using UnityEngine;

[System.Serializable]
public class RacePath : MonoBehaviour
{
    [Tooltip("Nom du Chemin")] 
    public string pathName = "Chemin";
    
    [Tooltip("Points définissant le tracé, dans l'ordre : le premier = départ, le dernier = arrivée")]
    public Transform[] waypoints;
    
    [Tooltip("Largeur par défaut du couloir si 'widths' n'est pas rempli")]
    public float width = 1.5f;
 
    [Tooltip("Optionnel : largeur du couloir à CHAQUE waypoint (même taille que 'waypoints'). " +
             "Permet un chemin qui s'élargit ou se rétrécit. Laissez vide pour une largeur fixe.")]
    public float[] widths;
    
    public float GetWidthAt(int segIndex, float progress)
    {
        if (widths == null || widths.Length != waypoints.Length)
        {
            return width;
        }
 
        float widthStart = widths[segIndex];
        float widthEnd = widths[segIndex + 1];
 
        return Mathf.Lerp(widthStart, widthEnd, progress);
    }
}
