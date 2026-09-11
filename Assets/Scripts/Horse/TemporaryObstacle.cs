using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TemporaryObstacle : MonoBehaviour
{
    [Header("Paramètres de l'obstacle")]
    [Tooltip("Temps en secondes pendant lequel l'obstacle reste ouvert après un contact")]
    [SerializeField] private float _disableDuration = 20f;

    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer; // Optionnel : pour rendre l'obstacle visible/invisible lors des tests

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifie si l'objet qui touche l'obstacle est bien un cheval
        HorsePhysicsWander horse = collision.gameObject.GetComponent<HorsePhysicsWander>();
        
        if (horse != null && _collider.enabled)
        {
            StartCoroutine(DisableRoutine());
        }
    }

    private IEnumerator DisableRoutine()
    {
        Debug.Log("[TemporaryObstacle] Obstacle percuté ! Ouverture du passage pour 20 secondes.");
        
        // Désactive l'obstacle
        _collider.enabled = false;
        if (_spriteRenderer != null) _spriteRenderer.enabled = false;

        // Attend 20 secondes
        yield return new WaitForSeconds(_disableDuration);

        // Réactive l'obstacle
        Debug.Log("[TemporaryObstacle] Fermeture du passage.");
        _collider.enabled = true;
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
    }

    // Permet de réinitialiser l'obstacle quand on lance une nouvelle course
    public void ResetObstacle()
    {
        StopAllCoroutines();
        if (_collider != null) _collider.enabled = true;
        if (_spriteRenderer != null) _spriteRenderer.enabled = true;
    }
}